using System;
using System.Collections.Generic;

using UnityEngine;
using System.Collections;

public abstract class AbstractBuilding : UnitOptions
{
    /// <summary>
    /// Main
    /// Contains the Mainstats of a Building.
    /// </summary>

    public uint MaxLevel
    {
        get
        {
            return (uint)this.SettingFile.LevelResource.Count;
        }
    }


    public BuildingSetting SettingFile;

    /// <summary>
    /// Gets the current resource.
    /// </summary>
    protected uint CurrentResource
    {
        get
        {
            return (uint)this.SettingFile.LevelResource[(int)SettingFile.Level];
        }
    }

    /// <summary>
    /// The building cost.
    /// Subtract the building cost from resources.
    /// Call only once!
    /// </summary>
    public void BuildingCost()
    {
        ResourceManager.SubtractResouce(ResourceManager.Resource.MATTER, this.SettingFile.MatterCost);
        ResourceManager.SubtractResouce(ResourceManager.Resource.NANITEN, this.SettingFile.NaniteCost);
    }

    /// <summary>
    /// The give energy back.
    /// Add the current energyconsumption to the resource energy if the gameobject get destroyed.
    /// </summary>
    public void GiveEnergyBack()
    {
        ResourceManager.AddResouce(ResourceManager.Resource.ENERGY, (uint)this.SettingFile.UsedEnergy);
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

    /*internal override void DoStart()
    {

        Debug.Log("Abstract UnitOption");
    }*/

    internal override void DoUpdate()
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
