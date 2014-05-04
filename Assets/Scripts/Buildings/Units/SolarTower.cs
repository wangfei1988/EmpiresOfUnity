using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SolarTower : AbstractBuilding 
{
    private Dictionary<uint, uint> SolarWork = new Dictionary<uint, uint>();

    private Dictionary<uint, uint> SolarTowerUpgrade = new Dictionary<uint, uint>(); 

    private void Start()
    {
        Life = (uint)SettingFile.Life;
        Level = (uint)SettingFile.Level;
        ViewDistance = (uint)SettingFile.ViewDistance;
        Level1Resource = (uint)SettingFile.Level1Resource;
        Level2Resource = (uint)SettingFile.Level2Resource;
        Level3Resource = (uint)SettingFile.Level3Resource;
        Level4Resource = (uint)SettingFile.Level4Resource;
        Level5Resource = (uint)SettingFile.Level5Resource;

        SolarWork.Add(1, Level1Resource);
        SolarWork.Add(2, Level2Resource);
        SolarWork.Add(3, Level3Resource);
        SolarWork.Add(4, Level4Resource);
        SolarWork.Add(5, Level5Resource);

        SolarTowerUpgrade.Add(2, 50);
        SolarTowerUpgrade.Add(3, 100);
        SolarTowerUpgrade.Add(4, 150);
        SolarTowerUpgrade.Add(5, 200);

        UpdateManager.OnUpdate += DoUpdate;
    }

    private void SolarTowerWork()
    {
        uint resValue = 0;
        SolarWork.TryGetValue(Level, out resValue);
        ResourceManager.AddResouce(ResourceManager.Resource.ENERGY, resValue);
    }

    private void Upgrade()
    {
        if (useGUILayout)
        {
            
        }
    }

    private void DoUpdate()
    {
            SolarTowerWork();
    }

    private void OnDestroy()
    {
        UpdateManager.OnUpdate -= DoUpdate;
    }
}
