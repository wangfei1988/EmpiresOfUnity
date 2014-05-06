using System;
using UnityEngine;
using System.Collections;

public class Lifebar : ScriptableObject
{

    private bool activated = false;
    public bool Activated
    {
        get
        {
            return this.activated;
        }
        set
        {
            //Debug.Log(value);
            if (this.activated != value)
            {
                this.activated = value;
                if (ParentContainer == null)
                    ParentContainer = GameObject.FindGameObjectWithTag("LifebarContainer").transform;
                if (Prefab == null)
                {
                    Prefab = GUIScript.main.GetComponent<GUIScript>().Prefab_Lifebar;
                }
                
                if (activated == true)
                    CreateObject();
                else
                    DestroyObject();
            }
        }
    }
    private Vector3 position;
    private Vector3 Offset = new Vector3(0, 5f, 0);
    public Vector3 Position
    {
        set
        {
            this.position = value + Offset;
            if(cam == null)
                cam = Camera.main;
            if (this.Activated)
            {
                float camDistance = Vector3.Distance(cam.transform.position, this.position);
                this.LifebarObject.position = this.position;
                if(initialScale == Vector3.zero)
                    initialScale = Prefab.localScale;
                this.LifebarObject.localScale = initialScale * camDistance / 1000 * Scale;
            }
        }
        get { return this.position; }
    }
    public Transform LifebarObject;
    public static Transform Prefab;
    public static Transform ParentContainer;

    /* Camera Relative Size */
    private Camera cam;
    private Vector3 initialScale; 
    private float Scale = 5.0f;

    /* Methods */

    private void CreateObject()
    {
        this.LifebarObject = GameObject.Instantiate(Prefab, Position, Quaternion.identity) as Transform;
        this.LifebarObject.parent = ParentContainer;
    }

    private void DestroyObject()
    {
        Destroy(this.LifebarObject.gameObject);
    }

}
