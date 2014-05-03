using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[AddComponentMenu("Character/Unit Options (Standard GroundUnit)")]
class GroundUnitOptions : MovingUnitOptions
{
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
                             case OPTIONS.Attack:
                                 {
                                     SetKinematic();
                                     WayPoints.Clear();
                                     LockOnFocus();
                                     MouseEvents.LEFTCLICK += MouseEvents_LEFTCLICK;
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


   protected override void MouseEvents_LEFTCLICK(Ray qamRay, bool hold)
   {
       if (!hold)
       {
           if ((standardOrder) && (!gameObject.GetComponent<Focus>()))
               gameObject.AddComponent<Focus>();

           if (gameObject.GetComponent<Focus>())
           {
                
               if(!standardOrder) base.MouseEvents_LEFTCLICK(qamRay, hold);

               if (unitState == OPTIONS.Attack)
               {
                   if (TargetIsEnemy(UnitUnderCursor.gameObject))
                   {
                       Target = UnitUnderCursor.gameObject;
                       MoveToPoint = standardOrder ? ActionPoint ?? gameObject.transform.position : Target.transform.position;
                       IsMoving = true;
                       IsAttacking = true;
                   }
                   else if (TargetIsAllied(UnitUnderCursor.gameObject))
                   {
                        //Target = UnitUnderCursor.UNIT.SetInteracting(this.gameObject);
                        //if (UnitUnderCursor.UNIT.Options.IsAttacking) Target = UnitUnderCursor.UNIT.Options.Target;
                        //IsAttacking = true;
                        //MoveAsGroup(UnitUnderCursor.gameObject);
                   }
                   if(!standardOrder) MouseEvents.LEFTCLICK -= MouseEvents_LEFTCLICK;
                   UnlockFocus();
               }
           }
       }
   }

   protected bool toDoToDo;
   protected override bool ProcessAllOrders()
   {
       bool toDoToDo = base.ProcessAllOrders();
       if (stillWorkDoDo)
       {
           if (ActionPoint != null) MouseEvents_LEFTCLICK(MouseEvents.State.Position.AsRay, false);
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
                    CalculateDirection();
                if (Distance < UNIT.AttackRange)
                {
                    UNIT.weapon.Reloade();
                    UNIT.weapon.Engage(Target);
                }
                return IsMoving = true; 
            }
            else { return false; }

        }
        protected set
        {
            if ((Target!=null)&&(unitState == OPTIONS.Attack)) __attacking = value;
            else __attacking = false;

        }
    }



    internal override void DoStart()
    {
        base.DoStart();
        foreach (int option in System.Enum.GetValues(typeof(OPTIONS)))
            if (!OPTIONSlist.ContainsKey(option)) OPTIONSlist.Add(option, ((OPTIONS)option).ToString());

        unitstateint = 20;
        IsMoving = true;
    }


    internal override void DoUpdate()
    {
        if (IsAttacking) MoveToPoint = Target.transform.position;
        base.DoUpdate();

    }

}
