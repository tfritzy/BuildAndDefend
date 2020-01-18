using UnityEngine;

public class DragSelectInputDAO : InputDAO
{
    public Vector2? point1;
    public Vector2? point2;

    public DragSelectInputDAO(Vector2? point1, Vector2? point2)
    {
        this.point1 = point1;
        this.point2 = point2;
    }

    public override bool HasValue()
    {
        return point1.HasValue && point2.HasValue;
    }
}