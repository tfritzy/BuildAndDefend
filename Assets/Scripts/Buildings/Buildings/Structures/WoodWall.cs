using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWall : Building
{
    public override ResourceDAO BuildCost { get => new ResourceDAO(wood: 75, stone: 20); }
    public override Vector2Int Size => new Vector2Int(0, 0);
    public override BuildingType Type => BuildingType.Wall;
    public override PathableType PathableType => PathableType.UnPathable;
    public override bool IsTower => false;
}
