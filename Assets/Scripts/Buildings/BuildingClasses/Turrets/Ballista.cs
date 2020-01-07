using System;
using UnityEngine;

public class Ballista : Tower
{
    public float startChargeTime;
    public bool isCharging;
    public override BuildingType Type => BuildingType.Ballista;
    public int Level;
    public override ResourceDAO BuildCost { get => new ResourceDAO(wood: 250, gold: 25, stone: 10); }

    private Vector2 lastTouchLocation;
    protected float chargeTime;
    protected float maxProjectileSpeed;
    public override Vector2Int Size => new Vector2Int(1, 1);
    public override PathableType PathableType => PathableType.UnPathable;
    protected override string projectilePrefabName => "BallistaBolt";

    public override float projectileSpeed
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
    public override int projectileDamage
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
    public override float projectileLifespan
    {
        get
        {
            return Mathf.Min(
                (getChargePercentage() * maxProjectileLifespan),
                maxProjectileDamage
            );
        }
    }

    protected override void SetParameters()
    {
        this.fireCooldown = 4;
        this.Health = 100 * Level;
        this.chargeTime = 4f;
        this.maxProjectileSpeed = 20f;
        this.maxProjectileDamage = 10;
        this.maxProjectileLifespan = 1.5f;
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