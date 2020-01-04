using System.Collections.Generic;
using UnityEngine;

public static class Map
{
    public static TileType[,] Grid;
    public static Dictionary<string, HashSet<Zombie>> PathTakers = new Dictionary<string, HashSet<Zombie>>();

    /// <summary>
    /// A dictionary containing all the player's towers.
    /// </summary>
    /// <typeparam name="string">The position of the tower in form 'x,y'</typeparam>
    /// <typeparam name="GameObject">The tower object.</typeparam>
    /// <returns></returns>
    public static Dictionary<string, GameObject> Towers = new Dictionary<string, GameObject>();

    public static void FreeGridLoc(Vector3 pos)
    {
        Vector2Int gridLoc = Map.WorldPointToGridPoint((Vector2)pos);
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

    public static Vector2Int WorldPointToGridPoint(Vector2 worldPoint)
    {
        Vector2Int loc = new Vector2Int(
            (int)(((worldPoint.x - .25f) * 2) + Map.Grid.GetLength(1) / 2),
            (int)(((worldPoint.y - .25f) * 2) + Map.Grid.GetLength(0) / 2)
        );
        if (loc.x < 0)
            loc.x = 0;
        if (loc.x > Map.Grid.GetLength(1) - 1)
            loc.x = Map.Grid.GetLength(1) - 1;
        if (loc.y < 0)
            loc.y = 0;
        if (loc.y > Map.Grid.GetLength(0) - 1)
            loc.y = Map.Grid.GetLength(0) - 1;
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