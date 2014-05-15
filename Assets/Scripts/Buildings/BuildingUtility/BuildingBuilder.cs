using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class BuildingBuilder : MonoBehaviour
{
    private Focus.HANDLING focusHandling = Focus.HANDLING.None;
    //public GameObject Prefab;
    public GridSystem Grid;
    public Object[] BuildableBuildings;

    private Transform Transform;
    private bool dragNow = false;

    /* Start & Update */
	void Start ()
    {
        this.Grid = this.GetComponent<GridSystem>();
        UpdateManager.OnMouseUpdate += DoUpdate;
    }
	
	void DoUpdate ()
	{
	    //if (this.dragNow == false)
	    //{
	        //this.CreatePrefab();
	    //}

        if (this.dragNow == true)
        {
	        DragObject();

            if (MouseEvents.State.LEFT.Pressed && !MouseEvents.State.LEFT.Hold)
            {
                Vector2 mouseScreen = MouseEvents.State.Position;
                if (GUIScript.main.MapViewArea.Contains(mouseScreen))
                {
                    // Mouse in Map
                    this.DragFinished();
                }
                else
                {
                    // Mouse in GUI
                    this.DragCancel();
                }
            }

            if (MouseEvents.State.RIGHT.Pressed && !MouseEvents.State.RIGHT.Hold)
            {
                // Cancel at Right Click
                this.DragCancel();
            }

	    }
    }

    /* Methods */
    public void CreatePrefab(int index)
    {
        // focus on building builder
        this.gameObject.AddComponent<Focus>().Lock();

        Vector3 StartPosition = new Vector3(0, 0, 0);
        GameObject newBuilding = GameObject.Instantiate(this.BuildableBuildings[index], StartPosition, Quaternion.identity) as GameObject;
        this.Transform = newBuilding.transform;
        //this.Transform.GetComponent<BuildingGrower>().StartGrowing = false;

        // Config me
        this.Transform.GetComponent<MeshCollider>().enabled = false;

        // Config
        this.dragNow = true;
        this.Grid.ShowGrid = true;
    }

    private void DragObject()
    {

        Vector3 pos = MouseEvents.State.Position.AsWorldPointOnMap;

        this.Transform.position = pos;
    }

    private void DragFinished()
    {
        // Grid Building
        Vector3 pos = this.Grid.DragObjectPosition(this.Transform);
        this.Transform.position = pos;

        // Unlock Focus
        this.gameObject.GetComponent<Focus>().Unlock(this.gameObject);
        Component.Destroy(this.gameObject.GetComponent<Focus>());

        // Config me
        if (this.Transform != null)
        {
            this.Transform.GetComponent<MeshCollider>().enabled = true;
            //this.Transform.GetComponent<BuildingGrower>().StartGrowing = true;
        }

        // Config
        this.Transform = null;
        this.Grid.ShowGrid = false;
        this.dragNow = false;
    }

    private void DragCancel()
    {
        Destroy(this.Transform.gameObject);
        this.Transform = null;
        this.DragFinished();
    }


}
