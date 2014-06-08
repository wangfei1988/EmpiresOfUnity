using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[AddComponentMenu("Program-X/UNIT/UnitOptions (Standard GroundUnits)")]
public class GroundUnitOptions : MovingUnitOptions
{
    public override EnumProvider.UNITCLASS UNIT_CLASS
    {
        get { return EnumProvider.UNITCLASS.GROUND_UNIT; }
    }
    new public enum OPTIONS : int
    {
        Attack = EnumProvider.ORDERSLIST.Attack,
        Seek = EnumProvider.ORDERSLIST.Seek,
    }

    private OPTIONS unitState;
    protected override bool GotToDoPrimaryOrders
    {
        get
        {
            return !standardOrder;
        }
        set
        {

        }
    }
    public override System.Enum UnitState
    {
        get
        {
            if (System.Enum.IsDefined(typeof(OPTIONS), (OPTIONS)unitstateint))
                return unitState;
            return base.UnitState;
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
                        case OPTIONS.Attack:
                            {
                                this.GetComponent<Movability>().SetKinematic();
                                this.GetComponent<Movability>().WayPoints.Clear();
                                LockOnFocus();
                                break;
                            }
                        }
                    }
                    unitstateint = (int)order;
                    unitState = order;
                }
            }

            base.UnitState = value;
        }
    }


    internal override void MouseEvents_LEFTCLICK(Ray qamRay, bool hold)
    {
        if (!hold)
        {
            if ((standardOrder) && (!gameObject.GetComponent<Focus>()))
                gameObject.AddComponent<Focus>();

            if (gameObject.GetComponent<Focus>())
            {
                if (!standardOrder)
                    base.MouseEvents_LEFTCLICK(qamRay, hold);

                if (unitState == OPTIONS.Attack)
                {
                    UnitScript unit = MouseEvents.State.Position.AsUnitUnderCursor;
                    if (unit != null)
                    {
                        if (UNIT.IsEnemy(unit.GoodOrEvil))
                        {
                            Target = MouseEvents.State.Position.AsUnitUnderCursor.gameObject;
                            MoveToPoint = standardOrder ? ActionPoint ?? gameObject.transform.position : Target.transform.position;
                            IsMoving = true;
                            IsAttacking = true;
                        }
                        else if (UNIT.IsAllied(MouseEvents.State.Position.AsUnitUnderCursor.gameObject))
                        {
                            //Target = UnitUnderCursor.UNIT.SetInteracting(this.gameObject);
                            //if (UnitUnderCursor.UNIT.Options.IsAttacking) Target = UnitUnderCursor.UNIT.Options.Target;
                            //IsAttacking = true;
                            //MoveAsGroup(UnitUnderCursor.gameObject);
                        }
                    }

                    UnlockFocus();
                }


            }
        }

        base.MouseEvents_LEFTCLICK(qamRay, hold);
    }

    protected bool toDoToDo;
    protected override bool ProcessAllOrders()
    {
        bool toDoToDo = base.ProcessAllOrders();
        if (stillWorkDoDo)
        {
            if (ActionPoint != null)
                MouseEvents_LEFTCLICK(MouseEvents.State.Position.AsRay, false);
        }
        return toDoToDo;
    }

    internal override void FocussedLeftOnEnemy(GameObject enemy)
    {
        standardOrder = true;
        unitState = OPTIONS.Attack;
        MoveToPoint = enemy.transform.position;
        Target = enemy;
        IsAttacking = true;
        IsMoving = true;
        standardOrder = false;
    }

    private bool __attacking=false;
    public override bool IsAttacking
    {
        get
        {
            if (__attacking)
            {
                if (Target == null)
                    __attacking = false;
                else
                    MovingDirection = MoveToPoint;

                return IsMoving = true;
            }
            else { return false; }
        }
        protected set
        {
            if ((Target!=null)&&(unitState == OPTIONS.Attack))
                __attacking = value;
            else
                __attacking = false;
        }
    }
    private void Attack()
    {
        if (__attacking)
        {
            if (Distance <= UNIT.AttackRange)
            {
                UNIT.weapon.Reload();
                UNIT.weapon.Engage(Target);
            }
        }

    }



    internal override void DoStart()
    {
        base.DoStart();
        //foreach (int option in System.Enum.GetValues(typeof(OPTIONS)))
        //    if (!OptionalStatesOrder.ContainsKey(option))
        //        OptionalStatesOrder.Add(option, ((OPTIONS)option).ToString());
        RegisterInheridedOrderStateOptions(typeof(OPTIONS));
        unitstateint = 20;
        IsMoving = true;
    }


    internal override void DoUpdate()
    {
        if (IsAttacking)
        {
            Attack();
            if (Target)
                MoveToPoint = Target.transform.position;
        }
        //base.DoUpdate();

    }

}