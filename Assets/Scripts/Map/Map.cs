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
    /// <typeparam name="string"> The building id. </typeparam>
    /// <typeparam name="GameObject"> The tower object. </typeparam>
    /// <returns></returns>
    public static Dictionary<string, Building> BuildingDict = new Dictionary<string, Building>();

    public static Dictionary<string, GameObject> Spawners = new Dictionary<string, GameObject>();

    public static void ReprocessPathingLoc(Vector2Int location)
    {
        PathableType pathingType =
            (Buildings[location.x, location.y]?.PathableType == PathableType.UnPathable ||
            Environment[location.x, location.y]?.PathableType == PathableType.UnPathable) ?
            PathableType.UnPathable : PathableType.Pathable;
        PathingGrid[location.x, location.y] = pathingType;
    }

    public static GameObject InstantiateBuilding(BuildingOnMapDAO building)
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
        buildingInst.GetComponent<Building>().Position = position;
        Map.AddBuildingToMap(buildingInst.GetComponent<Building>(), new Vector2Int(building.xPos, building.yPos));

        return buildingInst;
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
    /// </summary>
    /// <param name="building"></param>
    /// <param name="position">The starting position of the building.</param>
    public static void AddBuildingToMap(Building building, Vector2Int position)
    {
        for (int x = position.x; x <= position.x + building.Size.x; x++)
        {
            for (int y = position.y; y <= position.y + building.Size.y; y++)
            {
                if (Buildings[x, y] != null)
                {
                    throw new InvalidOperationException($"Cannot place building on occupied tile! Tile currently occupied by: {Buildings[x, y].Type}");
                }
                Buildings[x, y] = building;
                ReprocessPathingLoc(new Vector2Int(x, y));
                NotifyPathTakersOfBuildingChange(x, y);
            }
        }
        BuildingDict.Add(building.BuildingId, building);
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
            }
        }

        BuildingDict.Remove(building.BuildingId);
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

    public static void SaveState()
    {
        SaveMapToFile();
        UpdateTotalResourceProduction();
    }

    public static List<BuildingOnMapDAO> GetCurrentBuildingsOnMap()
    {
        List<BuildingOnMapDAO> currentBuildings = new List<BuildingOnMapDAO>();
        foreach (string towerId in Map.BuildingDict.Keys)
        {
            currentBuildings.Add(Map.BuildingDict[towerId].ToBuildingOnMapDAO());
        }
        return currentBuildings;
    }

    public static void SaveMapToFile()
    {
        MapDAO mapFile = MapLoader.ReadMapFile(Player.Data.CurrentLevel);
        mapFile.resourceProduction = Map.Harvesters;
        mapFile.buildings = GetCurrentBuildingsOnMap();
        MapLoader.SaveMapToFile(mapFile);
    }

    public static void UpdateTotalResourceProduction()
    {
        ResourceDAO totalResourceProductionOnThisMap = new ResourceDAO();
        foreach (ResourceDAO production in Harvesters.Values)
        {
            totalResourceProductionOnThisMap.Add(production);
        }

        Player.Data.ResourceHarvestByMapPerHour[Player.Data.CurrentLevel] = totalResourceProductionOnThisMap;
    }
}