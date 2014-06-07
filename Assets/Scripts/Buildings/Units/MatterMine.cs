using System;
using UnityEngine;
using System.Collections.Generic;

public class MatterMine : ProductionBuilding
{
    new private enum OPTIONS { Upgrade = EnumProvider.ORDERSLIST.Upgrade }

    private OPTIONS MatterMineState;
    private bool buildFinished = false;
    private bool buildFinishedLastFrame = false;

    public override Enum UnitState {
        get
        {
            return MatterMineState;
        }
        set
        {
            MatterMineState = (OPTIONS)value;
        } 
    }

    public override EnumProvider.UNITCLASS UNIT_CLASS
    {
        get
        {
            return EnumProvider.UNITCLASS.BUILDING;
        }
    }

    internal override void DoStart()
    {
        foreach (int value in System.Enum.GetValues(typeof(OPTIONS)))
        {
            OptionalStatesOrder.Add(value, ((OPTIONS)value).ToString());
        }
    }

    public override void BuildFinished()
    {
        this.buildFinished = true;
        this.buildFinishedLastFrame = true;
    }
   
    internal override void DoUpdate()
    {
        if (this.buildFinishedLastFrame)
        {
            this.buildFinishedLastFrame = false;
            MatterMineCount++;
        }
        if (this.buildFinished)
        {
            this.UpdateProduction(UnitScript.UNITTYPE.MatterMine);
        }
    }

    internal override void MoveAsGroup(GameObject leader)
    {
    }

    protected override void MineWork()
    {
        if (EnoughEnergy)
        {
            ResourceManager.AddResouce(ResourceManager.Resource.MATTER, 1);
        }
    }

    void OnDestroy()
    {
        MatterMineCount--;
    }
}
