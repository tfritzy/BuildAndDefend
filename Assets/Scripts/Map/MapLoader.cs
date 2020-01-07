using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    private Dictionary<EnvironmentTileType, GameObject> tileMap;
    private GameObject zombieSpawner;
    public const int xGridSize = 40;
    public const int yGridSize = 40;

    void Awake()
    {
        ResetMapData();
    }

    void Start()
    {
        this.zombieSpawner = Resources.Load<GameObject>($"{FilePaths.Zombies}/ZombieSpawner");
        tileMap = new Dictionary<EnvironmentTileType, GameObject>();
        foreach (EnvironmentTileType type in Enum.GetValues(typeof(EnvironmentTileType)))
        {
            tileMap.Add(type, Resources.Load<GameObject>($"{FilePaths.Terrain}/{type}"));
        }
        LoadMap(Player.Data.vals.CurrentLevel);
    }

    public void LoadMap(string mapName)
    {
        ResetMapData();
        MapDAO map = ReadMapFile(mapName);
        LoadGrid(map);
        LoadSpawners(map);
    }

    public void ResetMapData()
    {
        Map.Environment = new EnvironmentTile[xGridSize, yGridSize];
        Map.Buildings = new Building[xGridSize, yGridSize];
        Map.PathingGrid = new PathableType[xGridSize, yGridSize];
        Map.Spawners = new Dictionary<string, GameObject>();
        Map.BuildingsDict = new Dictionary<string, GameObject>();
        Map.PathingGrid = new PathableType[yGridSize, xGridSize];
        Map.Buildings = new Building[yGridSize, xGridSize];
        Map.Environment = new EnvironmentTile[yGridSize, xGridSize];
        Map.Buildings = new Building[yGridSize, xGridSize];
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
        GameObject tileParent = new GameObject();
        tileParent.name = "Tiles";
        Instantiate(tileParent, Vector3.zero, new Quaternion(), null);
        for (int i = 0; i < map.environment.Length - 1; i++)
        {
            int x = i % 32;
            int y = i / 32;
            EnvironmentTileType value = map.environment[i];
            PlaceBlock(value, x, y, tileParent.transform);
        }
    }

    private void LoadSpawners(MapDAO map)
    {
        GameObject spawnerParent = new GameObject();
        spawnerParent.name = "Spawners";
        Instantiate(spawnerParent, Vector3.zero, new Quaternion(), null);
        foreach (ZombieSpawnerDAO spawner in map.zombieSpawners)
        {
            Vector3 spawnerPos = Map.GridPointToWorldPoint(spawner.Pos);
            GameObject spawnerInst = Instantiate(this.zombieSpawner, spawnerPos, new Quaternion(), spawnerParent.transform);
            spawnerInst.GetComponent<ZombieSpawner>().SpawnRate = spawner.SpawnRate;
            Map.Spawners.Add(spawner.Pos.ToStr(), spawnerInst);
        }
    }

    private void PlaceBlock(EnvironmentTileType tile, int x, int y, Transform parent)
    {
        if (tile == EnvironmentTileType.Nothing)
            return;
        GameObject selectedBlock = tileMap[tile];
        Map.Environment[y, x] = tileMap[tile].GetComponent<EnvironmentTile>();
        Instantiate(selectedBlock, Map.GridPointToWorldPoint(new int[] { x, y }), new Quaternion(), parent);
    }
}