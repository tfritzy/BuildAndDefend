using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using System;

public class Builder : MonoBehaviour
{

    // Private fields
    private float blockSize;
    private Text woodLabel;
    private float lastNewPathQueuePopTime;
    private Transform gridParent;

    // Public Fields
    public bool inBuildMode = false;
    public int woodCount = 6000;
    public bool deleteMode = false;
    public GameObject gridLine;
    public const int xGridSize = 40;
    public const int yGridSize = 40;
    public static Dictionary<BuildingType, GameObject> Buildings = new Dictionary<BuildingType, GameObject>();    
    public static GameObject SelectedBuilding;

    private void Awake()
    {
        Map.Grid = new TileType[xGridSize, yGridSize];
    }

    // Use this for initialization
    void Start()
    {
        LoadBuildings();
        this.woodLabel = GameObject.Find("WoodValueLabel").GetComponent<Text>();
        woodLabel.text = woodCount.ToString();
        this.gridParent = GameObject.Find("BuildGrid").transform;
    }

    public void ToggleBuildMode()
    {
        inBuildMode = !inBuildMode;
        if (inBuildMode)
        {
            SetupBuildGrid();
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            RemoveBuildGrid();
            Map.TellAllZombiesToGetNewPath();
        }
    }

    private void LoadBuildings(){
        foreach (BuildingType type in Enum.GetValues(typeof(BuildingType)))
        {
            Buildings.Add(type, Resources.Load<GameObject>($"{FilePaths.Buildings}/{type}"));
        }
    }

    private void ToggleDeleteMode()
    {
        this.deleteMode = !deleteMode;
        Debug.Log("DELETE MODE ACTIVE: " + deleteMode);
    }

    // Update is called once per frame
    void Update()
    {
        BuildBlock();
    }


    private void BuildTower(Vector2Int gridLoc, BuildingType buildingType)
    {
        GameObject building = Resources.Load<GameObject>($"{FilePaths.Towers}/{buildingType}");
        Instantiate(building, Map.GridPointToWorldPoint(gridLoc), new Quaternion(), null);
        Map.Towers.Add(gridLoc.ToStr(), building);
    }

    void BuildBlock()
    {
        if (!inBuildMode)
            return;
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            Vector2 location = Input.mousePosition != Vector3.zero ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
            location = Camera.main.ScreenToWorldPoint(location);
            Vector2Int gridLoc = Map.WorldPointToGridPoint(location);
            if (gridLoc[1] < 0 || gridLoc[1] > (xGridSize - 1) || gridLoc[0] < 0 || gridLoc[0] > (yGridSize - 1))
            {
                return;
            }

            if (!deleteMode)
            {
                if (this.woodCount < SelectedBuilding.GetComponent<Building>().WoodCost)
                {
                    return;
                }

                if (Map.Grid[gridLoc[1], gridLoc[0]] > 0)
                {
                    return;
                }

                Map.Grid[gridLoc[1], gridLoc[0]] = TileType.Wall;
                NotifyPathTakersOfWallChange(gridLoc[1], gridLoc[0]);

                GameObject inst = Instantiate(SelectedBuilding,
                                              Map.GridPointToWorldPoint(gridLoc),
                                              new Quaternion());
                inst.name = "Block" + gridLoc[0] + "," + gridLoc[1];
                AddWood(-1 * SelectedBuilding.GetComponent<Building>().WoodCost);
                OnWallBuild();
            }
            else
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
            Instantiate(gridLine, new Vector3(i / 2f - 15.25f, 0, 0), new Quaternion(), gridParent);
        }
    }

    private void RemoveBuildGrid()
    {
        foreach (GameObject line in GameObject.FindGameObjectsWithTag("BuildGridLine"))
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
        if (!Map.PathTakers.ContainsKey(x + "," + y))
        {
            return;
        }
        foreach (Zombie z in Map.PathTakers[x + "," + y])
        {
            z.NotifyOfPathBreak();
        }
    }

    public void AddWood(int amount)
    {
        this.woodCount += amount;
        woodLabel.text = woodCount.ToString();
    }
}
