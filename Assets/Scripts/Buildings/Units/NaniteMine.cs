using UnityEngine;
using System.Collections;
using  System.Collections.Generic;

public class NaniteMine : ProductionBuilding
{
    private  Dictionary<uint, uint> NaniteWork = new Dictionary<uint, uint>();

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