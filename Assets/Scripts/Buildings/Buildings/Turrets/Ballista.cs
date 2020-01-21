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
                gold: 100 * this.Tier,
                wood: 40 * this.Tier,
                stone: 10 * this.Tier);
        }
    }

    public override TowerStats GetTowerParameters(int level, int tier)
    {
        TowerStats stats = new TowerStats();
        stats.FireCooldown = 4;
        stats.Health = 100 * Level;
        this.chargeTime = 4f;
        this.maxProjectileSpeed = 20f;
        this.maxProjectileDamage = 10;
        this.maxProjectileLifespan = 4f;
        return stats;
    }
}