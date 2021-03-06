using UnityEngine;

public class FireMeteorTower : TargetLocationFlyingProjTower
{
    public override BuildingType Type => BuildingType.FireMeteor;
    public override string Name => "Fire Meteor";
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
        stats.FireCooldown = .3f;
        stats.ProjectileLifespan = 10f;
        stats.ExplosionRadius = 1f;
        stats.ProjectileMovementSpeed = 80f;
        return stats;
    }
}