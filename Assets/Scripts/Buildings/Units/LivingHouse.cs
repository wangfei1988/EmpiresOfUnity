using UnityEngine;
using System.Collections.Generic;

public class LivingHouse : AbstractBuilding
{
    private Dictionary<uint, uint> LivingHouseDic = new Dictionary<uint, uint>(); 
    private bool isAlreadyUsed = true;

    void Start()
    {
        UpdateManager.OnUpdate += DoUpdate;
    }

    private void LivingHouseWork()
    {
        uint resValue = 0;
        LivingHouseDic.TryGetValue((uint)SettingFile.Level, out resValue);
        ResourceManager.AddResouce(ResourceManager.Resource.LABORER, resValue);
    }

    void DoUpdate()
    {
        if (isAlreadyUsed)
        {
            LivingHouseWork();
            this.isAlreadyUsed = false;
        }

        DestroyTheGameObject();
    }

    void OnDestroy()
    {
        UpdateManager.OnUpdate -= DoUpdate;
    }
}
