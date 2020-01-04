using System.Collections.Generic;
using UnityEngine;

public static class Map
{
    public static TileType[,] Grid;
    public static Dictionary<string, HashSet<Zombie>> PathTakers = new Dictionary<string, HashSet<Zombie>>();

    public static void FreeGridLoc(Vector3 pos)
    {
        int[] gridLoc = Map.WorldPointToGridPoint((Vector2)pos);
        Map.Grid[gridLoc[1], gridLoc[0]] = 0;
        TellAllZombiesToGetNewPath();
    }

    public static void TellAllZombiesToGetNewPath()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        foreach (GameObject z in zombies)
        {
            z.GetComponent<Zombie>().NotifyOfPathBreak();
        }
    }

    public static int[] WorldPointToGridPoint(Vector2 worldPoint)
    {
        int[] loc = new int[2] {
            (int)(((worldPoint.x - .25f) * 2) + Map.Grid.GetLength(1) / 2),
            (int)(((worldPoint.y - .25f) * 2) + Map.Grid.GetLength(0) / 2),
        };
        if (loc[0] < 0)
            loc[0] = 0;
        if (loc[0] > Map.Grid.GetLength(1) - 1)
            loc[0] = Map.Grid.GetLength(1) - 1;
        if (loc[1] < 0)
            loc[1] = 0;
        if (loc[1] > Map.Grid.GetLength(0) - 1)
            loc[1] = Map.Grid.GetLength(0) - 1;
        return loc;
    }

    public static Vector2 GridPointToWorldPoint(Vector2 gridLoc)
    {
        Vector3 loc = new Vector3(
            ((float)gridLoc[0] - Map.Grid.GetLength(1) / 2) / 2f + .5f,
            ((float)gridLoc[1] - Map.Grid.GetLength(0) / 2) / 2f + .5f);

        return loc;
    }

    public static Vector2 GridPointToWorldPoint(int[] gridLoc)
    {
        Vector2 loc = new Vector2(
            ((float)gridLoc[0] - Map.Grid.GetLength(1) / 2) / 2f + .5f,
            ((float)gridLoc[1] - Map.Grid.GetLength(0) / 2) / 2f + .5f);

        return loc;
    }


}