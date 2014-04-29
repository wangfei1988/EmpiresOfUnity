using UnityEngine;
using System.Collections;
/*
 * Scrolling of Camera
 * @date 2014-04-26
 */
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
        /* 
         * -> Check Q & E Key to rotate Camera left / right (like Banished)
         * -> Check R & F for zoom in / zoom out
         */

        Vector2 MousePosition = MouseEvents.State.Position;

        float x = 0;
        float y = 0;

        //Srolling Left
        if (MousePosition.x < mainGUI.MapViewArea.xMin || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            x = scrollSpeed.x * -1;
        //Scrolling Right
        else if (MousePosition.x > mainGUI.MainGuiArea.xMax || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            x = scrollSpeed.x;
        //Scrolling Up
        if (MousePosition.y > mainGUI.MapViewArea.yMax || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            y = scrollSpeed.y;
        //Scrolling Down
        else if (MousePosition.y < mainGUI.MapViewArea.yMin || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            y = scrollSpeed.y * -1;
        //Rotate Left
        if (Input.GetKey(KeyCode.Q))
        {
            
        }
        //Rotate Rigth
        if (Input.GetKey(KeyCode.E))
        {
            
        }
        //Zoom in
        if (Input.GetKey(KeyCode.R))
        {
            
        }
        //Zoom out
        if (Input.GetKey(KeyCode.F))
        {
            
        }
        //Space Key Switch Camera
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Camera.main.GetComponent<Cam>().SwitchCam();
        }

        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + x, Camera.main.transform.position.y, Camera.main.transform.position.z + y);
    }
}
