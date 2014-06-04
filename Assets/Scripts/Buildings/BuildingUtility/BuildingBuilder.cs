using System;
using UnityEngine;
using System.Collections;

using Object = UnityEngine.Object;

public class BuildingBuilder : ProductionBuilding
{
    //private Focus.HANDLING focusHandling = Focus.HANDLING.None;
    //public GameObject Prefab;
    public GridSystem Grid;
    public Object[] BuildableBuildings;

    private Transform Transform;
    private bool dragNow = false;
    private int curIndex;
    private Collider curCollider;
    private AnimatedCursor Cursor;

    private bool IsBuildable;
    private int solutionMatter;
    private int solutionNaniten;
    private int BuildingCostMatter;
    private int BuildingCostNanite;
    //Energy
    private int TempEnergy;
    private int CurrentEnergy;

    private string ResourceFeedBack;
    
    /* Start & Update */

    public override EnumProvider.UNITCLASS UNIT_CLASS
    {
        get
        {
            throw new System.NotImplementedException();
        }
    }

    internal override void MoveAsGroup(GameObject leader)
    {
        throw new System.NotImplementedException();
    }

    internal override void DoStart()
    {
        this.Grid = this.GetComponent<GridSystem>();
        UpdateManager.OnMouseUpdate += DoUpdate;
        this.Cursor = GUIScript.main.GetComponent<AnimatedCursor>();
        //throw new System.NotImplementedException();
    }

    internal override void DoUpdate ()
	{

        if (this.dragNow == true)
        {
	        DragObject();

            if (MouseEvents.State.LEFT.Pressed && !MouseEvents.State.LEFT.Hold)
            {
                Vector2 mouseScreen = MouseEvents.State.Position;
                if (GUIScript.main.MapViewArea.Contains(mouseScreen))
                {
                        if (IsBuildable == true)
                        {
                            // Mouse in Map
                            this.DragFinished();
                            this.BuildingCost();
                        }
                        else
                        {
                            Debug.Log("Need more Resources");
                        }
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
        this.BuildingCheck();
        this.CheckUsedEnergy();
        CurrentEnergy = (int)ResourceManager.GetResourceCount(ResourceManager.Resource.ENERGY);
        //Debug.Log(EnoughEnergy);
	}

    /* Methods */
    public void CreatePrefab(int index)
    {
        //Check if enough Resources to build 
        BuildingCostMatter = ((GameObject)this.BuildableBuildings[index]).GetComponent<UnitOptions>().SettingFile.MatterCost;
        BuildingCostNanite = ((GameObject)this.BuildableBuildings[index]).GetComponent<UnitOptions>().SettingFile.NaniteCost;
        IsBuildable = ((GameObject)this.BuildableBuildings[index]).GetComponent<UnitOptions>().SettingFile.IsBuildable;
        solutionMatter = (int)ResourceManager.GetResourceCount(ResourceManager.Resource.MATTER) -this.BuildingCostMatter;
        solutionNaniten = (int)(ResourceManager.GetResourceCount(ResourceManager.Resource.NANITEN) - this.BuildingCostNanite);
        //Energy
        TempEnergy = ((GameObject)this.BuildableBuildings[index]).GetComponent<UnitOptions>().SettingFile.UsedEnergy;

        

        this.curIndex = index;

        // focus on building builder
        this.gameObject.AddComponent<Focus>().Lock();

        Vector3 StartPosition = new Vector3(0, 0, 0);
        GameObject newBuilding = GameObject.Instantiate(this.BuildableBuildings[index], StartPosition, Quaternion.identity) as GameObject;
        this.Transform = newBuilding.transform;
        //this.Transform.GetComponent<BuildingGrower>().StartGrowing = false;

        // Config me
        this.curCollider = this.Transform.GetComponent<Collider>();
        this.curCollider.enabled = false;

        // Cursor
        this.Cursor.CurrentCursor = AnimatedCursor.CURSOR.DRAGnDROP;
        this.Cursor.LockCursor = true;

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
        // Unlock Focus
        this.gameObject.GetComponent<Focus>().Unlock(this.gameObject);
        Component.Destroy(this.gameObject.GetComponent<Focus>());

        // Config me
        if (this.Transform != null)
        {
            // Grid & Grow Building
            Vector3 pos = this.Grid.DragObjectPosition(this.Transform);
            pos.y = ((GameObject)this.BuildableBuildings[curIndex]).transform.position.y;
            this.Transform.position = pos;

            // enable Collider
            this.curCollider.enabled = true;

            // Start Grow
            this.Transform.GetComponent<BuildingGrower>().StartGrowing = true;
        }

        // Cursor
        this.Cursor.LockCursor = false;

        // Config
        this.Transform = null;
        this.Grid.ShowGrid = false;
        this.dragNow = false;

        EnergyConsumption += TempEnergy;

        IsBuildable = false;
    }

    private void DragCancel()
    {
        Destroy(this.Transform.gameObject);
        this.Transform = null;
        this.DragFinished();
        IsBuildable = false;
    }

    private void BuildingCheck()
    {
        if (solutionMatter >= 0 &&  solutionNaniten >= 0)
        {
            IsBuildable = true;
        }
        else
        {
            IsBuildable = false;
        }
    }

    //Subtract Resources for Building
    public void BuildingCost()
    {
        ResourceManager.SubtractResouce(ResourceManager.Resource.MATTER, (uint)this.BuildingCostMatter);
        ResourceManager.SubtractResouce(ResourceManager.Resource.NANITEN, (uint)this.BuildingCostNanite);
        ResourceManager.SubtractResouce(ResourceManager.Resource.ENERGY,(uint)this.TempEnergy);
    }

    public static void GiveEnergyBack()
    {
        ResourceManager.AddResouce(ResourceManager.Resource.ENERGY, (uint)this.TempEnergy);
    }

    public void CheckUsedEnergy()
    {
        EnergyConsumption = CurrentEnergy - TempEnergy;
        EnoughEnergy = EnergyConsumption >= 0;
    }

    public override void BuildFinished()
    {
    }

    protected override void MineWork()
    {
    }
}
