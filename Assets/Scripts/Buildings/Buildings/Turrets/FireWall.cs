using System.Collections.Generic;
using UnityEngine;

public class FireWall : DragSelectTower
{
    public override TowerType Type => TowerType.FireWall;

    protected override string projectilePrefabName => "FireWall";

    protected override void CreateProjectile()
    {
        List<GameObject> selectedTiles = new List<GameObject>();
        RaycastHit2D[] hits = Physics2D.LinecastAll(this.dragStartPos, this.dragEndPos);
        foreach (RaycastHit2D hit in hits)
        {

        }
    }
}