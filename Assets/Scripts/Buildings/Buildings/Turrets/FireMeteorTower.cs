public class FireMeteorTower : TargetLocationFlyingProjTower
{
    public override TowerType Type => TowerType.FireMeteor;
    public override string Name => "Fire Meteor";
    public override Faction Faction => Faction.Fire;
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
    public override TowerStats GetTowerParameters(int level)
    {
        TowerStats stats = new TowerStats();
        stats.Health = 100;
        stats.Damage = 10;
        stats.Inaccuracy = .1f;
        stats.FireCooldown = .3f;
        stats.ProjectileLifespan = 10f;
        stats.ExplosionRadius = 1f;
        stats.ProjectileMovementSpeed = 80f;
        return stats;
    }
}