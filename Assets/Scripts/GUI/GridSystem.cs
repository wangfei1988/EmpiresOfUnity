using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridSystem : MonoBehaviour {

    /*
     * Unity: Easy Grid System
     * (c) by Dario D. Müller
     * <moin@game-coding.com>
     * Functionality:
     * -> Snap Objects
     * -> Grid Projector
     * -> Grid Lines Debug
     */

	public const string GRID_TAG = "GridObject";

	public Rect world = new Rect(-30f, -50f, 150f, 100f);
    public int gridWidth = 5;
	public GameObject ProjectorPrefab;
	public bool DebugLines = false;
	public Vector3 objectPivot = new Vector3(0, 0, 0);
    public bool ShowGrid = false;
	
	private GameObject Projector = null;
    private List<Vector3> startList = new List<Vector3>();
    private List<Vector3> endList = new List<Vector3>();
    private bool ShowGridCurrent = true;

	/* Use this for initialization grid-debug & projector */
	void Start () {
        InitGrid();
		SpawnProjector();
	    UpdateManager.OnUpdate += DoUpdate;
	}

    void DoUpdate()
    {
        // Change Grid Visibility
        if (this.ShowGridCurrent != this.ShowGrid)
        {
            this.ShowGridCurrent = this.ShowGrid;
            this.Projector.SetActive(this.ShowGrid);
        }
    }

	/* Initialize Grid Line Debug */
	private void InitGrid()
    {
        float lineY = 0.01f;
        /* Grid for Width */
        for (int i = (int)world.xMin; i <= world.xMin +  world.width; i += this.gridWidth)
        {
            startList.Add(new Vector3(i, lineY, world.yMin));
            endList.Add(new Vector3(i, lineY, world.yMin + world.height));
        }
        /* Grid for Height */
        for (int i = (int)world.yMin; i <= world.yMin + world.height; i += this.gridWidth)
        {
            startList.Add(new Vector3(world.xMin, lineY, i));
            endList.Add(new Vector3(world.xMin + world.width, lineY, i));
        }
    }

	/* Projector */
	private void SpawnProjector()
	{
		if (ProjectorPrefab != null)
		{
			this.Projector = GameObject.Instantiate(ProjectorPrefab) as GameObject;
			Light proj = this.Projector.GetComponent<Light>();
			proj.cookieSize = (float)gridWidth;
			Vector3 pos = this.Projector.transform.position;
			this.Projector.transform.position = new Vector3(this.world.xMin, pos.y, this.world.yMin);
		}
	}

	/* Draw Debug Lines */
    void OnDrawGizmos()
    {
		if (this.DebugLines)
		{
			for (int i = 0; i < this.startList.Count; i++)
			{
				Debug.DrawLine(startList[i], endList[i], Color.green);
			}
		}
    }

    /* Get grid'ed position of selected value */
    public Vector3 DragObjectPosition(Transform currObject)
    {
        Vector3 mouse = MouseEvents.State.Position.AsWorldPointOnMap;

        Vector3 offset = mouse - currObject.position;

        /* form the current position */
        Vector3 newPosition = currObject.position + offset;
        newPosition.y = currObject.position.y;

        /* grid it */
        int x = (int)newPosition.x / gridWidth;
        if (newPosition.x % gridWidth >= (float)gridWidth / 2)
            x += 1;
        int z = (int)newPosition.z / gridWidth;
        if (newPosition.x % gridWidth >= (float)gridWidth / 2)
            z += 1;

        /* set position*/
        Vector3 gridPosition = new Vector3(x * gridWidth, newPosition.y, z * gridWidth);
        return gridPosition + objectPivot;
    }

}
