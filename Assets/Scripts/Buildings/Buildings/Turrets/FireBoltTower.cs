public class FireBoltTower : Tower
{
    public override TowerType Type => TowerType.FireBolt;
    public override string Name => "Fire Bolt";
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
        stats.ProjectileMovementSpeed = 5;
        stats.FireCooldown = 1f;
        stats.ProjectileLifespan = 3f;
        return stats;
    }
}