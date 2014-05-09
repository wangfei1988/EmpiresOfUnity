using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatterMine : AbstractBuilding 
{
    private Dictionary<uint, uint> MatterWork = new Dictionary<uint, uint>();
    private float workTimer;
    private bool energyConsumptionBool = true;

    private void Start()
    {
        //Add the Settings from the Setting Files(Resources/Balancing/Buildings)
        Life = (uint)SettingFile.Life;
        Level = (uint)SettingFile.Level;
        ViewDistance = (uint)SettingFile.ViewDistance;
        ProductionTime = (uint) SettingFile.ProductionTime;
        Level1Resource = (uint)SettingFile.Level1Resource;
        Level2Resource = (uint)SettingFile.Level2Resource;
        Level3Resource = (uint)SettingFile.Level3Resource;
        Level4Resource = (uint)SettingFile.Level4Resource;
        Level5Resource = (uint)SettingFile.Level5Resource;

        MatterWork.Add(1, Level1Resource);
        MatterWork.Add(2, Level2Resource);
        MatterWork.Add(3, Level3Resource);
        MatterWork.Add(4, Level4Resource);
        MatterWork.Add(5, Level5Resource);

        UpdateManager.OnUpdate += DoUpdate;
    }

    //Main Method for Mine
    private void MatterMineWork()
    {
        uint resValue = 0;
        MatterWork.TryGetValue(Level, out resValue);
        ResourceManager.AddResouce(ResourceManager.Resource.MATTER, resValue);
    }

    private void MatterMineEnergyConsumption()
    {
        ResourceManager.SubtractResouce(ResourceManager.Resource.ENERGY, 10);
    }


    void DoUpdate()
    {
        //Timer for Resources per Time(ProductionTime)
        workTimer += Time.deltaTime;
        if (workTimer >= ProductionTime)
        {
            workTimer = 0;
            MatterMineWork();
        }
        //calls the method only once
        if (energyConsumptionBool)
        {
            MatterMineEnergyConsumption();
            this.energyConsumptionBool = false;
        }
        
        //Destroy this Gameobject if Life is 0
        DestroyTheGameObject();
    }

    void OnDestroy()
    {
        UpdateManager.OnUpdate -= DoUpdate;
    }
}
