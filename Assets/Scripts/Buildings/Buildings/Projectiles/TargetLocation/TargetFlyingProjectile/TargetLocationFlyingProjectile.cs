using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetLocationFlyingProjectile : TargetFlyingProjectile
{
    protected Vector3 desiredPosition;
    protected override Vector3 targetPosition { get { return desiredPosition; } }

    public override void SetParameters(ProjectileStatsDAO stats)
    {
        if (!(stats is TargetLocationFlyingProjectileStatsDAO))
        {
            throw new ArgumentException("projectileStats must be of the type TargetLocationFlyingProjectileStatsDAO");
        }

        base.SetParameters(stats);
        this.desiredPosition = ((TargetLocationFlyingProjectileStatsDAO)stats).Location;
        this.MovementSpeed = ((TargetLocationFlyingProjectileStatsDAO)stats).MovementSpeed;
    }
}
