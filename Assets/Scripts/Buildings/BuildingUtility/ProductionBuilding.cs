using UnityEngine;
using System.Collections;

public abstract class ProductionBuilding : AbstractBuilding
{
    protected float workTimer;

    protected bool firstStart = false;

    protected void UpdateProduction()
    {

        //Timer for Resources per Time(ProductionTime)
        workTimer += Time.deltaTime;

        if (workTimer >= (float)1 / (float)this.CurrentResource)
        {
            workTimer = 0;
            this.MineWork();
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

        //Destroy this Gameobject if Life is 0
    }

    protected abstract void MineWork();

}

