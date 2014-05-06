using UnityEngine;
using System.Collections;

public class LivingHouse : AbstractBuilding
{
    private bool isAlreadyUsed = true;

    void Start()
    {
        Life = (uint) SettingFile.Life;
        Level = (uint) SettingFile.Level;
        ViewDistance = (uint) SettingFile.ViewDistance;
        

        UpdateManager.OnUpdate += DoUpdate;
    }

    private void LivingHouseWork()
    {

    }

    void DoUpdate()
    {
        if (isAlreadyUsed)
        {
            LivingHouseWork();
            this.isAlreadyUsed = false;
        }
    }

    void OnDestroy()
    {
        UpdateManager.OnUpdate -= DoUpdate;
    }
}
