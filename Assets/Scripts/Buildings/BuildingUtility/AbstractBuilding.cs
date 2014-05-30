using UnityEngine;
using System.Diagnostics;

public abstract class AbstractBuilding : UnitOptions
{
  //  public BuildingSetting SettingFile;    -now in UnitOptions !
    private EnumProvider.ORDERSLIST unitState;

    public uint MaxLevel
    {
        get
        {
            return (uint)this.SettingFile.LevelResource.Count;
        }
    }

    public int CurrentLevel
    {
        get
        {
            return this.SettingFile.Level;
        }
    }

    protected uint CurrentResource
    {
        get
        {
            return (uint)this.SettingFile.LevelResource[(int)SettingFile.Level];
        }
    }

    protected int GetBuildingCostMatter
    {
        get
        {
            return this.SettingFile.MatterCost;
        }
    }

    protected int GetBuildingCostNaniten
    {
        get
        {
            return this.SettingFile.NaniteCost;
        }
    }

    // True / False if enouth  resources 2 build buidling
    protected bool CheckBuildable()
    {
        // check current resources - building cost 
            // return true
        // return false
        return true;
    }



    public override System.Enum UnitState
    {
        get
        {
            return unitState;
        } 
        set
        {
            //switch ((EnumProvider.ORDERSLIST)value)
            //{    
            //}
            unitState = (EnumProvider.ORDERSLIST)value;
        }
    }

    public abstract void BuildFinished();

    public void UpGradeBuilding()
    {
        if (CurrentLevel != MaxLevel)
        {
            if (ResourceManager.Resource.MATTER - (ResourceManager.Resource)this.SettingFile.MatterCost >= 0 && ResourceManager.Resource.NANITEN - (ResourceManager.Resource)this.SettingFile.NaniteCost >= 0)
            {
                //if (false)
                //{
                //    SettingFile.Level++;
                //}   
            }
        }
        else
        {
             //Show MaxLevel Reached
        }
    }
}
