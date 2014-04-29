using UnityEngine;
using System.Collections;

public class Airport : AbstractBuilding 
{
    void Start()
    {
     Life = (uint) SettingFile.Life;
    }
}
