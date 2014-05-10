using System;
using UnityEngine;
using System.Collections;
/*
 * Scrolling of Camera
 * @date 2014-04-26
 */
public class Scrolling : MonoBehaviour
{
    /* Member */
    public float SpeedScrollX = 10f;
    public float SpeedScrollY = 10f;
    public float SpeedZoom = 25f;
    public float SpeedRotate = 100f;

    /* Vars */
    public bool scrollingAllowed = true;
    private GUIScript mainGUI;

    private Transform camPoint;

    /* Properties */

    /* Start & Update */
    void Start()
    {
        mainGUI = this.GetComponent<GUIScript>();
        camPoint = Camera.main.transform.GetChild(0);
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

        Vector2 MousePosition = MouseEvents.State.Position;


        Vector3 direction = Vector3.zero;

        /* Srolling Left & Right */
        if (MousePosition.x < mainGUI.MapViewArea.xMin || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            direction += Vector3.left * SpeedScrollX;
        if (MousePosition.x > mainGUI.MainGuiArea.xMax || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            direction += Vector3.right * SpeedScrollX;

        /* Zoom in & out */
        if (Input.GetKey(KeyCode.R))
            direction += Vector3.forward * SpeedZoom;
        if (Input.GetKey(KeyCode.F))
            direction += Vector3.back * SpeedZoom;

        Camera.main.transform.Translate(direction * Time.deltaTime);


        /* Scrolling Up & Down */
        float x = 0f;
        float z = 0f;
        float degrees = Camera.main.transform.eulerAngles.y;
        if (MousePosition.y > mainGUI.MapViewArea.yMax || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            /* Calculate Sin & Cos to move Camera to this position */
            double angle = Math.PI * degrees / 180.0;
            x += (float)Math.Sin(angle);
            z += (float)Math.Cos(angle);
        }
        if (MousePosition.y < mainGUI.MapViewArea.yMin || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            double angle = Math.PI * degrees / 180.0;
            x -= (float)Math.Sin(angle);
            z -= (float)Math.Cos(angle);
        }
        if (x != 0f || z != 0f)
        {
            x *= SpeedScrollY * Time.deltaTime;
            z *= SpeedScrollY * Time.deltaTime;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + x, Camera.main.transform.position.y, Camera.main.transform.position.z + z);
        }

        /* Rotate Left & Right */
        if (Input.GetKey(KeyCode.Q))
            Camera.main.transform.RotateAround(camPoint.position, new Vector3(0.0f, 1.0f, 0.0f), Time.deltaTime * this.SpeedRotate);
        if (Input.GetKey(KeyCode.E))
            Camera.main.transform.RotateAround(camPoint.position, new Vector3(0.0f, -1.0f, 0.0f), Time.deltaTime * this.SpeedRotate);


        /* Space Key Switch Camera */
        if (Input.GetKeyDown(KeyCode.Space))
            Camera.main.GetComponent<Cam>().SwitchCam();

    }
}
