using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public abstract class AbstractBuilding : UnitOptions
{
    public BuildingSetting SettingFile;

    public uint MaxLevel
    {
        get
        {
            return (uint)this.SettingFile.LevelResource.Count;
        }
    }

    protected uint CurrentResource
    {
        get
        {
            return (uint)this.SettingFile.LevelResource[(int)SettingFile.Level];
        }
    }

    public void BuildingCost()
    {
        ResourceManager.SubtractResouce(ResourceManager.Resource.MATTER, this.SettingFile.MatterCost);
        ResourceManager.SubtractResouce(ResourceManager.Resource.NANITEN, this.SettingFile.NaniteCost);
    }

    private EnumProvider.ORDERSLIST unitState;
    public override Enum UnitState
    {
        get
        {
            return unitState;
        } 
        set
        {
            switch ((EnumProvider.ORDERSLIST)value)
            {
                    
            }
            unitState = (EnumProvider.ORDERSLIST)value;
        }
    }

    internal override void DoUpdate()
    {
    }

    public void SubtractLaborer()
    {

    }


    /// <summary>
    /// The upgrade building
    /// This method can be used to upgrade a building.
    /// The method checks the resources and the max level to deside about to upgrade or not to upgrade.
    /// </summary>
    //public void UpgradeBuilding()
    //{
    //    if (this.Level == 1 && this.Level != this.MaxLevel && ResourceManager.Resource.MATTER - (ResourceManager.Resource)this.MatterUpgradeCostLvl1 >= 0 && ResourceManager.Resource.NANITEN - (ResourceManager.Resource) this.NaniteUpgradeCostLvl1 >= 0 /*&&Button Upgrade pressed*/
    //    {
    //        this.Level = 2;
    //        ResourceManager.SubtractResouce(ResourceManager.Resource.MATTER, this.MatterUpgradeCostLvl1);
    //        ResourceManager.SubtractResouce(ResourceManager.Resource.NANITEN, this.NaniteUpgradeCostLvl1);
    //    }
    //}
}
