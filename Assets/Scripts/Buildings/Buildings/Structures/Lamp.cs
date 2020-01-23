using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : Building
{

    private Darkness darkness;
    public float strength = .8f;
    public override ResourceDAO BuildCost { get => new ResourceDAO(wood: 100, gold: 10); }
    public override Vector2Int Size => new Vector2Int(0, 0);
    public override TowerType Type => TowerType.Lamp;
    public override PathableType PathableType => PathableType.UnPathable;
    public override bool IsTower => false;
    public override string Name => "Lamp";
    public override Faction Faction => Faction.All;
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

    protected override void OnDeath()
    {
    }

    public override void Setup()
    {
    }
}
