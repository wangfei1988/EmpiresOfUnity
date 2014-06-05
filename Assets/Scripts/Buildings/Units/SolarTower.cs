using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolarTower : AbstractBuilding 
{
    //private Dictionary<uint, uint> SolarWork = new Dictionary<uint, uint>();
    private uint CurrentEnergy = 100;
    private bool builded = false;

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
        this.builded = true;
        ResourceManager.AddResouce(ResourceManager.Resource.ENERGY, CurrentEnergy);
        ResourceManager.AddResouce(ResourceManager.Resource.MAXENERGY, CurrentEnergy);
    }

    internal override void DoUpdate()
    {
    }

    private void OnDestroy()
    {
        if (this.builded)
        {
            ResourceManager.SubtractResouce(ResourceManager.Resource.ENERGY, CurrentEnergy);
            ResourceManager.SubtractResouce(ResourceManager.Resource.MAXENERGY, CurrentEnergy);
        }
    }
}
