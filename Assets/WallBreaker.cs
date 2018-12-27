using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBreaker : Zombie {


    protected override void ChildrenSetup()
    {
        base.ChildrenSetup();
        this.health = 500;
        this.damage = 25;
        this.attackSpeed = 3f;
    }


    public override List<Vector2> RestartPath()
    {
        pathProgress = 0;
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        if (walls.Length == 0)
            return new List<Vector2>();
        GameObject closestWall = walls[0];
        float closestDist = (closestWall.transform.position - this.transform.position).magnitude;
        for (int i = 1; i < walls.Length; i++)
        {
            float testDist = (walls[i].transform.position - this.transform.position).magnitude;
            if (testDist < closestDist)
            {
                closestDist = testDist;
                closestWall = walls[i];
            }
        }
        this.target = closestWall;
        this.locationInGrid = builder.WorldPointToGridPoint(this.transform.position);
        this.targetLoc = builder.WorldPointToGridPoint(this.target.transform.position);
        this.path = FindPath(builder.grid, locationInGrid, this.targetLoc);
        return path;
    }
}
