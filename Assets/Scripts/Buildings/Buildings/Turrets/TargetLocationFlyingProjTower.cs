public class TargetLocationFlyingProjTower : TargetLocationTower
{
    public override TowerType Type => TowerType.FireMeteor;
    protected float projectileMovementSpeed;

    protected override void SetProjectileValues(Projectile p)
    {
        p.GetComponent<TargetLocationFlyingProjectile>().SetParameters(
            this.projectileDamage,
            this.projectileLifespan,
            this.projectilePierce,
            this,
            this.explosionRadius,
            this.lastInputPosition,
            this.projectileMovementSpeed
        );
    }
}