using System;
using UnityEngine;
using System.Collections;

public class BuildMenuUpdate : BuildMenu
{
    public bool BuildMode = true;
    public bool MatterMine = false;
    public bool NaniteMine = false;
    public bool SolarTower = false;

	void Start ()
	{
	    MatterMineBuildingCost_Matter = (uint)this.SettingFile.MatterMineBuildingCost_Matter;
	    MatterMineBuildingCost_Naniten = (uint)this.SettingFile.MatterMineBuildingCost_Naniten;
	    NaniteMineBuildingCost_Matter = (uint)this.SettingFile.NaniteMineBuildingCost_Matter;
	    NaniteMineBuildingCost_Naniten = (uint)this.SettingFile.NaniteMineBuildingCost_Naniten;
	    SolarTowerBuildingCost_Matter = (uint)this.SettingFile.SolarTowerBuildingCost_Matter;
	    SolarTowerBuildingCost_Naniten = (uint)this.SettingFile.SolarTowerBuildingCost_Naniten;

	    UpdateManager.OnUpdate += this.DoUpdate;
	}

    void DoUpdate()
    {
        if (this.BuildMode)
        {
            this.BuildingCheck();
        }
    }

    private void BuildingCheck()
    {
        if (ResourceManager.Resource.MATTER - (ResourceManager.Resource)this.MatterMineBuildingCost_Matter >= 0 && ResourceManager.Resource.NANITEN - (ResourceManager.Resource)this.MatterMineBuildingCost_Naniten >= 0)
        {
            this.MatterMine = true;
        }
        else
        {
            this.MatterMine = false;
        }

        if (ResourceManager.Resource.MATTER - (ResourceManager.Resource)this.NaniteMineBuildingCost_Matter >= 0 && ResourceManager.Resource.NANITEN - (ResourceManager.Resource)this.NaniteMineBuildingCost_Naniten >= 0)
        {
            this.NaniteMine = true;
        }
        else
        {
            this.NaniteMine = false;
        }

        if (ResourceManager.Resource.MATTER - (ResourceManager.Resource)this.SolarTowerBuildingCost_Matter >= 0 && ResourceManager.Resource.NANITEN - (ResourceManager.Resource)this.SolarTowerBuildingCost_Naniten >= 0)
        {
            this.SolarTower = true;
        }
        else
        {
            this.SolarTower = false;
        }
    }


}
