using System;
using System.Collections.Generic;
using UnityEngine;

public class FlamePlumeProjectile : TimedExplosionProjectile
{
    protected override BuildingType TowerType => BuildingType.FlamePlume;
}
