using UnityEngine;
using System.Collections.Generic;

public class LivingHouse : AbstractBuilding
{
    private Dictionary<uint, uint> LivingHouseDic = new Dictionary<uint, uint>(); 
    private bool isAlreadyUsed = true;

    private enum OPTIONS { Upgrade = EnumProvider.ORDERSLIST.Upgrade }

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

    internal override void MoveAsGroup(GameObject leader)
    {
    }

    void Start()
    {
        UpdateManager.OnUpdate += DoUpdate;
    }

    private void LivingHouseWork()
    {
        uint resValue = 0;
        LivingHouseDic.TryGetValue((uint)SettingFile.Level, out resValue);
        ResourceManager.AddResouce(ResourceManager.Resource.LABORER, resValue);
    }

    internal override void DoStart()
    {
        
    }

    void DoUpdate()
    {
        if (isAlreadyUsed)
        {
            LivingHouseWork();
            this.isAlreadyUsed = false;
        }

    }

    void OnDestroy()
    {
        UpdateManager.OnUpdate -= DoUpdate;
    }
}
