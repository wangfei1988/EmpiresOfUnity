using System;
using UnityEngine;
using System.Collections;
using  System.Collections.Generic;

public class NaniteMine : ProductionBuilding
{
    new private enum OPTIONS { Upgrade = EnumProvider.ORDERSLIST.Upgrade }

    private OPTIONS NaniteMineState;
    private bool buildFinished = false;
    private bool buildFinishedLastFrame = false;

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
    }

    public override void BuildFinished()
    {
        this.buildFinished = true;
        this.buildFinishedLastFrame = true;
    }

    internal override void MoveAsGroup(GameObject leader)
    {
    }

    //Main Method for Mine
    protected override void MineWork()
    {
        if (EnoughEnergy)
        {
            ResourceManager.AddResouce(ResourceManager.Resource.NANITEN, 1);
        }
    }

    internal override void DoStart()
    {
        foreach (int value in System.Enum.GetValues(typeof(OPTIONS)))
        {
            OptionalStatesOrder.Add(value, ((OPTIONS)value).ToString());
        }
    }

    internal override void DoUpdate()
    {
        if (this.buildFinishedLastFrame)
        {
            this.buildFinishedLastFrame = false;
            NaniteMineCount++;
        }
        if (this.buildFinished)
        {
            this.UpdateProduction(UnitScript.UNITTYPE.NaniteMine);
        }
    }

    void OnDestroy()
    {
        
    }

    
}