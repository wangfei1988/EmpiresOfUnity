using System;
using UnityEngine;
using System.Collections;
using  System.Collections.Generic;

public class NaniteMine : ProductionBuilding
{
    private Dictionary<uint, uint> NaniteWork = new Dictionary<uint, uint>();

    private enum OPTIONS { Upgrade = EnumProvider.ORDERSLIST.Upgrade }

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

    internal override void MoveAsGroup(GameObject leader)
    {
    }

    void Start()
    {
        
        UpdateManager.OnUpdate += DoUpdate;
    }

    //Main Method for Mine
    protected override void MineWork()
    {
        ResourceManager.AddResouce(ResourceManager.Resource.NANITEN, 1);
    }

    private void NaniteMineEnergyConsumption()
    {
        ResourceManager.SubtractResouce(ResourceManager.Resource.ENERGY, 10);
    }

    internal override void DoStart()
    {
  
    }

    void DoUpdate()
    {

        this.UpdateProduction();
    }

    void OnDestroy()
    {
        UpdateManager.OnUpdate -= DoUpdate;
    }
}