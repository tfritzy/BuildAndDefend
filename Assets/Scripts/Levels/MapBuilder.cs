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
    public static TileType SelectedTileType = TileType.Water;

    private void Awake()
    {
        Map.Grid = new TileType[16, 32];
    }

    // Use this for initialization
    void Start()
    {
        GameObject selectionBox = GameObject.Find("SelectionBox");
        float width = selectionBox.GetComponent<RectTransform>().rect.width;
        int buttonsPerRow = 11;
        float distBetweenButtons = 1f;
        Vector2 selectionBoxWorldPos = Camera.main.ScreenToWorldPoint(selectionBox.transform.position);
        float xPos = selectionBoxWorldPos.x + 10; 
        float yPos = selectionBoxWorldPos.y + .6f;
        TileType[] tileTypes = (TileType[])Enum.GetValues(typeof(TileType));
        for (int i = 0; i < tileTypes.Length; i++){
            TileType tile = tileTypes[i];
            GameObject tileSelectButton = Instantiate(TileSelectButton, new Vector3(xPos, yPos, 0), new Quaternion(), selectionBox.transform);
            tileSelectButton.GetComponent<BlockSelector>().Type = tile;
            xPos += distBetweenButtons;
            if (i % buttonsPerRow == 0){
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
        List<TileType> tiles = new List<TileType>();
        foreach (TileType block in Map.Grid)
        {
            tiles.Add(block);
        }
        mapSave.grid = tiles.ToArray();
     
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
            location = Camera.main.ScreenToWorldPoint(location);
            int[] gridLoc = Map.WorldPointToGridPoint(location);

            if (Map.Grid[gridLoc[1], gridLoc[0]] > 0)
            {
                return;
            }
            Map.Grid[gridLoc[1], gridLoc[0]] = SelectedTileType;

            Instantiate(SelectedBlock, Map.GridPointToWorldPoint(gridLoc), new Quaternion());
        }
    }
}
