using UnityEngine;

public abstract class DragSelectTower : Tower
{
    public override BuildingType Type => BuildingType.FireWall;
    protected override string projectilePrefabName => "FireWall";
    protected bool isDragging;
    protected Vector2 dragStartPos;
    protected Vector2 dragEndPos;

    private Vector2 lastInputLocation;
    protected override void Fire()
    {
        Vector2? inputLocation = this.GetInputLocation();
        if (isDragging)
        {
            if (inputLocation.HasValue)
            {
                lastInputLocation = inputLocation.Value;
            }
            else
            {
                dragEndPos = lastInputLocation;
                isDragging = false;
                this.CreateProjectile();
            }
        }
        else if (inputLocation.HasValue)
        {
            dragStartPos = inputLocation.Value;
            isDragging = true;
        }
    }

    protected abstract void CreateProjectile();
}