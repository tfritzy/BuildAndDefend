using UnityEngine;

public class VectorClickInput : InputController
{
    private VectorInputDAO _inputDAO;
    public VectorInputDAO PlayerInput
    {
        get
        {
            if (_inputDAO == null)
            {
                _inputDAO = new VectorInputDAO(null);
            }
            return _inputDAO;
        }
        set
        {
            _inputDAO = value;
        }
    }

    public override InputDAO GetInput()
    {
        if (Input.GetMouseButton(0) || Input.touchCount > 0 && IsActive)
        {
            // TODO: Check what will happen if click on 0,0,0
            PlayerInput.location = Input.mousePosition != Vector3.zero
                ? (Vector2)Input.mousePosition
                : Input.GetTouch(0).position;
            PlayerInput.location = GameObjectCache.Camera.ScreenToWorldPoint(PlayerInput.location.Value);
        }
        else
        {
            PlayerInput.location = null;
        }
        return PlayerInput;
    }
}