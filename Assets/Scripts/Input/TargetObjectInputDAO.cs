using UnityEngine;

public class TargetInputDAO : InputDAO
{
    public GameObject Target;

    public override bool HasValue()
    {
        return this.Target != null;
    }
}