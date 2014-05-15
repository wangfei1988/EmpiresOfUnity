using UnityEngine;
using System.Collections;

public class VisibilityOnMiniMap : UnitComponent
{
    public GameObject Blib;
    void Awake()
    {
        ComponentExtendsTheOptionalstateOrder = false;
    }
    void Start()
    {
        PflongeOnUnit();
        if (UNIT.GoodOrEvil == FoE.GOODorEVIL.Good)
        {
            if (UNIT.IsABuilding)
                Blib = GameObject.Instantiate(BlibContainer.BuildingBlip, this.gameObject.transform.position, BlibContainer.BuildingBlip.transform.rotation) as GameObject;
            else
                Blib = GameObject.Instantiate(BlibContainer.UnitBlip, this.gameObject.transform.position, BlibContainer.UnitBlip.transform.rotation) as GameObject;
        }
        else
        {
            if (UNIT.IsABuilding)
                Blib = GameObject.Instantiate(BlibContainer.EnemyBuildingBlip, this.gameObject.transform.position, BlibContainer.EnemyBuildingBlip.transform.rotation) as GameObject;
            else
                Blib = GameObject.Instantiate(BlibContainer.EnemyUnitBlip, this.gameObject.transform.position, BlibContainer.EnemyUnitBlip.transform.rotation) as GameObject;
        }
    }
    public override void DoUpdate()
    {
        
    }
    protected override EnumProvider.ORDERSLIST on_UnitStateChange(EnumProvider.ORDERSLIST stateorder)
    {
        return stateorder;
    }
}
