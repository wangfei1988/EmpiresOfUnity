using System;
using System.ComponentModel;
using UnityEngine;
using System.Collections;

public class Lifebar : ScriptableObject
{

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

    private Vector3 position;
    public Vector3 Position
    {
        get { return this.position; }
        set
        {
            this.position = value;
            Vector3 tempPos = this.position + Offset;
            if (this.Activated)
            {
                Vector3 screenPos = Camera.main.WorldToViewportPoint(tempPos);
                screenPos.z = 0;
                this.LifebarObject.position = screenPos;
            }
        }
    }

    /* Methods */
    private void CreateObject()
    {
        this.LifebarObject = GameObject.Instantiate(Prefab, Vector3.zero, Quaternion.identity) as Transform;
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
