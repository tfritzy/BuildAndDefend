using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWall : Building {

    public WoodWall()
    {
        this.WoodCost = 100;
        this.StructPath = "Gameobjects/Buildings/WallSegment";
    }

    protected override void OnDeath()
    {

    }

    protected override void Setup()
    {

    }
}
