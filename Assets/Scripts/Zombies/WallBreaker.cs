using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBreaker : Zombie
{

    public override ResourceDAO KillReward { get => new ResourceDAO(gold: 30); }

    protected override void ChildrenSetup()
    {
        base.ChildrenSetup();
        this.health = 500;
        this.damage = 25;
        this.attackSpeed = 3f;
    }

    public override List<Vector2> RestartPath(bool attackClosestBuilding = false)
    {
        return base.RestartPath(attackClosestBuilding: true);
    }
}
