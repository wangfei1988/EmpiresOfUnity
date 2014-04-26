using UnityEngine;
using System.Collections;

public class Scrolling : MonoBehaviour
{
    /* Member */
    public Vector2 scrollSpeed = new Vector2(0.5f, 0.5f);

    /* Vars */
    private bool scrollingAllowed = false;
    private GUIScript mainGUI;

    /* Properties */

    /* Start & Update */
    void Start()
    {
        mainGUI = this.GetComponent<GUIScript>();
        UpdateHandler.OnUpdate += DoUpdate;
    }

    void DoUpdate()
    {
        if (scrollingAllowed)
            CheckForScrolling();
    }

    public void SwitchScrollingStatus()
    {
        this.scrollingAllowed = !this.scrollingAllowed;
    }

    private void CheckForScrolling()
    {
        /* TODO LUCAS 2014-04-26 -> Check WASD or Arrow-Keys for Scrolling */

        Vector2 MousePosition = MouseEvents.State.Position;

        float x = 0;
        float y = 0;

        if (MousePosition.x < mainGUI.MapViewArea.xMin)
            x = scrollSpeed.x * -1;
        else if (MousePosition.x > mainGUI.MainGuiArea.xMax)
            x = scrollSpeed.x;

        if (MousePosition.y > mainGUI.MapViewArea.yMax)
            y = scrollSpeed.y;
        else if (MousePosition.y < mainGUI.MapViewArea.yMin)
            y = scrollSpeed.y * -1;

        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + x, Camera.main.transform.position.y, Camera.main.transform.position.z + y);
    }
}
