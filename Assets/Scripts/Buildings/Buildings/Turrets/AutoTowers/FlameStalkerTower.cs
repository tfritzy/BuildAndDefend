public class FlameStalkerTower : AutoAttackTower
{
    public override TowerType Type => TowerType.FlameStalker;

    public override void SetTowerParameters()
    {
        this.Health = 100;
        this.ProjectileDamage = 30;
        this.inaccuracy = .9f;
        this.ProjectileMovementSpeed = 3f;
        this.FireCooldown = 3f;
        this.ProjectileLifespan = 20f;
        this.Range = 5f;
        this.projectileExplosionRadius = .3f;
    }

    protected override void SetProjectileValues(UnityEngine.GameObject p)
    {
        p.GetComponent<ITargetEntityFlyingProjectile>().SetParameters(
            this.ProjectileDamage,
            this.ProjectileLifespan,
            this.ProjectilePierce,
            this,
            this.projectileExplosionRadius,
            this.Target,
            this.ProjectileMovementSpeed
        );
    }
}