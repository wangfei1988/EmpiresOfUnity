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


    internal override void DoStart()
    {
       
    }

    public override void BuildFinished()
    {
        this.BuildingCost();
    }

    internal override void DoUpdate()
    {

        //Check for Upgrade
        //Destroy this Gameobject if Life is 0

    }

    private void OnDestroy()
    {
        //ResourceManager.AddResouce(ResourceManager.Resource.ENERGY, (int)SettingFile.UsedEnergy);
    }
}
