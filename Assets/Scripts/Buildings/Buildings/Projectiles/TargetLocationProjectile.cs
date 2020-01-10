using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetLocationProjectile : ExplosiveProjectile
{
    public Vector3 TargetPosition;

    protected override void UpdateLoop()
    {
        base.UpdateLoop();
        if (Mathf.Abs(Vector3.SqrMagnitude(TargetPosition - this.transform.position)) < .2f)
        {
            this.OnHalt(this.gameObject);
        }
    }

    [Obsolete("The SetParameters needs to be called with target position and explosion radius", true)]
    public override void SetParameters(int damage, float lifespan, int pierceCount, Tower owner)
    {
    }

    [Obsolete("The SetParameters needs to be called with target position and explosion radius", true)]
    public virtual void SetParameters(
        int damage,
        float lifespan,
        int pierceCount,
        Tower owner,
        float explosionRadius)
    {
    }

    public virtual void SetParameters(
        int damage,
        float lifespan,
        int pierceCount,
        Tower owner,
        float explosionRadius,
        Vector3 targetPosition)
    {
        this.Damage = damage;
        this.Lifespan = lifespan;
        this.Owner = owner;
        this.TargetPosition = targetPosition;
        this.ExplosionRadius = explosionRadius;
        this.PierceCount = pierceCount;
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
