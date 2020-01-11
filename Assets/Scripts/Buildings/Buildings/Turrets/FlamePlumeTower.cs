using UnityEngine;

public class FlamePlumeTower : TargetLocationTower
{
    public override TowerType Type => TowerType.FlamePlume;

    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.ProjectileDamage = 10;
        this.inaccuracy = .1f;
        this.ProjectileMovementSpeed = 5;
        this.FireCooldown = .3f;
        this.ProjectileLifespan = 20f;
        this.projectileExplosionRadius = 1f;
        this.explosionDelay = 1f;
    }

    protected override void SetProjectileValues(GameObject p)
    {
        p.GetComponent<FlamePlumeProjectile>().SetParameters(
            this.ProjectileDamage,
            this.ProjectileLifespan,
            this.ProjectilePierce,
            this,
            this.projectileExplosionRadius,
            this.explosionDelay,
            this.lastInputPosition
        );
    }

    protected override void Setup()
    {
        base.Setup();
    }
}