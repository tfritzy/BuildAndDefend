using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetLocationSpawningProjectile : ExplosiveProjectile
{
    protected float spawnDelay;
    protected float spawnTimerStartTime;
    protected Vector3 TargetPosition;

    protected override void UpdateLoop()
    {
        base.UpdateLoop();
        if (Time.time > spawnTimerStartTime + spawnDelay)
        {
            this.gameObject.transform.position = TargetPosition;
            OnHalt(this.gameObject);
        }
    }

    protected override void Startup()
    {
        base.Startup();
        this.spawnTimerStartTime = Time.time;
        Destroy(this.gameObject.GetComponent<Collider2D>());
    }

    [Obsolete("Blocking calls because other constructor should be called.", true)]
    public override void SetParameters(int damage, float lifespan, int pierceCount, Tower owner, float explosionRadius)
    {
    }

    [Obsolete("Blocking calls because other constructor should be called.", true)]
    public override void SetParameters(int damage, float lifespan, int pierceCount, Tower owner)
    {
    }

    public virtual void SetParameters(
        int damage,
        float lifespan,
        int pierceCount,
        Tower owner,
        float explosionRadius,
        float spawnDelay,
        Vector2 targetPosition)
    {
        this.Damage = damage;
        this.Lifespan = lifespan;
        this.PierceCount = pierceCount;
        this.Owner = owner;
        this.ExplosionRadius = explosionRadius;
        this.spawnDelay = spawnDelay;
        this.TargetPosition = targetPosition;
    }
}
