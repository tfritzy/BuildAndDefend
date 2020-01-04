using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    private Dictionary<TileType, GameObject> tileMap;

    private GameObject zombieSpawner;

    void Start()
    {
        this.zombieSpawner = Resources.Load<GameObject>($"{FilePaths.Zombies}/ZombieSpawner");
        tileMap = new Dictionary<TileType, GameObject>();
        foreach (TileType type in Enum.GetValues(typeof(TileType)))
        {
            tileMap.Add(type, Resources.Load<GameObject>($"{FilePaths.Terrain}/{type}"));
        }
        LoadMap(Player.data.vals.CurrentLevel);
    }

    public void LoadMap(string mapName)
    {
        MapDAO map = ReadMapFile(mapName);
        LoadGrid(map);
        LoadSpawners(map);

    }

    private MapDAO ReadMapFile(String mapName)
    {
        string path = $"{FilePaths.Maps}/{mapName}.json";
        StreamReader reader = new StreamReader(path);
        string jsonMap = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<MapDAO>(jsonMap); ;
    }

    private void LoadGrid(MapDAO map)
    {
        for (int i = 0; i < map.grid.Length - 1; i++)
        {
            int x = i % 32;
            int y = i / 32;
            TileType value = map.grid[i];
            Map.Grid[y, x] = value;
            PlaceBlock(value, x, y);
        }
    }

    private void LoadSpawners(MapDAO map)
    {
        GameObject spawnerParent = new GameObject();
        Instantiate(spawnerParent, Vector3.zero, new Quaternion(), null);
        foreach (ZombieSpawnerDAO spawner in map.zombieSpawners)
        {
            Vector3 spawnerPos = Map.GridPointToWorldPoint(spawner.Pos);
            GameObject spawnerInst = Instantiate(this.zombieSpawner, spawnerPos, new Quaternion(), spawnerParent.transform);
            spawnerInst.GetComponent<ZombieSpawner>().SpawnRate = spawner.SpawnRate;
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