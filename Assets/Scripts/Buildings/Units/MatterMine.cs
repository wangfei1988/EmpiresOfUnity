using UnityEngine;
using System.Collections;

public class MatterMine : AbstractBuilding 
{
    void Start()
    {
        Life = (uint)SettingFile.Life;
        Level = (uint) SettingFile.Level;
        ViewDistance = (uint)SettingFile.ViewDistance;

        UpdateHandler.OnUpdate += DoUpdate;
    }


    void DoUpdate()
    {

    }

    void OnDestroy()
    {
        UpdateHandler.OnUpdate -= DoUpdate;
    }
}
