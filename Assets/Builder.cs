using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Builder : MonoBehaviour {

    // Private fields
    float blockSize;
    float wallZAxis = 0f;
    private HashSet<Zombie> needNewPaths;
    private Text woodLabel;

    // Pulic Fields
    public GameObject wallSegment;
    public bool[,] grid;
    public Dictionary<string, HashSet<Zombie>> pathTakers;
    public bool inBuildMode = false;
    public int woodCount = 5;

    private void Awake()
    {
        this.grid = new bool[32, 64];
        pathTakers = new Dictionary<string, HashSet<Zombie>>();
    }

    // Use this for initialization
    void Start()
    {
        this.needNewPaths = new HashSet<Zombie>();
        this.woodLabel = GameObject.Find("WoodValueLabel").GetComponent<Text>();
        woodLabel.text = woodCount.ToString();
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

    
	
	// Update is called once per frame
	void Update () {
        BuildBlock();
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
            if (gridLoc[0] < 1 || gridLoc[0] > 30 || gridLoc[1] < 1 || gridLoc[1] > 14)
            {
                return;
            }
            if (this.woodCount <= 0)
            {
                return;
            }
            if (grid[gridLoc[0], gridLoc[1]])
            {
                return;
            }
            grid[gridLoc[0], gridLoc[1]] = true;
            NotifyZombieSubs(gridLoc);
            Instantiate(wallSegment, GridPointToWorldPoint(gridLoc), new Quaternion());

            this.woodCount -= 1;
            woodLabel.text = woodCount.ToString();

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
        Vector3 loc = new Vector3(((float)gridLoc[0] - 16) / 2f + .5f, ((float)gridLoc[1] - 6f) / 2f + .5f);
        
        return loc;
    }

    public Vector2 GridPointToWorldPoint(int[] gridLoc)
    {
        Vector3 loc = new Vector3(((float)gridLoc[0] - 16) / 2f + .5f, ((float)gridLoc[1] - 6f) / 2f + .5f);

        return loc;
    }
}
