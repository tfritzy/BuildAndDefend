using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapBuilder : MonoBehaviour
{
    public string mapName;
    public GameObject TileSelectButton;

    private float blockSize;
    public static GameObject SelectedBlock;
    public static EnvironmentTileType SelectedTileType = EnvironmentTileType.Water;

    private static GameObject tileParent;
    public static GameObject TileParent {
        get {
            if (tileParent == null){
                tileParent = GameObject.Find("TileParent");
            }
            return tileParent;
        }
    }

    private void Awake()
    {
        Map.Environment = new EnvironmentTile[80, 60];
        SelectedTileType = EnvironmentTileType.Grass;
        SelectedBlock = Resources.Load<GameObject>($"{FilePaths.Terrain}/{SelectedTileType}");
        SelectedTileType = EnvironmentTileType.Grass;
        for (int i = 0; i < Map.Environment.GetLength(0); i++)
        {
            for (int j = 0; j < Map.Environment.GetLength(1); j++)
            {
                GameObject inst = Instantiate(
                    SelectedBlock,
                    Map.GridPointToWorldPoint(new Vector2(i, j)),
                    new Quaternion(),
                    TileParent.transform);
                Map.Environment[i, j] = inst.GetComponent<EnvironmentTile>();
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        GameObject selectionBox = GameObject.Find("SelectionBox");
        float width = selectionBox.GetComponent<RectTransform>().rect.width;
        int buttonsPerRow = 11;
        float distBetweenButtons = 2f;
        Vector2 selectionBoxWorldPos = selectionBox.transform.position;
        float xPos = selectionBoxWorldPos.x;
        float yPos = selectionBoxWorldPos.y;
        EnvironmentTileType[] tileTypes = (EnvironmentTileType[])Enum.GetValues(typeof(EnvironmentTileType));
        for (int i = 0; i < tileTypes.Length; i++)
        {
            EnvironmentTileType tile = tileTypes[i];
            GameObject tileSelectButton = Instantiate(TileSelectButton, new Vector3(xPos, yPos, 0), new Quaternion(), selectionBox.transform);
            tileSelectButton.transform.Find("Text").GetComponent<Text>().text = tileTypes[i].ToString();
            tileSelectButton.GetComponent<BlockSelector>().Type = tile;
            xPos += distBetweenButtons;
            if (i % buttonsPerRow == 0 && i != 0)
            {
                xPos = 0;
                yPos -= distBetweenButtons;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        BuildBlock();
    }

    public void Save()
    {
        MapDAO mapSave = new MapDAO();
        mapSave.width = Map.Environment.GetLength(0);
        mapSave.height = Map.Environment.GetLength(1);
        mapSave.name = this.mapName;
        List<EnvironmentTileType> tiles = new List<EnvironmentTileType>();
        foreach (EnvironmentTile block in Map.Environment)
        {
            if (block == null)
            {
                tiles.Add(EnvironmentTileType.Nothing);
            }
            else
            {
                tiles.Add(block.Type);
            }
        }
        mapSave.environment = tiles.ToArray();

        string path = $"{FilePaths.Maps}/{this.mapName}.json";
        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(JsonConvert.SerializeObject(mapSave));
        writer.Close();
    }

    void BuildBlock()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            Vector2 location = Input.mousePosition != Vector3.zero ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
            location = GameObjectCache.Camera.ScreenToWorldPoint(location);
            Vector2Int gridLoc = Map.WorldPointToGridPoint(location);
            if (SelectedTileType == EnvironmentTileType.Nothing)
            {
                Destroy(Map.Environment[gridLoc.x, gridLoc.y]?.gameObject);
                Map.Environment[gridLoc.x, gridLoc.y] = null;
            }
            else
            {
                if (Map.Environment[gridLoc.x, gridLoc.y] == null ||
                    Map.Environment[gridLoc.x, gridLoc.y].Type != SelectedTileType)
                {
                    GameObject inst = Instantiate(
                        SelectedBlock,
                        Map.GridPointToWorldPoint(gridLoc),
                        new Quaternion(),
                        TileParent.transform);
                    Map.Environment[gridLoc.x, gridLoc.y] = inst.GetComponent<EnvironmentTile>();
                }
            }
        }
    }
}
