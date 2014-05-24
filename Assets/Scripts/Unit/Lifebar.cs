using System;
using System.ComponentModel;
using UnityEngine;
using System.Collections;

public class Lifebar : ScriptableObject
{

    private Vector3 position;
    private Vector3 Offset = new Vector3(0f, 0f, 3f);

    public Transform LifebarObject;
    public Transform LifebarObjectInner;
    public static Transform Prefab;
    public static Transform ParentContainer;

    /* Camera Relative Size */
    //private Camera cam;
    //private Vector3 initialScale;
    //private float Scale = 5.0f;

    /* Life*/
    public int LifeDefault;
    private int lifeNow;
    public int Life
    {
        get
        {
            return this.lifeNow;
        }
        set
        {
            if (this.lifeNow != value)
            {
                this.lifeNow = value;
                this.UpdateLifeCount();
            }
        }
    }
    private int WidthDefault;
    public bool IsEnemy = false;

    /* Properties */
    private bool activated = false;
    public bool Activated
    {
        get
        {
            return this.activated;
        }
        set
        {
            if (this.activated != value)
            {
                this.activated = value;
                if (ParentContainer == null)
                {
                    GameObject LifebarContainer = GameObject.FindGameObjectWithTag("LifebarContainer");
                    if(LifebarContainer != null)
                        ParentContainer = LifebarContainer.transform;
                }
                if (Prefab == null)
                {
                    Prefab = GUIScript.main.GetComponent<GUIScript>().PrefabLifebar;
                }

                if (activated == true)
                    CreateObject();
                else
                    DestroyObject();
            }
        }
    }

    public Vector3 Position
    {
        set
        {
            this.position = value + Offset;
            //if(cam == null)
            //    cam = Camera.main;
            if (this.Activated)
            {
                //this.LifebarObject.position = this.position;


                Vector3 screenPos = Camera.main.WorldToViewportPoint(this.position);
                screenPos.z = 0;
                this.LifebarObject.position = screenPos;

                

                //float camDistance = Vector3.Distance(cam.transform.position, this.position);
                //if(initialScale == Vector3.zero)
                //    initialScale = Prefab.localScale;
                //this.LifebarObject.localScale = initialScale * camDistance / 1000 * Scale;
            }
        }
        get { return this.position; }
    }

    /* Methods */
    private void CreateObject()
    {
        this.LifebarObject = GameObject.Instantiate(Prefab, Position, Quaternion.identity) as Transform;
        this.LifebarObject.parent = ParentContainer;

        this.LifebarObjectInner = this.LifebarObject.FindChild("LifebarInner").transform;
        this.WidthDefault = (int)this.LifebarObjectInner.GetComponent<GUITexture>().pixelInset.width;

        // Color the Lifebar
        if(this.IsEnemy)
            this.LifebarObjectInner.GetComponent<GUITexture>().color = new Color(210f / 255f, 69f / 255f, 69f / 255f);

        // Life Count
        UpdateLifeCount();
    }

    private void DestroyObject()
    {
        if (this.LifebarObject != null)
            Destroy(this.LifebarObject.gameObject);
    }

    private void UpdateLifeCount()
    {
        if (this.LifebarObjectInner != null)
        {
            float newWidth = (float)this.Life / (float)this.LifeDefault * (float)this.WidthDefault;
            Rect pixelInset = this.LifebarObjectInner.GetComponent<GUITexture>().pixelInset;
            pixelInset.width = newWidth;
            this.LifebarObjectInner.GetComponent<GUITexture>().pixelInset = pixelInset;
        }
    }

}
