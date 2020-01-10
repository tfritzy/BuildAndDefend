public abstract class ExplosiveProjectileTower : Tower
{
    public float explosionRadius;

    protected override void SetProjectileValues(Projectile p)
    {
        p.GetComponent<ExplosiveProjectile>().SetParameters(
            this.projectileDamage,
            this.projectileLifespan,
            this.projectilePierce,
            this,
            this.explosionRadius
        );
    }
}