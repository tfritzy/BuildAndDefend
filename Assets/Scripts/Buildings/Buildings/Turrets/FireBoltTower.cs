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
                gold: 100 * this.Level,
                wood: 40 * this.Level,
                stone: 10 * this.Level);
        }
    }
    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.ProjectileDamage = 10;
        this.inaccuracy = .1f;
        this.ProjectileMovementSpeed = 5;
        this.FireCooldown = 1f;
        this.ProjectileLifespan = 3f;
    }
}