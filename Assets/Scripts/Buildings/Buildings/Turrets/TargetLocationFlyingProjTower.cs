using UnityEngine;

public abstract class TargetLocationFlyingProjTower : TargetLocationTower
{
    protected float projectileMovementSpeed;

    protected override void SetProjectileValues(GameObject p, InputDAO input)
    {
        p.GetComponent<TargetLocationFlyingProjectile>().SetParameters(
            Stats.Damage,
            Stats.ProjectileLifespan,
            Stats.Pierce,
            this,
            Stats.ExplosionRadius,
            ((VectorInputDAO)input).location.Value,
            this.projectileMovementSpeed
        );
    }
}