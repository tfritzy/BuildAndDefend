using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    private Dictionary<EnvironmentTileType, GameObject> tileMap;
    private GameObject zombieSpawner;

    void Awake()
    {
    }

    void Start()
    {
        this.zombieSpawner = Resources.Load<GameObject>($"{FilePaths.Zombies}/ZombieSpawner");
        tileMap = new Dictionary<EnvironmentTileType, GameObject>();
        foreach (EnvironmentTileType type in Enum.GetValues(typeof(EnvironmentTileType)))
        {
            tileMap.Add(type, Resources.Load<GameObject>($"{FilePaths.Terrain}/{type}"));
        }
        LoadMap(Player.PlayerData.Values.CurrentLevel);
    }

    public void LoadMap(string mapName)
    {
        MapDAO map = ReadMapFile(mapName);
        LoadGrid(map);
        LoadSpawners(map);
        LoadBuildings(map);
    }

    public void LoadBuildings(MapDAO map)
    {
        if (map.buildings == null)
        {
            return;
        }
        foreach (BuildingOnMapDAO building in map.buildings)
        {
            GameObject inst = Map.InstantiateBuilding(building);
        }
    }

    public void ResetMapData(int width, int height)
    {
        Map.Environment = new EnvironmentTile[width, height];
        Map.Buildings = new Building[width, height];
        Map.PathingGrid = new PathableType[width, height];
        Map.Spawners = new Dictionary<string, GameObject>();
        Map.BuildingDict = new Dictionary<string, Building>();
    }

    public static MapDAO ReadMapFile(String mapName)
    {
        string path = $"{FilePaths.Maps}/{mapName}.json";
        StreamReader reader = new StreamReader(path);
        string jsonMap = reader.ReadToEnd();
        reader.Close();
        return JsonConvert.DeserializeObject<MapDAO>(jsonMap);
    }

    public static void SaveMapToFile(MapDAO mapData)
    {
        string path = $"{FilePaths.Maps}/{mapData.name}.json";
        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(JsonConvert.SerializeObject(mapData));
        writer.Close();
    }

    private void LoadGrid(MapDAO map)
    {
        Map.Environment = new EnvironmentTile[map.height, map.width];
        ResetMapData(map.width, map.height);
        GameObject tileParent = new GameObject();
        tileParent.name = "Tiles";
        Instantiate(tileParent, Vector3.zero, new Quaternion(), null);
        for (int i = 0; i < map.environment.Length - 1; i++)
        {
            int x = i % map.width;
            int y = i / map.width;
            EnvironmentTileType value = map.environment[i];
            PlaceBlock(value, x, y, tileParent.transform);
        }
    }

    private void LoadSpawners(MapDAO map)
    {
        if (map.zombieSpawners == null)
        {
            return;
        }
        GameObject spawnerParent = new GameObject();
        spawnerParent.name = "Spawners";
        Instantiate(spawnerParent, Vector3.zero, new Quaternion(), null);
        foreach (ZombieSpawnerDAO spawner in map.zombieSpawners)
        {
            if (Map.Environment[spawner.Pos.x, spawner.Pos.y].CanBeBuiltUpon == false)
            {
                continue;
            }

            Vector3 spawnerPos = Map.GridPointToWorldPoint(spawner.Pos);
            GameObject spawnerInst = Instantiate(this.zombieSpawner, spawnerPos, new Quaternion(), spawnerParent.transform);
            spawnerInst.GetComponent<ZombieSpawner>().SpawnRate = spawner.SpawnRate;
            Map.Spawners.Add(spawner.Pos.ToStr(), spawnerInst);
        }
    }

    private float blockSpawnZLocation = 3f;
    private void PlaceBlock(EnvironmentTileType tile, int x, int y, Transform parent)
    {
        if (tile == EnvironmentTileType.Nothing)
            return;
        GameObject selectedBlock = tileMap[tile];
        Map.Environment[x, y] = tileMap[tile].GetComponent<EnvironmentTile>();
        Map.PathingGrid[x, y] = tileMap[tile].GetComponent<EnvironmentTile>().PathableType;
        Vector3 spawnLocation = Map.GridPointToWorldPoint(new Vector2(x, y));
        spawnLocation.z = blockSpawnZLocation;
        Instantiate(selectedBlock, spawnLocation, new Quaternion(), parent);
    }
}