using UnityEngine;

public class FireBoltTower : Tower
{
    public override BuildingType Type => BuildingType.FireBolt;
    public override string Name => "Fire Bolt";
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
        stats.ProjectileMovementSpeed = 5;
        stats.FireCooldown = 1f;
        stats.ProjectileLifespan = 3f;
        return stats;
    }
}