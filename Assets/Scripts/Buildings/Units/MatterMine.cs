using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatterMine : AbstractBuilding 
{
    private Dictionary<uint, uint> MatterWork = new Dictionary<uint, uint>();
    private float workTimer;

    private void Start()
    {
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

    private void MatterMineWork()
    {
        uint resValue = 0;
        MatterWork.TryGetValue(Level, out resValue);
        ResourceManager.AddResouce(ResourceManager.Resource.MATTER, resValue);
    }

    void DoUpdate()
    {
        workTimer += Time.deltaTime;
        if (workTimer >= ProductionTime)
        {
            workTimer = 0;
            MatterMineWork();
        }
    }

    void OnDestroy()
    {
        UpdateManager.OnUpdate -= DoUpdate;
    }
}
