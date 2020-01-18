using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedExplosionProjectile : Projectile, ITimedExplosionProjectile
{
    protected float explosionDelay;
    public float ExplosionDelay { get => explosionDelay; set => explosionDelay = value; }
    protected float spawnTimerStartTime;


    protected override void UpdateLoop()
    {
        base.UpdateLoop();
        if (Time.time > spawnTimerStartTime + ExplosionDelay)
        {
            OnHalt(this.gameObject);
        }
    }

    protected override void Startup()
    {
        base.Startup();
        this.spawnTimerStartTime = Time.time;
        Destroy(this.gameObject.GetComponent<Collider2D>());
    }

    public virtual void SetParameters(
        int damage,
        float lifespan,
        int pierceCount,
        Tower owner,
        float explosionRadius,
        float spawnDelay)
    {
        this.Damage = damage;
        this.Lifespan = lifespan;
        this.PierceCount = pierceCount;
        this.Owner = owner;
        this.ExplosionRadius = explosionRadius;
        this.explosionDelay = spawnDelay;
    }
}
