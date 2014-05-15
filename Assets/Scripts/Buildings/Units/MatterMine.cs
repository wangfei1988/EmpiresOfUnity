using System;
using UnityEngine;
using System.Collections.Generic;

public class MatterMine : AbstractBuilding
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
        //base.DoStart();

        foreach (int value in System.Enum.GetValues(typeof(OPTIONS)))
        {
            OptionalStatesOrder.Add(value, ((OPTIONS)value).ToString());
            
        }
    }

   

    internal override void DoUpdate()
    {
    }

    internal override void MoveAsGroup(GameObject leader)
    {
    }

    //Main Method for Mine
    protected void MineWork()
    {
        uint resValue = 1;
        //MatterWork.TryGetValue(Level, out resValue);
        ResourceManager.AddResouce(ResourceManager.Resource.MATTER, resValue);
    }
}
