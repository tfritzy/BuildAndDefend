using UnityEngine;

public class FlamePlumeTower : TargetLocationTower
{
    public override TowerType Type => TowerType.FlamePlume;

    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.projectileDamage = 10;
        this.inaccuracy = .1f;
        this.projectileSpeed = 5;
        this.fireCooldown = .3f;
        this.projectileLifespan = 20f;
        this.explosionRadius = 1f;
        this.explosionDelay = 10f;
    }

    protected override void SetProjectileValues(Projectile p)
    {
        p.GetComponent<FlamePlumeProjectile>().SetParameters(
            this.projectileDamage,
            this.projectileLifespan,
            this.projectilePierce,
            this,
            this.explosionRadius,
            this.explosionDelay,
            this.lastInputPosition
        );
    }

    protected override void Setup()
    {
        base.Setup();
    }
}