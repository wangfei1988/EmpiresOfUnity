using System;
using UnityEngine;
using System.Collections;
using  System.Collections.Generic;

public class NaniteMine : ProductionBuilding
{
    private enum OPTIONS { Upgrade = EnumProvider.ORDERSLIST.Upgrade }

    private OPTIONS NaniteMineState;

    public override Enum UnitState
    {
        get
        {
            return UnitState;
        }
        set
        {
            UnitState = (OPTIONS)value;
        }
    }

    public override EnumProvider.UNITCLASS UNIT_CLASS
    {
        get
        {
            return EnumProvider.UNITCLASS.BUILDING;
        }
    }

    void Start()
    {
        NaniteMineCount++;
    }

    public override void BuildFinished()
    {

    }

    internal override void MoveAsGroup(GameObject leader)
    {
    }

    //Main Method for Mine
    protected override void MineWork()
    {
        ResourceManager.AddResouce(ResourceManager.Resource.NANITEN, 1);
    }

    internal override void DoStart()
    {
        NaniteMineCount++;

        foreach (int value in System.Enum.GetValues(typeof(OPTIONS)))
        {
            OptionalStatesOrder.Add(value, ((OPTIONS)value).ToString());
        }
    }

    internal override void DoUpdate()
    {
        this.UpdateProduction(UnitScript.UNITTYPE.NaniteMine);
    }

    void OnDestroy()
    {
    }
}