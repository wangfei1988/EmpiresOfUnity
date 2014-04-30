using UnityEngine;
using System.Collections;
using  System.Collections.Generic;

public class NaniteMine : AbstractBuilding
{
    private  Dictionary<uint, uint> NaniteWork = new Dictionary<uint, uint>();
    private float workTimer;

    private void Start()
    {
        Life = (uint) SettingFile.Life;
        Level = (uint) SettingFile.Level;
        ViewDistance = (uint) SettingFile.ViewDistance;
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
                                
        UpdateHandler.OnUpdate += DoUpdate;
    }

    private void NaniteMineWork()
    {
        uint resValue = 0;
        NaniteWork.TryGetValue(Level, out resValue);
        ResourceManager.AddResouce(ResourceManager.Resource.NANITEN, resValue);
    }

    void DoUpdate()
    {
        workTimer += Time.deltaTime;
        if (workTimer >= 5)
        {
            workTimer = 0;
            NaniteMineWork();
        }
    }

    void OnDestroy()
    {
        UpdateHandler.OnUpdate -= DoUpdate;
    }
}