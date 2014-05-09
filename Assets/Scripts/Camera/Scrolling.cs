using UnityEngine;
using System.Collections;
/*
 * Scrolling of Camera
 * @date 2014-04-26
 */
public class Scrolling : MonoBehaviour
{
    /* Member */
    private float SpeedScrollX = 10f;
    private float SpeedScrollY = 0.3f;
    private float SpeedZoom = 25f;
    private float SpeedRotate = 100f;

    /* Vars */
    private bool scrollingAllowed = false;
    private GUIScript mainGUI;

  //  private Transform camPoint;

    /* Properties */

    /* Start & Update */
    void Start()
    {
        mainGUI = this.GetComponent<GUIScript>();
  //      camPoint = Camera.main.transform;
        UpdateManager.OnUpdate += DoUpdate;
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


        Vector3 direction = Vector3.zero;
        
        //Srolling Left
        if (MousePosition.x < mainGUI.MapViewArea.xMin || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            direction += Vector3.left * SpeedScrollX;

        //Scrolling Right
        if (MousePosition.x > mainGUI.MainGuiArea.xMax || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            direction += Vector3.right * SpeedScrollX;

        //Zoom in
        if (Input.GetKey(KeyCode.R))
            direction += Vector3.forward * SpeedZoom;

        //Zoom out
        if (Input.GetKey(KeyCode.F))
            direction += Vector3.back * SpeedZoom;


        Camera.main.transform.Translate(direction * Time.deltaTime);





        float z = 0f;
        //Scrolling Up
        if (MousePosition.y > mainGUI.MapViewArea.yMax || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            //direction += Vector3.forward; //  * SpeedZoom
            z += 1f * SpeedScrollY;
        
        //Scrolling Down
        if (MousePosition.y < mainGUI.MapViewArea.yMin || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            //direction += Vector3.back; //  * SpeedZoom
            z += -1f * SpeedScrollY;


        if(z != 0f)
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + z);
        
        // Rotate Left
        //if (Input.GetKey(KeyCode.Q))
        //    Camera.main.transform.RotateAround(camPoint.position, new Vector3(0.0f, 1.0f, 0.0f), Time.deltaTime * this.SpeedRotate);
        
        //// Rotate Rigth
        //if (Input.GetKey(KeyCode.E))
        //    Camera.main.transform.RotateAround(camPoint.position, new Vector3(0.0f, -1.0f, 0.0f), Time.deltaTime * this.SpeedRotate);

        //Space Key Switch Camera
        if (Input.GetKeyDown(KeyCode.Space))
            Camera.main.GetComponent<Cam>().SwitchCam();

    }
}
