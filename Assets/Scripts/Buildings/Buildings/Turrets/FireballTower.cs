public class FireballTower : Tower
{
    public override BuildingType Type => BuildingType.Fireball;
    public override string Name => "Fireball";
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
    public override TowerStats GetTowerParameters(int level, int tier)
    {
        TowerStats stats = new TowerStats();
        stats.Health = 100 + 10 * level + 5 * tier;
        stats.Damage = 10 + level * 5 + tier * 3;
        stats.Inaccuracy = .1f;
        stats.ProjectileMovementSpeed = 5;
        stats.FireCooldown = 1f;
        stats.ProjectileLifespan = 3f;
        stats.Pierce = 0;
        stats.ExplosionRadius = 1f;
        return stats;
    }
}