using UnityEngine;

public class RapidFireTower : StretchProjectileTower
{
    public override BuildingType Type => BuildingType.RapidFire;
    public override string Name => "Rapid Fire";
    public override Faction Faction => Faction.Fire;
    public override Vector2Int Size => new Vector2Int(1, 1);
    public override ResourceDAO PowerUpCost
    {
        get
        {
            return new ResourceDAO(
                gold: 100 * this.Tier,
                wood: 40 * this.Tier,
                stone: 10 * this.Tier);
        }
    }
    public override TowerStats GetTowerParameters(int level, int tier)
    {
        TowerStats stats = new TowerStats();
        stats.Health = 100 + 10 * level + 5 * tier;
        stats.Damage = 10 + level * 5 + tier * 3;
        stats.Inaccuracy = .1f;
        stats.ProjectileMovementSpeed = 0;
        stats.FireCooldown = .01f;
        stats.ProjectileLifespan = .2f;
        stats.Pierce = int.MaxValue;
        return stats;
    }
}