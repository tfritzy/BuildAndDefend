using System;
using System.Collections.Generic;
using UnityEngine;

public static class Map
{
    public static EnvironmentTile[,] Environment;
    public static Building[,] Buildings;
    public static PathableType[,] PathingGrid;
    public static Dictionary<string, HashSet<Zombie>> PathTakers = new Dictionary<string, HashSet<Zombie>>();

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
            (Buildings[location.y, location.x].PathableType == PathableType.Pathable &&
            Environment[location.y, location.x].PathableType == PathableType.Pathable) ?
            PathableType.Pathable : PathableType.UnPathable;
        PathingGrid[location.y, location.x] = pathingType;
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
                if (Buildings[y, x] != null && Buildings[y, x].Type != BuildingType.Nothing)
                {
                    return false;
                }
                if (Environment[y, x] != null && !Environment[y, x].CanBeBuiltUpon)
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
                if (Buildings[y, x] != null && Buildings[y, x].Type != BuildingType.Nothing)
                {
                    throw new InvalidOperationException($"Cannot place building on occupied tile! Tile currently occupied by: {Buildings[y, x].Type}");
                }
                Buildings[y, x] = building;
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
                if (Buildings[y, x] == null || Buildings[y, x].Type != building.Type)
                {
                    throw new InvalidOperationException($"Call included removing incorrect building type." +
                        " Tile ({x}, {y}) type={Buildings[y, x].Type}. RemovalType={building.Type}");
                }
                Buildings[y, x] = null;
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
            (int)(((worldPoint.x - .25f) * 2) + Map.Environment.GetLength(1) / 2),
            (int)(((worldPoint.y - .25f) * 2) + Map.Environment.GetLength(0) / 2)
        );
        if (loc.x < 0)
            loc.x = 0;
        if (loc.x > Map.Environment.GetLength(1) - 1)
            loc.x = Map.Environment.GetLength(1) - 1;
        if (loc.y < 0)
            loc.y = 0;
        if (loc.y > Map.Environment.GetLength(0) - 1)
            loc.y = Map.Environment.GetLength(0) - 1;
        return loc;
    }

    public static Vector2 GridPointToWorldPoint(Vector2 gridLoc)
    {
        Vector3 loc = new Vector3(
            ((float)gridLoc[0] - Map.Environment.GetLength(1) / 2) / 2f + .5f,
            ((float)gridLoc[1] - Map.Environment.GetLength(0) / 2) / 2f + .5f);

        return loc;
    }

    public static Vector2 GridPointToWorldPoint(int[] gridLoc)
    {
        Vector2 loc = new Vector2(
            ((float)gridLoc[0] - Map.Environment.GetLength(1) / 2) / 2f + .5f,
            ((float)gridLoc[1] - Map.Environment.GetLength(0) / 2) / 2f + .5f);

        return loc;
    }


}