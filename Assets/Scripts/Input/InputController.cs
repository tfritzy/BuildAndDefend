using UnityEngine;

public abstract class InputController
{
    public abstract InputDAO GetInput();
    public virtual bool IsActive { get; set; }

    protected Vector3? GetInputPosition()
    {
        Vector3? position = null;
        if (Input.GetMouseButton(0))
        {
            position = GameObjectCache.Camera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.touchCount > 0)
        {
            position = GameObjectCache.Camera.ScreenToWorldPoint(Input.GetTouch(0).position);
        }

        return position;
    }

}