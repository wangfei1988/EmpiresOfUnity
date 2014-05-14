using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatterMine : ProductionBuilding 
{
    private Dictionary<uint, uint> MatterWork = new Dictionary<uint, uint>();


    void Start()
    {
        UpdateManager.OnUpdate += DoUpdate;
    }

    //Main Method for Mine
    protected override void MineWork()
    {
        uint resValue = 1;
        //MatterWork.TryGetValue(Level, out resValue);
        ResourceManager.AddResouce(ResourceManager.Resource.MATTER, resValue);
    }

    private void MatterMineEnergyConsumption()
    {
        ResourceManager.SubtractResouce(ResourceManager.Resource.ENERGY, 10);
    }


    void DoUpdate()
    {
       this.UpdateProduction();
    }

    void OnDestroy()
    {
        GiveEnergyBack();
        UpdateManager.OnUpdate -= DoUpdate;
    }
}
