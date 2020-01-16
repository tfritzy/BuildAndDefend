using UnityEngine;

public class BasicInputDAO : InputDAO
{
    public Vector2? location;

    public override bool HasValue()
    {
        return location.HasValue;
    }
}