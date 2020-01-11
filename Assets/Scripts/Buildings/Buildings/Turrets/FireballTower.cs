public class FireballTower : ExplosiveProjectileTower
{
    public override TowerType Type => TowerType.Fireball;

    protected override string projectilePrefabName => "Fireball";

    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.projectileDamage = 10;
        this.inaccuracy = .1f;
        this.projectileSpeed = 5;
        this.fireCooldown = 1f;
        this.projectileLifespan = 3f;
        this.explosionRadius = 1f;
        this.projectilePierce = 0;
    }
}