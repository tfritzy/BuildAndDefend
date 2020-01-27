using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimedExplosionProjectile : Projectile
{
    protected float explosionDelay;
    protected float spawnTimerStartTime;

    protected override void UpdateLoop()
    {
        base.UpdateLoop();
        if (Time.time > spawnTimerStartTime + explosionDelay)
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

    public override void SetParameters(ProjectileStatsDAO projectileStats)
    {
        if (!(projectileStats is TargetLocationSpawningProjectileStatsDAO))
        {
            throw new ArgumentException("ProjectileStats must be of the type TargetLocationSpawningProjectileStatsDAO");
        }
        base.SetParameters(projectileStats);
        this.explosionDelay = ((TargetLocationSpawningProjectileStatsDAO)projectileStats).ExplosionDelay;
    }
}
