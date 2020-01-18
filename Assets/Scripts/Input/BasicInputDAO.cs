using UnityEngine;

public class VectorInputDAO : InputDAO
{
    public VectorInputDAO(Vector2? location)
    {
        this.location = location;
    }

    public Vector2? location;

    public override bool HasValue()
    {
        return location.HasValue;
    }
}