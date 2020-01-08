using UnityEngine;

public class FireWall : DragSelectTower
{
    public override BuildingType Type => BuildingType.FireWall;

    protected override string projectilePrefabName => "FireWall";

    protected override void CreateProjectile()
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(this.dragStartPos, this.dragEndPos);
        foreach (RaycastHit2D hit in hits)
        {

        }
    }
}