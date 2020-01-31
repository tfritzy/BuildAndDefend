using System.Collections.Generic;
using UnityEngine;

public abstract class Harvester : Building
{
    public ResourceDAO ResourceProductionPerHour { get; set; }
    protected abstract int resourceGatherRange { get; }
    protected Dictionary<EnvironmentTileType, int> gatherRateByTileType { get; set; }

    public override void Setup()
    {
        base.Setup();
        this.gatherRateByTileType = GetResourceProductionByTileType();
        this.ResourceProductionPerHour = CalculateResourceProduction(this.Position);
        RegisterResourceProduction();
    }

    public void RegisterResourceProduction()
    {
        if (Player.Data.ResourceHarvestByMapPerHour.ContainsKey(Player.Data.CurrentLevel) == false)
        {
            Player.Data.ResourceHarvestByMapPerHour.Add(Player.Data.CurrentLevel, new ResourceDAO());
        }

        if (!Map.Harvesters.ContainsKey(BuildingId))
        {
            Map.Harvesters.Add(BuildingId, ResourceProductionPerHour);
        }
        else
        {
            Map.Harvesters[BuildingId] = ResourceProductionPerHour;
        }
    }

    public void DeRegisterResourceProduction()
    {
        if (Player.Data.ResourceHarvestByMapPerHour.ContainsKey(Player.Data.CurrentLevel) == false)
        {
            throw new System.Exception("It shouldn't be possible that this harvester" +
                "is getting de-registered from a map that isn't in playerdata");
        }

        Map.Harvesters.Remove(BuildingId);
    }

    protected EnvironmentTileType[,] GetSurroundingEnvironment(Vector2Int size, Vector2Int offset)
    {
        EnvironmentTileType[,] tiles = new EnvironmentTileType[size.x + 1, size.y + 1];

        int x = Mathf.Max(this.Position.x - size.x / 2 + offset.x, 0);
        int xMax = Mathf.Min(this.Position.x + size.x / 2 + offset.x, Map.Environment.GetLength(0) - 1);
        int yMax = Mathf.Min(this.Position.y + size.y / 2 + offset.y, Map.Environment.GetLength(1) - 1);
        for (; x <= xMax; x++)
        {
            for (int y = Mathf.Max(this.Position.y - size.y / 2 + offset.y, 0); y <= yMax; y++)
            {
                Debug.Log($"{x - this.Position.x + size.x / 2},{y - this.Position.y + size.y / 2}");
                tiles[x - this.Position.x + size.x / 2, y - this.Position.y + size.y / 2] = Map.Environment[x, y].Type;
            }
        }

        return tiles;
    }

    public abstract Dictionary<EnvironmentTileType, int> GetResourceProductionByTileType();

    protected int brushWoodGatherRate = 3;
    protected virtual ResourceDAO CalculateResourceProduction(Vector2Int position)
    {
        ResourceDAO resourceProductionPerHour = new ResourceDAO();
        foreach (EnvironmentTileType type in GetSurroundingEnvironment(new Vector2Int(this.Size.x + 2, this.Size.y + 2), Vector2Int.zero))
        {
            if (type == EnvironmentTileType.Brush)
            {
                resourceProductionPerHour.Add(new ResourceDAO(wood: brushWoodGatherRate));
            }
        }

        return resourceProductionPerHour;
    }
}