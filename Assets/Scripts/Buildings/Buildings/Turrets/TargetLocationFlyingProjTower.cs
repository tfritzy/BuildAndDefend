using UnityEngine;

public abstract class TargetLocationFlyingProjTower : TargetLocationTower
{
    protected override void SetProjectileValues(GameObject p, InputDAO input)
    {
        TargetLocationFlyingProjectileStatsDAO stats = new TargetLocationFlyingProjectileStatsDAO
        {
            Damage = Stats.Damage,
            Lifespan = Stats.ProjectileLifespan,
            PierceCount = Stats.Pierce,
            Owner = this,
            ExplosionRadius = Stats.ExplosionRadius,
            Location = ((VectorInputDAO)input).location.Value,
            MovementSpeed = Stats.ProjectileMovementSpeed,
        };

        p.GetComponent<Projectile>().SetParameters(stats);
    }
}