using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEngine.EventSystems;

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
    public static GameObject SelectedBuilding;

    private void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
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

    void BuildBlock()
    {
        if (!inBuildMode)
            return;
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            if (IsPointerOverUIObject())
            {
                return;
            }

            Vector2 location = Input.mousePosition != Vector3.zero ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
            location = GameObjectCache.Camera.ScreenToWorldPoint(location);
            Vector2Int gridLoc = Map.WorldPointToGridPoint(location);
            if (gridLoc[1] < 0 || gridLoc.y > (Map.Buildings.GetLength(1) - 1) ||
                gridLoc[0] < 0 || gridLoc.x > (Map.Buildings.GetLength(0) - 1))
            {
                return;
            }

            if (!deleteMode)
            {
                if (!Purchaser.CanBuy(SelectedBuilding.GetComponent<Building>().BuildCost))
                {
                    return;
                }

                SelectedBuilding.GetComponent<Building>().Position = gridLoc;
                if (!Map.CanPlaceBuildingHere(SelectedBuilding.GetComponent<Building>()))
                {
                    Console.WriteLine("Invalid place to put building");
                    return;
                }

                GameObject newBuilding = InstantiateBuilding(gridLoc);
                Purchaser.Buy(SelectedBuilding.GetComponent<Building>().BuildCost);
                Map.AddBuildingToMap(newBuilding.GetComponent<Building>(), gridLoc);
            }
            else
            {
                Building building = Map.Buildings[gridLoc.x, gridLoc.y].GetComponent<Building>();
                if (building != null)
                {
                    building.Delete();
                    Map.TellAllZombiesToGetNewPath();
                }
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private GameObject InstantiateBuilding(Vector2Int gridLoc)
    {
        Building building = SelectedBuilding.GetComponent<Building>();
        Vector2 buildingPos = building.GetWorldPointFromGridPoint(gridLoc);
        GameObject inst = Instantiate(SelectedBuilding,
                                      buildingPos,
                                      new Quaternion());
        return inst;
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

    public void AddWood(int amount)
    {
        this.woodCount += amount;
        woodLabel.text = woodCount.ToString();
    }
}
