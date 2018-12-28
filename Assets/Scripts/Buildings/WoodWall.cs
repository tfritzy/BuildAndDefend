using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWall : Building {

    public WoodWall()
    {
        this.woodCost = 100;
        this.structPath = "Gameobjects/Buildings/WallSegment";
    }

    protected override void OnDeath()
    {

    }

    protected override void Setup()
    {

    }
}
