using System;
using System.Collections.Generic;
using UnityEngine;

public class FlamePlumeProjectile : TargetLocationSpawningProjectile
{
    protected override TowerType TowerType => TowerType.FlamePlume;
}
