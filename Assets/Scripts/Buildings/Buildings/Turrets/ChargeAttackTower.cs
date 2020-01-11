using System;
using UnityEngine;

public abstract class ChargeAttack : Tower
{
    private Vector2 lastTouchLocation;
    protected float chargeTime;
    protected float maxProjectileSpeed;
    public float startChargeTime;
    public bool isCharging;

    public override float ProjectileMovementSpeed
    {
        get
        {
            return Mathf.Min(
                getChargePercentage() * maxProjectileSpeed,
                maxProjectileSpeed
            );
        }
    }

    protected int maxProjectileDamage;
    public override int ProjectileDamage
    {
        get
        {
            return Mathf.Min(
                (int)(getChargePercentage() * maxProjectileSpeed),
                maxProjectileDamage
            );
        }
    }

    protected float maxProjectileLifespan;
    public override float ProjectileLifespan
    {
        get
        {
            return Mathf.Min(
                (getChargePercentage() * maxProjectileLifespan),
                maxProjectileDamage
            );
        }
    }

    private float getChargePercentage()
    {
        return ((Time.time - startChargeTime) / chargeTime);
    }

    protected override void Fire()
    {
        Vector2? inputLocation = GetInputLocation();

        if (inputLocation.HasValue)
        {
            lastTouchLocation = inputLocation.Value;
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
                CreateProjectile(fireDirection);
                isCharging = false;
            }
        }
    }
}