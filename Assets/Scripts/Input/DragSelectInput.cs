using UnityEngine;

public class DragSelectInput : InputController
{
    protected bool isDragging;
    protected Vector2? dragStartPos;
    protected Vector2? dragEndPos;
    private Vector3 lastInputLocation;
    private DragSelectInputDAO emptyInputDAO = new DragSelectInputDAO(null, null);

    public override InputDAO GetInput()
    {
        Vector3? inputPosition = GetInputPosition();

        if (isDragging)
        {
            if (inputPosition.HasValue)
            {
                lastInputLocation = inputPosition.Value;
            }
            else
            {
                dragEndPos = lastInputLocation;
                isDragging = false;
                return new DragSelectInputDAO(dragStartPos, dragEndPos);
            }
        }
        else if (inputPosition.HasValue)
        {
            dragStartPos = inputPosition.Value;
            isDragging = true;
        }
        return emptyInputDAO;
    }
}