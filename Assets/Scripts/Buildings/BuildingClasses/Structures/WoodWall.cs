using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWall : Building
{
    public override int WoodCost { get => 100; }
    public override Vector2Int Size => new Vector2Int(0, 0);
    public override BuildingType Type => BuildingType.Wall;
    public override PathableType PathableType => PathableType.UnPathable;
}
