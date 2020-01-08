using System;
using UnityEngine;

public class Ballista : ChargeAttack
{
    public override BuildingType Type => BuildingType.Ballista;
    public int Level;
    public override ResourceDAO BuildCost { get => new ResourceDAO(wood: 250, gold: 25, stone: 10); }
    public override Vector2Int Size => new Vector2Int(1, 1);
    protected override string projectilePrefabName => "BallistaBolt";

    public override void SetTowerParameters()
    {
        this.fireCooldown = 4;
        this.Health = 100 * Level;
        this.chargeTime = 4f;
        this.maxProjectileSpeed = 20f;
        this.maxProjectileDamage = 10;
        this.maxProjectileLifespan = 4f;
    }
}