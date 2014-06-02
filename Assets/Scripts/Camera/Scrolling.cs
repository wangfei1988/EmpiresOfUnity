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
    public float SpeedScrollX = 20f;
    public float SpeedScrollY = 20f;
    public float SpeedZoom = 25f;
    public float SpeedRotate = 100f;
    public float ShiftKeyMulti = 3f;

    /* Vars */
    public bool scrollingAllowed = true;
    private GUIScript mainGUI;
    public MouseMovement MouseMove;

    /* Properties */

    /* Start & Update */
    void Start()
    {
        mainGUI = this.GetComponent<GUIScript>();
        MouseMove = this.gameObject.AddComponent<MouseMovement>();
        UpdateManager.OnUpdate += DoUpdate;
    }

    void DoUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Y))
            this.SwitchScrollingStatus();

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
        if (
            (scrollingAllowed && MousePosition.x < mainGUI.MapViewArea.xMin)
            || Input.GetKey(KeyCode.A)
            || Input.GetKey(KeyCode.LeftArrow)
            )
            direction += Vector3.left * SpeedScrollX;

        if (
            (scrollingAllowed && MousePosition.x > mainGUI.MainGuiArea.xMax)
            || Input.GetKey(KeyCode.D)
            || Input.GetKey(KeyCode.RightArrow)
            )
            direction += Vector3.right * SpeedScrollX;

        /* Zoom in & out */
        if (Input.GetKey(KeyCode.R))
            direction += Vector3.forward * SpeedZoom;
        if (Input.GetKey(KeyCode.F))
            direction += Vector3.back * SpeedZoom;

        /* Multi via Shift-Press */
        if (Input.GetKey(KeyCode.LeftShift))
        {
            direction *= this.ShiftKeyMulti;
        }

        Camera.main.transform.Translate(direction * Time.deltaTime);


        ///* Rotate Up & Down */
        //if (MouseMove.Speed.y != 0)
        //{
        //    if (MouseMove.Speed.y > 0f)
        //        Camera.main.transform.forward = (Camera.main.transform.forward + (Camera.main.transform.up / 100f)).normalized;
        //    else if (MouseMove.Speed.y < 0f)
        //        Camera.main.transform.forward = (Camera.main.transform.forward - (Camera.main.transform.up / 100f)).normalized;
        //}

        /*
         * Scrolling Up & Down
         * Calculate Sin & Cos to move Camera to this position
         * by Dario
         */
        float x = 0f;
        float z = 0f;
        float degrees = Camera.main.transform.eulerAngles.y;
        if ((scrollingAllowed && MousePosition.y > mainGUI.MapViewArea.yMax) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            double angle = Math.PI * degrees / 180.0;
            x += (float)Math.Sin(angle);
            z += (float)Math.Cos(angle);
        }
        if ((scrollingAllowed && MousePosition.y < mainGUI.MapViewArea.yMin) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            double angle = Math.PI * degrees / 180.0;
            x -= (float)Math.Sin(angle);
            z -= (float)Math.Cos(angle);
        }
        if (x != 0f || z != 0f)
        {
            x *= SpeedScrollY * Time.deltaTime;
            z *= SpeedScrollY * Time.deltaTime;

            /* Multi via Shift-Press */
            if (Input.GetKey(KeyCode.LeftShift))
            {
                x *= this.ShiftKeyMulti;
                z *= this.ShiftKeyMulti;
            }
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + x, Camera.main.transform.position.y, Camera.main.transform.position.z + z);
        }

        /* Rotate Left & Right */
        int status = 0;
        if (Input.GetKey(KeyCode.Q) || (MouseMove.Speed.x < 0))
            status = 1;
        if (Input.GetKey(KeyCode.E) || (MouseMove.Speed.x > 0))
            status = -1;
        if (status != 0)
        {
            // Raycast to center of screen
            int screenX = Screen.width / 2;
            int screenY = Screen.height / 2;
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY));
            RaycastHit hit;
            if (Ground.Current.collider.Raycast(ray, out hit, Camera.main.farClipPlane))
            {
                Vector3 camPoint = hit.point;
                Camera.main.transform.RotateAround(camPoint, new Vector3(0.0f, 1.0f * status, 0.0f), Time.deltaTime * this.SpeedRotate);
            }
        }

        /* Space Key Switch Camera */
        if (Input.GetKeyDown(KeyCode.Space))
            Camera.main.GetComponent<Cam>().SwitchCam();

    }
}
