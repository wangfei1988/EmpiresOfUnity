using UnityEngine;
using System.Collections.Generic;

public class LivingHouse : AbstractBuilding
{
    private Dictionary<uint, uint> LivingHouseDic = new Dictionary<uint, uint>(); 
    private bool isAlreadyUsed = true;
    private uint currentResidentHuman;

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
    void Start()
    {
        UpdateManager.OnUpdate += DoUpdate;
    }

    internal override void DoStart()
    {

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

    private void LivingHouseWork()
    {
        uint resValue = 0;
        LivingHouseDic.TryGetValue((uint)SettingFile.Level, out resValue);
        ResourceManager.AddResouce(ResourceManager.Resource.LABORER, resValue);
        currentResidentHuman = resValue;
    }

    void DoUpdate()
    {
        if (isAlreadyUsed)
        {
            LivingHouseWork();
            this.isAlreadyUsed = false;
        }
    }

    public void SubtractLaborer()
    {
        ResourceManager.SubtractResouce(ResourceManager.Resource.LABORER, currentResidentHuman);
    }

    void OnDestroy()
    {
        this.SubtractLaborer();
        UpdateManager.OnUpdate -= DoUpdate;
    }
}
