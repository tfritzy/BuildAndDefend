using System;
using UnityEngine;

public abstract class ChargeAttack : Tower
{
    private Vector2 lastTouchLocation;
    protected float chargeTime;
    protected float maxProjectileSpeed;
    public float startChargeTime;
    public bool isCharging;
    protected int maxProjectileDamage;
    protected float maxProjectileLifespan;

    private float getChargePercentage()
    {
        return ((Time.time - startChargeTime) / chargeTime);
    }

    protected override void Fire(InputDAO input)
    {
        if (input.HasValue())
        {
            lastTouchLocation = ((VectorInputDAO)input).location.Value;
            if (!isCharging)
            {
                startChargeTime = Time.time;
                isCharging = true;
            }
        }
        else
        {
            if (isCharging)
            {
                Vector2 fireDirection = CalculateProjectileTargetLocation(lastTouchLocation);
                CreateProjectile(input);
                isCharging = false;
            }
        }
    }
}