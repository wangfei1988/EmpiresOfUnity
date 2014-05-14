using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class BuildingBuilder : MonoBehaviour
{

    public GameObject Prefab;
    public GridSystem Grid;

    private Transform Transform;
    private bool dragNow = false;

    /* Start & Update */
	void Start ()
    {
        this.Grid = this.GetComponent<GridSystem>();
        //UpdateManager.OnMouseUpdate += DoUpdate;
    }
	
	void DoUpdate ()
	{
	    if (this.dragNow == false)
	    {
	        this.CreatePrefab();
	    }

        if (this.dragNow == true)
        {
	        DragObject();
	    }
    }

    /* Methods */
    private void CreatePrefab()
    {
        Vector3 StartPosition = new Vector3(0, 0, 0);
        GameObject newBuilding = GameObject.Instantiate(Prefab, StartPosition, Quaternion.identity) as GameObject;
        this.Transform = newBuilding.transform;

        // Config me
        this.Transform.GetComponent<MeshCollider>().enabled = false;

        // Config
        this.dragNow = true;
        this.Grid.ShowGrid = true;
    }

    private void DragObject()
    {
        
        Vector3 pos = this.Grid.DragObjectPosition(this.Transform);

        this.Transform.position = pos;
    }

    private void DragFinished()
    {

        // Config me
        this.Transform.GetComponent<MeshCollider>().enabled = true;

        // Config
        this.Transform = null;
        this.Grid.ShowGrid = false;
        this.dragNow = false;
    }



}
