﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWall : Building
{
    public override ResourceDAO BuildCost { get => new ResourceDAO(wood: 75, stone: 20); }
    public override Vector2Int Size => new Vector2Int(0, 0);
    public override BuildingType Type => BuildingType.Wall;
    public override PathableType PathableType => PathableType.UnPathable;

    public override string Name => "Wood Wall";
    public override Faction Faction => Faction.All;

    public override ResourceDAO PowerUpCost
    {
        get
        {
            return new ResourceDAO(
                gold: 50 * this.Level,
                wood: 30 * this.Level,
                stone: 5 * this.Level);
        }
    }
}
