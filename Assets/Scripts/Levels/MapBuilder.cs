using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;

public class MapBuilder : MonoBehaviour
{
    public string mapName;
    public GameObject TileSelectButton;

    private float blockSize;
    public static GameObject SelectedBlock;
    public static EnvironmentTileType SelectedTileType = EnvironmentTileType.Water;

    private void Awake()
    {
        Map.Environment = new EnvironmentTile[16, 32];
    }

    // Use this for initialization
    void Start()
    {
        GameObject selectionBox = GameObject.Find("SelectionBox");
        float width = selectionBox.GetComponent<RectTransform>().rect.width;
        int buttonsPerRow = 11;
        float distBetweenButtons = 1f;
        Vector2 selectionBoxWorldPos = GameObjectCache.Camera.ScreenToWorldPoint(selectionBox.transform.position);
        float xPos = selectionBoxWorldPos.x + 10;
        float yPos = selectionBoxWorldPos.y + .6f;
        EnvironmentTileType[] tileTypes = (EnvironmentTileType[])Enum.GetValues(typeof(EnvironmentTileType));
        for (int i = 0; i < tileTypes.Length; i++)
        {
            EnvironmentTileType tile = tileTypes[i];
            GameObject tileSelectButton = Instantiate(TileSelectButton, new Vector3(xPos, yPos, 0), new Quaternion(), selectionBox.transform);
            tileSelectButton.GetComponent<BlockSelector>().Type = tile;
            xPos += distBetweenButtons;
            if (i % buttonsPerRow == 0)
            {
                xPos = distBetweenButtons / 2;
                yPos += distBetweenButtons;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        BuildBlock();
    }

    void Save()
    {
        MapDAO mapSave = new MapDAO();
        List<EnvironmentTileType> tiles = new List<EnvironmentTileType>();
        foreach (EnvironmentTile block in Map.Environment)
        {
            tiles.Add(block.Type);
        }
        mapSave.environment = tiles.ToArray();

        string path = $"{FilePaths.Maps}/{this.mapName}";
        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(JsonConvert.SerializeObject(mapSave));
        writer.Close();
    }

    void BuildBlock()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            Vector2 location = Input.mousePosition != Vector3.zero ? (Vector2)Input.mousePosition : Input.GetTouch(0).position;
            location = GameObjectCache.Camera.ScreenToWorldPoint(location);
            Vector2Int gridLoc = Map.WorldPointToGridPoint(location);

            if (Map.Environment[gridLoc.y, gridLoc.x].Type != EnvironmentTileType.Nothing)
            {
                return;
            }
            Map.Environment[gridLoc.y, gridLoc.x] = SelectedBlock.GetComponent<EnvironmentTile>();

            Instantiate(SelectedBlock, Map.GridPointToWorldPoint(gridLoc), new Quaternion());
        }
    }
}
