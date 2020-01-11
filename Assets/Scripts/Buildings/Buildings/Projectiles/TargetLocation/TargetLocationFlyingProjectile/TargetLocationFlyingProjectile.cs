using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetLocationFlyingProjectile : Projectile, ITargetLocationFlyingProjectile
{
    private Vector3 targetPosition;
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }

    protected override void UpdateLoop()
    {
        base.UpdateLoop();
        if (Mathf.Abs(Vector3.SqrMagnitude(this.TargetPosition - this.transform.position)) < .3f ||
            this.transform.position.z > 0)
        {
            this.OnHalt(this.gameObject);
        }
    }

    public virtual void SetParameters(
        int damage,
        float lifespan,
        int pierceCount,
        Tower owner,
        float explosionRadius,
        Vector3 targetPosition,
        float movementSpeed)
    {
        this.Damage = damage;
        this.Lifespan = lifespan;
        this.Owner = owner;
        this.TargetPosition = targetPosition;
        this.ExplosionRadius = explosionRadius;
        this.PierceCount = pierceCount;
        this.MovementSpeed = movementSpeed;
    }

    protected override void OnTargetCollisionEnter(GameObject target)
    {
        // Do Nothing.
    }

    protected override void OnTargetCollisionExit(GameObject target)
    {
        // Do Nothing.
    }
}
