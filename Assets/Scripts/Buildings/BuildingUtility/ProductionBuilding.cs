using System.Diagnostics;
using UnityEngine;
using System.Collections;

public abstract class ProductionBuilding : AbstractBuilding
{
    protected float workTimer;
    protected bool firstStart = false;

    protected static int MatterMineCount = 0;
    protected static int NaniteMineCount = 0;

    protected static int MatterCount = 0;
    protected static int NaniteCount = 0;

    protected void UpdateProduction(UnitScript.UNITTYPE type)
    {
        int dividier = 1;
        bool allowed = false;
        if (type == UnitScript.UNITTYPE.MatterMine)
        {
            dividier = MatterMineCount;
            MatterCount++;
            if (MatterCount > MatterMineCount)
                MatterCount = 1;
            if (MatterCount == 1)
                allowed = true;
        }
        else if (type == UnitScript.UNITTYPE.NaniteMine)
        {
            dividier = NaniteMineCount;
            NaniteCount++;
            if (NaniteCount > NaniteMineCount)
                NaniteCount = 1;
            if (NaniteCount == 1)
                allowed = true;
        }

        // If Player has several Miner -> only execute the first miner for counting all
        if (allowed)
        {
            //Timer for Resources per Time (ProductionTime)
            workTimer += Time.deltaTime;

            if (workTimer >= (float)1 / (float)this.CurrentResource / dividier)
            {
                workTimer = 0;
                this.MineWork();
            }
        }
        //calls the method only once
        if (this.firstStart)
        {
            this.firstStart = false;
            //MatterMineEnergyConsumption();
            //    BuildingCost();            
        }

        //Check for Upgrade
        // UpgradeBuilding();
    }

    protected abstract void MineWork();

}

