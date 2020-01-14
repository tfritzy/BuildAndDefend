using UnityEngine;

public abstract class TargetEntityFlyingProjectile : TargetLocationFlyingProjectile, ITargetEntityFlyingProjectile
{
    public GameObject Target { get; set; }

    protected override void UpdateLoop()
    {
        base.UpdateLoop();
        this.TargetPosition = this.Target.transform.position;
    }

    public virtual void SetParameters(
        int damage,
        float lifespan,
        int pierceCount,
        Tower owner,
        float explosionRadius,
        GameObject target,
        float movementSpeed)
    {
        this.Damage = damage;
        this.Lifespan = lifespan;
        this.Owner = owner;
        this.Target = target;
        this.ExplosionRadius = explosionRadius;
        this.PierceCount = pierceCount;
        this.MovementSpeed = movementSpeed;
    }
}