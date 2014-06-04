using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolarTower : AbstractBuilding 
{
    //private Dictionary<uint, uint> SolarWork = new Dictionary<uint, uint>();
    private uint CurrentEnergy = 100;

    private bool firstStart = true;

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
    }

    internal override void DoUpdate()
    {
        if (firstStart)
        {
            ResourceManager.AddResouce(ResourceManager.Resource.ENERGY, CurrentEnergy);
            firstStart = false;
        }
    }

    private void OnDestroy()
    {
        ResourceManager.SubtractResouce(ResourceManager.Resource.ENERGY, CurrentEnergy);
    }
}
