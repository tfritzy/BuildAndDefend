public class FireBoltTower : Tower
{
    public override TowerType Type => TowerType.FireBolt;

    protected override string projectilePrefabName => "FireBolt";

    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.projectileDamage = 10;
        this.inaccuracy = .1f;
        this.projectileSpeed = 5;
        this.fireCooldown = 1f;
        this.projectileLifespan = 3f;
    }
}