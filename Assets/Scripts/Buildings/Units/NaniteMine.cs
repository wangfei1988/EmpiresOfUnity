using UnityEngine;
using System.Collections;
using  System.Collections.Generic;

public class NaniteMine : AbstractBuilding
{
    private  Dictionary<uint, uint> NaniteMineUpgrade = new Dictionary<uint, uint>(); 
    private  Dictionary<uint, uint> NaniteWork = new Dictionary<uint, uint>();
    private float workTimer;
    private bool energyConsumptionBool = true;

    private void Start()
    {
        //Add the Settings from the Setting Files(Resources/Balancing/Buildings)
        Life = (uint) SettingFile.Life;
        Level = (uint) SettingFile.Level;
        ViewDistance = (uint) SettingFile.ViewDistance;
        ProductionTime = (uint) SettingFile.ProductionTime;
        Level1Resource = (uint) SettingFile.Level1Resource;
        Level2Resource = (uint) SettingFile.Level2Resource;
        Level3Resource = (uint) SettingFile.Level3Resource;
        Level4Resource = (uint) SettingFile.Level4Resource;
        Level5Resource = (uint) SettingFile.Level5Resource;

        NaniteWork.Add(1, Level1Resource);
        NaniteWork.Add(2, Level2Resource);
        NaniteWork.Add(3, Level3Resource);
        NaniteWork.Add(4, Level4Resource);
        NaniteWork.Add(5, Level5Resource);

        NaniteMineUpgrade.Add(2, 50);
        NaniteMineUpgrade.Add(3, 100);
        NaniteMineUpgrade.Add(4, 150);
        NaniteMineUpgrade.Add(5, 200);

        UpdateManager.OnUpdate += DoUpdate;
    }

    //Main Method for Mine
    private void NaniteMineWork()
    {
        uint resValue = 0;
        NaniteWork.TryGetValue(Level, out resValue);
        ResourceManager.AddResouce(ResourceManager.Resource.NANITEN, resValue);
    }

    private void Upgrade()
    {

    }

    private void NaniteMineEnergyConsumption()
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
            NaniteMineWork();
        }
        //calls the method only once
        if (energyConsumptionBool)
        {
            NaniteMineEnergyConsumption();
            energyConsumptionBool = false;
        }

        //Destroy this Gameobject if Life is 0
        DestroyTheGameObject();
    }

    void OnDestroy()
    {
        UpdateManager.OnUpdate -= DoUpdate;
    }
}