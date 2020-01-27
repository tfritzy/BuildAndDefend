using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetLocationProjectile : Projectile
{
    private Vector3 targetPosition;
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }

    protected override void UpdateLoop()
    {
        base.UpdateLoop();
        if (Mathf.Abs(Vector3.SqrMagnitude(TargetPosition - this.transform.position)) < .3f)
        {
            this.OnHalt(this.gameObject);
        }
    }

    public override void SetParameters(ProjectileStatsDAO projectileStats)
    {
        if (!(projectileStats is TargetLocationProjectileStatsDAO))
        {
            throw new ArgumentException("Projectile stats must be of the type TargetLocationProjectileStatsDAO.");
        }
        base.SetParameters(projectileStats);
        this.targetPosition = ((TargetLocationProjectileStatsDAO)projectileStats).TargetPosition;
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
