﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Builder : MonoBehaviour {

    // Private fields
    float blockSize;
    private Text woodLabel;
    private float lastNewPathQueuePopTime;
    private Building woodWall;
    private Building lamp;
    private Building selectedBuilding;
    private Transform gridParent;

    // Public Fields
    public bool inBuildMode = false;
    public int woodCount = 500;
    public bool deleteMode = false;
    public GameObject water;
    public GameObject brush;
    public GameObject chasm;
    public GameObject wire;
    public GameObject gridLine;
    
    private void Awake()
    {
        Map.Grid = new byte[16, 32];
        LoadMap(Player.data.vals.CurrentLevel);
    }

    // Use this for initialization
    void Start()
    { 
        this.woodLabel = GameObject.Find("WoodValueLabel").GetComponent<Text>();
        woodLabel.text = woodCount.ToString();
        this.woodWall = Resources.Load<GameObject>("Gameobjects/Buildings/WallSegment").GetComponent<WoodWall>();
        this.lamp = Resources.Load<GameObject>("Gameobjects/Buildings/Lamp").GetComponent<Lamp>();
        selectedBuilding = woodWall;
        this.gridParent = GameObject.Find("BuildGrid").transform;
    }

    public void ToggleBuildMode()
    {
        inBuildMode = !inBuildMode;
        if (inBuildMode)
        {
            SetupBuildGrid();
            Time.timeScale = 0f;
        } else
        {
            Time.timeScale = 1f;
            RemoveBuildGrid();
            Map.TellAllZombiesToGetNewPath();
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
    }

    void BuildBlock()
    {
        if (!inBuildMode)
            return;
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            Vector2 location = Input.mousePosition != Vector3.zero ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
            location = Camera.main.ScreenToWorldPoint(location);
            int[] gridLoc = Map.WorldPointToGridPoint(location);
            if (gridLoc[1] < 1 || gridLoc[1] > 14 || gridLoc[0] < 1 || gridLoc[0] > 30)
            {
                return;
            }

            if (!deleteMode)
            { 
                if (this.woodCount < selectedBuilding.WoodCost)
                {
                    return;
                }

                if (Map.Grid[gridLoc[1], gridLoc[0]] > 0)
                {
                    return;
                }

                Map.Grid[gridLoc[1], gridLoc[0]] = 5;
                NotifyPathTakersOfWallChange(gridLoc[1], gridLoc[0]);

                GameObject inst = Instantiate(selectedBuilding.GetStructure(), 
                                              Map.GridPointToWorldPoint(gridLoc), 
                                              new Quaternion());
                inst.name = "Block" + gridLoc[0] + "," + gridLoc[1];
                AddWood(-1 * selectedBuilding.WoodCost);
                OnWallBuild();
            } else
            {
                GameObject building = GameObject.Find("Block" + gridLoc[0] + "," + gridLoc[1]);
                if (building != null)
                {
                    building.SendMessage("Delete");
                    Map.Grid[gridLoc[1], gridLoc[0]] = 0;
                    Map.TellAllZombiesToGetNewPath();
                }
            }
        }
    }

    private void SetupBuildGrid()
    {
        for (int i = 0; i < 64; i++)
        {
            Instantiate(gridLine, new Vector3(0, i / 2f - 15.25f, 0), Quaternion.Euler(0, 0, 90), gridParent);
            Instantiate(gridLine, new Vector3(i/2f - 15.25f, 0, 0), new Quaternion(), gridParent);
        }
    }

    private void RemoveBuildGrid()
    {
        foreach(GameObject line in GameObject.FindGameObjectsWithTag("BuildGridLine"))
        {
            Destroy(line);
        }
    }

    private void OnWallBuild()
    {
        WallBreaker[] wallBreakers = GameObject.FindObjectsOfType<WallBreaker>();
        foreach (WallBreaker wallBreaker in wallBreakers)
        {
            wallBreaker.NotifyOfPathBreak();
        }
    }

    private void NotifyPathTakersOfWallChange(int x, int y)
    {
        if (!Map.PathTakers.ContainsKey(x + "," + y)){
            return;
        }
        foreach (Zombie z in Map.PathTakers[x + "," + y])
        {
            z.NotifyOfPathBreak();
        }
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
            Map.Grid[y , x] = value;
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

        Instantiate(selectedBlock, Map.GridPointToWorldPoint(new int[] { x, y }), new Quaternion());
    }

    void SelectWoodWall()
    {
        this.selectedBuilding = woodWall;
    }

    void SelectLamp()
    {
        this.selectedBuilding = lamp;
    }
}
