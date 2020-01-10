using UnityEngine;

public abstract class TargetLocationTower : ExplosiveProjectileTower
{
    protected Vector2 lastInputLocation;

    protected override void Fire()
    {
        Vector2? inputLocation = GetInputLocation();
        if (inputLocation.HasValue)
        {
            lastInputLocation = inputLocation.Value;
            CreateProjectile(Vector3.zero);
            lastFireTime = Time.time;
        }
    }

    protected override void SetProjectileValues(Projectile p)
    {
        p.GetComponent<TargetLocationProjectile>().SetParameters(
            this.projectileDamage,
            this.projectileLifespan,
            this.projectilePierce,
            this,
            this.explosionRadius,
            lastInputLocation);
    }
}