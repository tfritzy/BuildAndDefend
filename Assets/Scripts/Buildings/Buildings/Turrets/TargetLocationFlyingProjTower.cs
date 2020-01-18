using UnityEngine;

public abstract class TargetLocationFlyingProjTower : TargetLocationTower
{
    protected float projectileMovementSpeed;

    protected override void SetProjectileValues(GameObject p, InputDAO input)
    {
        p.GetComponent<TargetLocationFlyingProjectile>().SetParameters(
            this.ProjectileDamage,
            this.ProjectileLifespan,
            this.ProjectilePierce,
            this,
            this.projectileExplosionRadius,
            ((VectorInputDAO)input).location.Value,
            this.projectileMovementSpeed
        );
    }
}