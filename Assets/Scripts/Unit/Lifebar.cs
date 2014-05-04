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
            this.activated = value;
            if (ParentContainer == null)
            {
                ParentContainer = GameObject.FindGameObjectWithTag("LifebarContainer").transform;
            }
            if (Prefab == null)
            {
                Prefab = GUIScript.main.GetComponent<GUIScript>().Prefab_Lifebar;
            }
            CreateObject();
        }
    }
    public Vector3 Position;
    public Transform LifebarObject;
    public static Transform Prefab;
    public static Transform ParentContainer;

    private void CreateObject()
    {
        this.LifebarObject = GameObject.Instantiate(Prefab, Position, Quaternion.identity) as Transform;
        this.LifebarObject.parent = ParentContainer;
    }



}
