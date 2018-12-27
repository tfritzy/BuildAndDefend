using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Builder : MonoBehaviour {

    // Private fields
    float blockSize;
    private HashSet<Zombie> needNewPaths;
    private Text woodLabel;
    private LinkedList<Zombie> zombiesThatNeedNewPath;
    private float lastNewPathQueuePopTime;
    private Building woodWall;
    private Building lamp;
    private Building selectedBuilding;
    // Pulic Fields

    public byte[,] grid;
    public Dictionary<string, HashSet<Zombie>> pathTakers;
    public bool inBuildMode = false;
    public int woodCount = 500;
    public bool deleteMode = false;
    public GameObject water;
    public GameObject brush;
    public GameObject chasm;
    public GameObject wire;
    

    private void Awake()
    {
        this.grid = new byte[16, 32];
        pathTakers = new Dictionary<string, HashSet<Zombie>>();
        LoadMap("Lowland");
    }

    // Use this for initialization
    void Start()
    { 
        this.needNewPaths = new HashSet<Zombie>();
        this.woodLabel = GameObject.Find("WoodValueLabel").GetComponent<Text>();
        woodLabel.text = woodCount.ToString();
        zombiesThatNeedNewPath = new LinkedList<Zombie>();
        // Construct building objects.
        this.woodWall = new WoodWall();
        this.lamp = new Lamp();
        selectedBuilding = woodWall;
        Debug.Log("SelectedBuliding: " + selectedBuilding.structure);

    }

    private void ToggleBuildMode()
    {
        inBuildMode = !inBuildMode;
        if (inBuildMode)
            Time.timeScale = 0f;
        else
        {
            Time.timeScale = 1f;
            ProcessPathsNeeded();
        }
    }

    private void ToggleDeleteMode()
    {
        
        this.deleteMode = !deleteMode;
        Debug.Log("DELETE MODE ACTIVE: " + deleteMode);
    }

	// Update is called once per frame
	void Update () {
        BuildBlock();
        ProcessUpdateQueue();
    }

    void ProcessUpdateQueue()
    {
        //Debug.Log(zombiesThatNeedNewPath.Count + " still need to be updated.");
        if (zombiesThatNeedNewPath.Count > 0 && Time.time > lastNewPathQueuePopTime + .05f)
        {
            Zombie z = zombiesThatNeedNewPath.First.Value;
            if (z != null)
                z.RestartPath();

            zombiesThatNeedNewPath.RemoveFirst();
            lastNewPathQueuePopTime = Time.time;
        }
    }

    public void FreeGridLoc(Vector3 pos)
    {
        int[] gridLoc = WorldPointToGridPoint((Vector2)pos);
        grid[gridLoc[1], gridLoc[0]] = 0;
    }

    void BuildBlock()
    {
        if (!inBuildMode)
            return;
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            Vector2 location = Input.mousePosition != Vector3.zero ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
            location = Camera.main.ScreenToWorldPoint(location);
            int[] gridLoc = WorldPointToGridPoint(location);
            if (gridLoc[1] < 1 || gridLoc[1] > 14 || gridLoc[0] < 1 || gridLoc[0] > 30)
            {
                return;
            }

            if (!deleteMode)
            { 
                if (this.woodCount < selectedBuilding.woodCost)
                {
                    return;
                }

                if (grid[gridLoc[1], gridLoc[0]] > 0)
                {
                    return;
                }

                grid[gridLoc[1], gridLoc[0]] = 5;
                NotifyZombieSubs(gridLoc);
                GameObject inst = Instantiate(selectedBuilding.structure, 
                                              GridPointToWorldPoint(gridLoc), 
                                              new Quaternion());
                inst.name = "Block" + gridLoc[0] + "," + gridLoc[1];
                AddWood(-1 * selectedBuilding.woodCost);
                OnWallBuild();
            } else
            {
                GameObject asdf = GameObject.Find("Block" + gridLoc[0] + "," + gridLoc[1]);
                Debug.Log("block del: " + asdf);
                Destroy(asdf);
                grid[gridLoc[1], gridLoc[0]] = 0;
                NotifyAllZombies();

            }
        }
    }

    private void ProcessPathsNeeded()
    {
        HashSet<Zombie> finished = new HashSet<Zombie>();
        foreach (Zombie zombie in needNewPaths)
        {
            if (finished.Contains(zombie))
            {
                continue;
            }
            if (zombie == null)
            {
                continue;
            }
            List<Vector2> newPath = zombie.RestartPath();
            finished.Add(zombie);
            Collider2D[] nearbyZombs = Physics2D.OverlapCircleAll(zombie.transform.position, 1f);
            foreach (Collider2D col in nearbyZombs)
            {
                if (col.tag != "Zombie")
                    continue;
                if (col == null)
                {
                    continue;
                }
                Zombie colZomb = col.GetComponent<Zombie>();
                if (finished.Contains(colZomb))
                    continue;
                colZomb.SetPath(newPath);
                finished.Add(colZomb);
            }
        }
        needNewPaths = new HashSet<Zombie>();
    }

    private void OnWallBuild()
    {
        WallBreaker[] wallBreakers = GameObject.FindObjectsOfType<WallBreaker>();
        foreach (WallBreaker wallBreaker in wallBreakers)
        {
            needNewPaths.Add(wallBreaker);
        }
    }

    private void NotifyZombieSubs(int[] gridLoc)
    {
        string key = gridLoc[0] + "," + gridLoc[1];
        if (!pathTakers.ContainsKey(key))
        {
            pathTakers.Add(key, new HashSet<Zombie>());
        }
        HashSet<Zombie> subs = new HashSet<Zombie>(pathTakers[key]);
        foreach(Zombie zombie in subs)
        {
            needNewPaths.Add(zombie);
        }
    }

    private void NotifyPathTakersOfNewWall(int x, int y)
    {
        HashSet<Zombie> subs = new HashSet<Zombie>();
        pathTakers.TryGetValue(x + "," + y, out subs);
        foreach (Zombie z in subs)
        {
            z.RestartPath();
        }
    }

    public int[] WorldPointToGridPoint(Vector2 worldPoint)
    {
        int[] loc = new int[2] { (int)((worldPoint.x + 8 - .25f) * 2), (int)((worldPoint.y + 3 - .25f) * 2) };
        if (loc[0] < 0)
            loc[0] = 0;
        if (loc[0] > grid.GetLength(1) - 1)
            loc[0] = grid.GetLength(1) - 1;
        if (loc[1] < 0)
            loc[1] = 0;
        if (loc[1] > grid.GetLength(0) - 1)
            loc[1] = grid.GetLength(0) - 1;
        return loc;
    }

    public Vector2 GridPointToWorldPoint(Vector2 gridLoc)
    {
        Vector3 loc = new Vector3(((float)gridLoc[0] - 16) / 2f + .5f,
                                  ((float)gridLoc[1] - 6f) / 2f + .5f);
        
        return loc;
    }

    public Vector2 GridPointToWorldPoint(int[] gridLoc)
    {
        Vector2 loc = new Vector2( ((float)gridLoc[0] - 16) / 2f + .5f, 
                                   ((float)gridLoc[1] - 6f) / 2f + .5f);

        return loc;
    }

    public void LoadMap(string mapName)
    {
        string path = "Assets/Maps/" + mapName;
        StreamReader reader = new StreamReader(path);
        string[] strMap = reader.ReadLine().Split(' ');
        for (int i = 0; i < strMap.Length-1; i++)
        {
            int x = i % 32;
            int y = i / 32;
            byte value = byte.Parse(strMap[i]);
            grid[y , x] = value;
            PlaceBlock(value, x, y);
        }
    }

    public void AddWood(int amount)
    {
        this.woodCount += amount;
        woodLabel.text = woodCount.ToString();
    }

    private void PlaceBlock(int type, int x, int y)
    {
        if (type == 0)
            return;
        GameObject selectedBlock = water;
        if (type == 2)
        {
            selectedBlock = brush;
        } else if (type == 3)
        {
            selectedBlock = wire;
        }else if (type == 4)
        {
            selectedBlock = chasm;
        }

        Instantiate(selectedBlock, GridPointToWorldPoint(new int[] { x, y }), new Quaternion());


    }

    void SelectWoodWall()
    {
        this.selectedBuilding = woodWall;
    }

    void SelectLamp()
    {
        this.selectedBuilding = lamp;
    }

    private void NotifyAllZombies()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        for (int i = 0; i < zombies.Length; i++)
        {
            this.zombiesThatNeedNewPath.AddLast(zombies[i].GetComponent<Zombie>());
        }
    }


}
