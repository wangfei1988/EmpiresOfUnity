﻿using System;
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

    /* Vars */
    public bool scrollingAllowed = true;
    private GUIScript mainGUI;

    /* Properties */

    /* Start & Update */
    void Start()
    {
        mainGUI = this.GetComponent<GUIScript>();
        UpdateManager.OnUpdate += DoUpdate;
    }

    void DoUpdate()
    {
        if (scrollingAllowed)
            CheckForScrolling();
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            this.SwitchScrollingStatus();
        }
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
        int status = 0;
        if (Input.GetKey(KeyCode.Q))
            status = 1;
        if (Input.GetKey(KeyCode.E))
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

       

    }
}
