using UnityEngine;
using System.Collections;

public abstract class BuildMenu : MonoBehaviour
{
    public uint MatterMineBuildingCost_Naniten;
    public uint MatterMineBuildingCost_Matter;
    
    public uint NaniteMineBuildingCost_Naniten;
    public uint NaniteMineBuildingCost_Matter;

    public uint SolarTowerBuildingCost_Naniten;
    public uint SolarTowerBuildingCost_Matter;

    public BuildingCostBalancing SettingFile;
}
