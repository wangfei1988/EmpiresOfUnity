using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolarTower : AbstractBuilding 
{
    private Dictionary<uint, uint> SolarWork = new Dictionary<uint, uint>();

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

    private void SolarTowerWork()
    {
        uint resValue = 0;
        SolarWork.TryGetValue(SettingFile.Level, out resValue);
        ResourceManager.AddResouce(ResourceManager.Resource.ENERGY, resValue);
    }

    internal override void DoStart()
    {
       
    }

    public override void BuildFinished()
    {
        this.BuildingCost();
    }

    internal override void DoUpdate()
    {
        SolarTowerWork();

        //Check for Upgrade
        //Destroy this Gameobject if Life is 0

    }

    private void OnDestroy()
    {
        //ResourceManager.AddResouce(ResourceManager.Resource.ENERGY, (int)SettingFile.UsedEnergy);
    }
}
