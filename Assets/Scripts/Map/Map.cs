using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public static class Map
{
    public static EnvironmentTile[,] Environment;
    public static Building[,] Buildings;
    public static PathableType[,] PathingGrid;
    public static Dictionary<string, HashSet<Zombie>> PathTakers = new Dictionary<string, HashSet<Zombie>>();
    public static Dictionary<string, ResourceDAO> Harvesters = new Dictionary<string, ResourceDAO>();


    /// <summary>
    /// A dictionary containing all the player's buildings.
    /// </summary>
    /// <typeparam name="string">The position of the tower in form 'x,y'</typeparam>
    /// <typeparam name="GameObject">The tower object.</typeparam>
    /// <returns></returns>
    public static Dictionary<string, GameObject> BuildingsDict = new Dictionary<string, GameObject>();

    public static Dictionary<string, GameObject> Spawners = new Dictionary<string, GameObject>();

    public static void ReprocessPathingLoc(Vector2Int location)
    {
        PathableType pathingType =
            (Buildings[location.x, location.y]?.PathableType == PathableType.UnPathable ||
            Environment[location.x, location.y]?.PathableType == PathableType.UnPathable) ?
            PathableType.UnPathable : PathableType.Pathable;
        PathingGrid[location.x, location.y] = pathingType;
    }

    public static void InstantiateBuilding(BuildingOnMapDAO building)
    {
        Vector2Int position = new Vector2Int(building.xPos, building.yPos);
        GameObject buildingInst = GameObject.Instantiate(
            GameObjectCache.Buildings[building.Type],
            GameObjectCache.Buildings[building.Type]
                .GetComponent<Building>()
                .GetWorldPointFromGridPoint(position),
            new Quaternion(),
            null);
        buildingInst.GetComponent<Building>().BuildingId = building.BuildingId;
    }

    /// <summary>
    /// Returns true if the given building can be placed on the given starting position.
    /// </summary>
    public static bool CanPlaceBuildingHere(Building building)
    {
        for (int x = building.Position.x; x <= building.Position.x + building.Size.x; x++)
        {
            for (int y = building.Position.y; y <= building.Position.y + building.Size.y; y++)
            {
                if (Buildings[x, y] != null)
                {
                    return false;
                }
                if (Environment[x, y] != null && !Environment[x, y].CanBeBuiltUpon)
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Adds the given building to the Map.Buildings object.
    /// building.Size = 3, 2
    /// position = s = 1, 2
    /// x b b b x
    /// x s b b x
    /// x x x x x
    /// x x x x x
    /// </summary>
    /// <param name="building"></param>
    /// <param name="position">The starting position of the building.</param>
    public static void AddBuildingToMap(Building building, Vector2Int position)
    {
        for (int x = building.Position.x; x <= building.Position.x + building.Size.x; x++)
        {
            for (int y = building.Position.y; y <= building.Position.y + building.Size.y; y++)
            {
                if (Buildings[x, y] != null)
                {
                    throw new InvalidOperationException($"Cannot place building on occupied tile! Tile currently occupied by: {Buildings[x, y].Type}");
                }
                Buildings[x, y] = building;
                ReprocessPathingLoc(new Vector2Int(x, y));
                NotifyPathTakersOfBuildingChange(x, y);
                BuildingsDict.Add($"{x},{y}", building.gameObject);
            }
        }
    }

    public static void RemoveBuildingFromMap(Building building)
    {
        for (int x = building.Position.x; x <= building.Position.x + building.Size.x; x++)
        {
            for (int y = building.Position.y; y <= building.Position.y + building.Size.y; y++)
            {
                if (Buildings[x, y] == null || Buildings[x, y].Type != building.Type)
                {
                    throw new InvalidOperationException($"Call included removing incorrect building type." +
                        " Tile ({x}, {y}) type={Buildings[x, y].Type}. RemovalType={building.Type}");
                }
                Buildings[x, y] = null;
                ReprocessPathingLoc(new Vector2Int(x, y));
                BuildingsDict.Remove($"{x},{y}");
            }
        }

        TellAllZombiesToGetNewPath();
    }

    private static void NotifyPathTakersOfBuildingChange(int x, int y)
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

    public static void TellAllZombiesToGetNewPath()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        foreach (GameObject z in zombies)
        {
            z.GetComponent<Zombie>().NotifyOfPathBreak();
        }
    }

    public static Vector2Int WorldPointToGridPoint(Vector2 worldPoint)
    {
        Vector2Int loc = new Vector2Int(
            (int)(((worldPoint.x - .25f) * 2) + Map.Environment.GetLength(0) / 2),
            (int)(((worldPoint.y - .25f) * 2) + Map.Environment.GetLength(1) / 2)
        );
        if (loc.x < 0)
            loc.x = 0;
        if (loc.x > Map.Environment.GetLength(0) - 1)
            loc.x = Map.Environment.GetLength(0) - 1;
        if (loc.y < 0)
            loc.y = 0;
        if (loc.y > Map.Environment.GetLength(1) - 1)
            loc.y = Map.Environment.GetLength(1) - 1;
        return loc;
    }

    public static Vector2 GridPointToWorldPoint(Vector2 gridLoc)
    {
        Vector3 loc = new Vector3(
            ((float)gridLoc[0] - Map.Environment.GetLength(0) / 2) / 2f + .5f,
            ((float)gridLoc[1] - Map.Environment.GetLength(1) / 2) / 2f + .5f);

        return loc;
    }

    public static void SaveMapToFile()
    {
        MapDAO mapSave = new MapDAO();
        mapSave.width = Map.Environment.GetLength(0);
        mapSave.height = Map.Environment.GetLength(1);
        mapSave.name = Player.Data.CurrentLevel;
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

        string path = $"{FilePaths.Maps}/{Player.Data.CurrentLevel}.json";
        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(JsonConvert.SerializeObject(mapSave));
        writer.Close();
    }
}