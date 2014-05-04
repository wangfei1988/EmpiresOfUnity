using UnityEngine;
using System.Collections;

public class LivingHouse : AbstractBuilding 
{
    void Start()
    {
        Life = (uint) SettingFile.Life;
        Level = (uint) SettingFile.Level;
        ViewDistance = (uint) SettingFile.ViewDistance;

        UpdateManager.OnUpdate += DoUpdate;
    }

    void DoUpdate()
    {

    }

    void OnDestroy()
    {
        UpdateManager.OnUpdate -= DoUpdate;
    }
}
