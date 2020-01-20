using System;
using UnityEngine;

public class Ballista : ChargeAttack
{
    public override TowerType Type => TowerType.Ballista;
    public override ResourceDAO BuildCost { get => new ResourceDAO(wood: 250, gold: 25, stone: 10); }
    public override Vector2Int Size => new Vector2Int(1, 1);
    public override string Name => "Ballista";
    public override Faction Faction => Faction.Fire;
    public override ResourceDAO PowerUpCost
    {
        get
        {
            return new ResourceDAO(
                gold: 100 * this.Level,
                wood: 40 * this.Level,
                stone: 10 * this.Level);
        }
    }

    public override void SetTowerParameters()
    {
        this.FireCooldown = 4;
        this.Health = 100 * Level;
        this.chargeTime = 4f;
        this.maxProjectileSpeed = 20f;
        this.maxProjectileDamage = 10;
        this.maxProjectileLifespan = 4f;
    }
}