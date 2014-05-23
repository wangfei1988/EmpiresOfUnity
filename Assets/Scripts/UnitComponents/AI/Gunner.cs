using UnityEngine;
using System.Collections;

//--   A.I. to improve shooting-behavior of AttackUnits
[AddComponentMenu("Program-X/UNIT/AI - Gunner")]
public class Gunner : UnitComponent 
{

    void Awake()
    {
        
    }
    public override bool ComponentExtendsTheOptionalstateOrder
    {
        get
        {
            return false;
        }
    }
	void Start () 
    {
        PflongeOnUnit();
	}

    public override void DoUpdate()
    {
        throw new System.NotImplementedException();
    }

    protected override EnumProvider.ORDERSLIST on_UnitStateChange(EnumProvider.ORDERSLIST stateorder)
    {
        return stateorder;
    }
}
