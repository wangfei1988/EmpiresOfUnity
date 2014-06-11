using UnityEngine;
using System.Collections.Generic;

public class LivingHouse : AbstractBuilding
{
    //private Dictionary<uint, uint> LivingHouseDic = new Dictionary<uint, uint>(); 
    //private bool isAlreadyUsed = true;
    private uint currentResidentHuman;

    new private enum OPTIONS { Upgrade = EnumProvider.ORDERSLIST.Upgrade }

    public override System.Enum UnitState
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
        LivingHouseWork();
    }

    internal override void DoStart()
    {

    }

    internal override void DoUpdate()
    {
        
    }

    internal override void MoveAsGroup(GameObject leader)
    {
    }

    private void LivingHouseWork()
    {
        uint resValue = this.CurrentResource;
        ResourceManager.AddResouce(ResourceManager.Resource.MAXLABORER, resValue);
        currentResidentHuman = resValue;
    }

    public void SubtractLaborer()
    {
        ResourceManager.SubtractResouce(ResourceManager.Resource.MAXLABORER, currentResidentHuman);
    }

    void OnDestroy()
    {
        this.SubtractLaborer();
    }
}
