using System;
using UnityEngine;
using System.Collections.Generic;

public class MatterMine : ProductionBuilding
{
    private Dictionary<uint, uint> MatterWork = new Dictionary<uint, uint>();
    new private enum OPTIONS { Upgrade = EnumProvider.ORDERSLIST.Upgrade }

    private OPTIONS MatterMineState;

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
        MatterMineCount++;
        UpdateManager.OnUpdate += DoUpdate;
        //base.DoStart();

        foreach (int value in System.Enum.GetValues(typeof(OPTIONS)))
        {
            OptionalStatesOrder.Add(value, ((OPTIONS)value).ToString());
        }
    }


    public override void BuildFinished()
    {
        this.BuildingCost();
    }
   

    //internal override void DoUpdate()
    void DoUpdate()
    {
        this.UpdateProduction(UnitScript.UNITTYPE.MatterMine);
    }

    internal override void MoveAsGroup(GameObject leader)
    {
    }

    protected override void MineWork()
    {
        ResourceManager.AddResouce(ResourceManager.Resource.MATTER, 1);
    }

    void OnDestroy()
    {
        UpdateManager.OnUpdate -= DoUpdate;
    }
}
