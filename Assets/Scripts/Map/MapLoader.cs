using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    private Dictionary<TileType, GameObject> tileMap;
    private const string tileFilePath = "Gameobjects/Terrain/";
    void Start()
    {
        tileMap = new Dictionary<TileType, GameObject>();
        foreach (TileType type in Enum.GetValues(typeof(TileType))){
            Debug.Log($"tileFilePath/{type}");
            tileMap.Add(type, Resources.Load<GameObject>(tileFilePath + type));
        }
        LoadMap(Player.data.vals.CurrentLevel);
    }

    public void LoadMap(string mapName)
    {
        string path = "Assets/Maps/" + mapName + ".json";
        StreamReader reader = new StreamReader(path);
        string jsonMap = reader.ReadToEnd();
        MapDAO map = JsonConvert.DeserializeObject<MapDAO>(jsonMap);
        for (int i = 0; i < map.grid.Length - 1; i++)
        {
            int x = i % 32;
            int y = i / 32;
            TileType value = map.grid[i];
            Map.Grid[y, x] = value;
            PlaceBlock(value, x, y);
        }
    }

    private void PlaceBlock(TileType tile, int x, int y)
    {
        if (tile == 0)
            return;

        GameObject selectedBlock = tileMap[tile];
        
        Instantiate(selectedBlock, Map.GridPointToWorldPoint(new int[] { x, y }), new Quaternion());
    }
}