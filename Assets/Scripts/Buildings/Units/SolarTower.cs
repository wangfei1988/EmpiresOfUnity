using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolarTower : AbstractBuilding 
{
    private Dictionary<uint, uint> SolarWork = new Dictionary<uint, uint>();

    void Start()
    {
        UpdateManager.OnUpdate += DoUpdate;
    }

    private void SolarTowerWork()
    {
        uint resValue = 0;
        SolarWork.TryGetValue(SettingFile.Level, out resValue);
        ResourceManager.AddResouce(ResourceManager.Resource.ENERGY, resValue);
    }

    private void DoUpdate()
    {
        SolarTowerWork();

        //Check for Upgrade
        UpgradeBuilding();

        //Destroy this Gameobject if Life is 0
        DestroyTheGameObject();
    }

    private void OnDestroy()
    {
        //ResourceManager.AddResouce(ResourceManager.Resource.ENERGY, (int)SettingFile.UsedEnergy);
        UpdateManager.OnUpdate -= DoUpdate;
    }
}
