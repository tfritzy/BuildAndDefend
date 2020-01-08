public class FireballTower : Tower
{
    public override BuildingType Type => BuildingType.Fireball;

    protected override string projectilePrefabName => "Fireball";

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