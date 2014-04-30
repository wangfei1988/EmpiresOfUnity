using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[AddComponentMenu("Character/Unit Options (Standard GroundUnit)")]
class GroundUnitOptions : MovingUnitOptions
{
    new public enum OPTIONS : int
    {
 //       MoveTo = MovingUnitOptions.OPTIONS.MoveTo,
        Attack = 1,
 //       Guard = MovingUnitOptions.OPTIONS.Guard,
 //       Patrol = MovingUnitOptions.OPTIONS.Patrol,
 //       Hide = MovingUnitOptions.OPTIONS.Hide,
        Seek=35,
//        Stay = MovingUnitOptions.OPTIONS.Stay,
 //       Cancel = MovingUnitOptions.OPTIONS.Cancel
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
           if (gameObject.GetComponent<Focus>())
           {
               base.MouseEvents_LEFTCLICK(qamRay, hold);

               if (unitState == OPTIONS.Attack)
               {
                   RaycastHit Hit;
                   if (Physics.Raycast(qamRay, out Hit, Camera.main.transform.position.y))
                   {
                       if (TargetIsEnemy(Hit.collider.gameObject))
                       {
                           Target = Hit.collider.gameObject;
                           MoveToPoint = Target.transform.position;
                           IsMoving = true;
                           IsAttacking = true;
                       }
                   }
                   MouseEvents.LEFTCLICK -= MouseEvents_LEFTCLICK;
                   UnlockFocus();
               }
           }
       }
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
            if (__attacking) { CalculateDirection(); return IsMoving = true; }

            else { return false; }

        }
        protected set
        {
            if (unitState == OPTIONS.Attack) __attacking = value;
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
