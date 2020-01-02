using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWall : Building {

    public override string StructPath { get => "Gameobjects/Buildings/WallSegment"; }
    public override int WoodCost { get => 100; }
}
