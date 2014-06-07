using UnityEngine;
using System.Collections;

public class MouseMovement : MonoBehaviour 
{

    // if Activated is set to true, Speed will contain the actual mousemovementspeed all the time.
    // if Activated is set to false, Speed will only contain actual movementspeed data while MIDDLE-button is hold down, otherwise Vector2.zero...
    private bool _activated=false;
    public bool Activated
    {
        get { return _activated; }
        set 
        {
            if (_activated != value)
            {
                if (value)
                {
                    UpdateManager.OnMouseUpdate += UpdateManager_OnMouseUpdate;
                    MouseEvents.RIGHTCLICK -= MouseEvents_CLICK;
                    MouseEvents.RIGHTRELEASE -= MouseEvents_RELEASE;
                    lastMousePosition = MouseEvents.State.Position;
                }
                else
                {
                    UpdateManager.OnMouseUpdate -= UpdateManager_OnMouseUpdate;
                    MouseEvents.RIGHTCLICK += MouseEvents_CLICK;
                    MouseEvents.RIGHTRELEASE += MouseEvents_RELEASE;
                    Speed = Vector2.zero;
                }
            }
        }
    }
    public Vector2 Speed
    {
        get;
        private set;
    }
    private Vector2 lastMousePosition = Vector2.zero;


    void Start()
    {
        MouseEvents.RIGHTCLICK += MouseEvents_CLICK;
        MouseEvents.RIGHTRELEASE += MouseEvents_RELEASE;
    }

    void MouseEvents_CLICK(Ray qamRay, bool hold)
    {
        if (!hold)
        {
            lastMousePosition = MouseEvents.State.Position;
        }
        else
            UpdateManager_OnMouseUpdate();
    }
    void MouseEvents_RELEASE()
    {
        Speed = Vector2.zero;
    }

    private void UpdateManager_OnMouseUpdate()
    {
        Speed = MouseEvents.State.Position - lastMousePosition;
        lastMousePosition = MouseEvents.State.Position;
    }

}
