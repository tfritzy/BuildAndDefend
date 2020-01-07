public class FireBoltTower : Tower
{
    public override BuildingType Type => BuildingType.FireBolt;

    protected override string projectilePrefabName => "FireBolt";

    protected override void SetParameters()
    {
        this.Health = 100;
        this.projectileDamage = 10;
        this.inaccuracy = .1f;
        this.projectileSpeed = 5;
        this.fireCooldown = 1f;
        this.projectileLifespan = 3f;
    }
}