using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapBuilder : MonoBehaviour {

    // Private fields
    float blockSize;
    float wallZAxis = 0f;
    private GameObject selectedBlock;

    // Pulic Fields
    public GameObject wallSegment;
    public int[,] grid;
    public GameObject water;
    public GameObject brush;
    public GameObject chasm;
    public GameObject wire;
    public string mapName;

    private int blockNum = 0;

    private void Awake()
    {
        this.grid = new int[16, 32];
    }

    // Use this for initialization
    void Start () {
        SelectBrush();
	}
	
	// Update is called once per frame
	void Update () {
        BuildBlock();
    }

    void Save()
    {
        string path = "Assets/Maps/" + this.mapName;
        StreamWriter writer = new StreamWriter(path, false);
        foreach (int block in grid)
        {
            writer.Write(block + " ");
        }
        writer.Close();
    }

    void BuildBlock()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            Vector2 location = Input.mousePosition != Vector3.zero ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
            location = Camera.main.ScreenToWorldPoint(location);
            int[] gridLoc = WorldPointToGridPoint(location);

            if (grid[gridLoc[1], gridLoc[0]] > 0)
            {
                return;
            }
            grid[gridLoc[1], gridLoc[0]] = blockNum;

            Instantiate(selectedBlock, GridPointToWorldPoint(gridLoc), new Quaternion());
        }
    }

    public int[] WorldPointToGridPoint(Vector2 worldPoint)
    {
        int[] loc = new int[2] { (int)((worldPoint.x + 8 - .25f) * 2), (int)((worldPoint.y + 3 - .25f) * 2) };

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

    public void SelectChasm()
    {
        this.selectedBlock = chasm;
        this.blockNum = 4;
    }

    public void SelectWater()
    {
        this.selectedBlock = water;
        this.blockNum = 1;
    }

    public void SelectBrush()
    {
        this.selectedBlock = brush;
        this.blockNum = 2;
    }
    
    public void SelectWire()
    {
        this.selectedBlock = wire;
        this.blockNum = 3;
    }


    
}
