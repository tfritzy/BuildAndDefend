using System.Collections.Generic;
using UnityEngine;

public class LumberCamp : Harvester
{
    public override ResourceDAO BuildCost => new ResourceDAO(wood: 10, iron: 1, gold: 15);
    public override Vector2Int Size => new Vector2Int(0, 0);
    public override string Name => "Lumber Camp";
    public override BuildingType Type => BuildingType.LumberCamp;
    public override PathableType PathableType => PathableType.UnPathable;
    public override Faction Faction => Faction.All;
    public override ResourceDAO PowerUpCost => new ResourceDAO(wood: 2 * this.Tier, gold: 3 * this.Tier);
    protected override int resourceGatherRange => 1;

    public override Dictionary<EnvironmentTileType, int> GetResourceProductionByTileType()
    {
        return new Dictionary<EnvironmentTileType, int>
        {
            {
                EnvironmentTileType.Brush,
                3
            },
        };
    }
}