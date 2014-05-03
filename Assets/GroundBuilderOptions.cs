using UnityEngine;
using System.Collections;
using System.Reflection.Emit;

/*
[AddComponentMenu("Character/Unit Options (Standard GroundBuilder)")]
public class GroundBuilderOptions : MovingUnitOptions 
{


   new public enum OPTIONS : int
    {
        Build = 2,
        Repaire = 3,
    }
    internal override void DoStart()
    {
        base.DoStart();
        foreach (int option in System.Enum.GetValues(typeof(OPTIONS)))
            if (!OPTIONSlist.ContainsKey(option)) OPTIONSlist.Add(option, ((OPTIONS)option).ToString());
        UnitState = unitState;
        IsMoving = true;
    }
    private OPTIONS unitState;
    public override System.Enum UnitState
    {
        get
        {
            if (System.Enum.IsDefined(typeof(OPTIONS), (OPTIONS)unitstateint))
                return unitState;
            else return base.UnitState;
        }
        set
        {
            OPTIONS order;
            if (System.Enum.IsDefined(typeof(OPTIONS), (OPTIONS)value))
            {
                order = (OPTIONS)value;
                if (unitstateint != (int)order)
                {

                    if (!standardOrder)
                    {
                        switch (order)
                        {
                            case OPTIONS.Build:
                                {
                                        //todo                    -----------------  Build a building
                                    break;
                                }
                            case OPTIONS.Repaire:
                                {
                                    // todo             ------------- Repair other unit
                                    break;
                                }
                        }
                    }
                    unitstateint = (int)order;
                    unitState = order;
                }
            }
            else base.UnitState = value;
        }
    }

    internal override void MoveAsGroup(GameObject leader)
    {

    }

    internal override void FocussedRightOnGround(Vector3 worldPoint)
    {
        
    }
    internal override void FocussedLeftOnAllied(GameObject friend)
    {
        //todo---------------------------repair
    }
    internal override void FocussedLeftOnEnemy(GameObject enemy)
    {
        
    }

    internal override void DoUpdate()
    {
        base.DoUpdate();
    }
}
*/