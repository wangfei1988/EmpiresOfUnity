using UnityEngine;
using System.Collections;
using System.Collections.Generic;


abstract public class MovingUnitOptions : UnitOptions 
{
    
    new public enum OPTIONS : int
    {
        MoveTo = EnumProvider.ORDERSLIST.MoveTo,
        Guard = EnumProvider.ORDERSLIST.Guard,
        Patrol = EnumProvider.ORDERSLIST.Patrol,
        Hide = EnumProvider.ORDERSLIST.Hide,
        Stay = EnumProvider.ORDERSLIST.Stay,
    }

    virtual public float Speed
    {
        get { return movement.Speed; }
        set
        {
            movement.Speed = value;
        }
    }
    public Vector3 MovingDirection
    {
        get { return movement.MovingDirection; }
        set { movement.MovingDirection = value; }
    }

    public Movability movement;

    internal override void FocussedLeftOnGround(Vector3 worldPoint)
    {
        standardOrder = true;
     //   IsMovingAsGroup = true;
        movement.SetKinematic();
        UnitState = OPTIONS.MoveTo;
        movement.MoveToPoint = worldPoint;
        movement.MovingDirection = worldPoint;
   //     gameObject.transform.position += (Movement.MovingDirection * Movement.Speed);
        IsAttacking = false;
        Target = null;
        standardOrder = false;
        IsMoving = true;
        //Debug.Log("MovingUnitOptions->FocussedLeftOnGround");
    }


    public float standardYPosition
    {
        get { return movement.standardYPosition; }
        set { movement.standardYPosition = value; }
    }

    public override Vector3 MoveToPoint
    {
        get
        {
            return base.MoveToPoint;
        }
        internal set
        {
            base.MoveToPoint = value;
        }
    }

    public  bool IsMoving
    {
        get
        {
           return movement.IsMoving;
        }
        set
        {
            movement.IsMoving = value;
            //if (value) { if (!gameObject.GetComponent<Pilot>()) gameObject.AddComponent<Pilot>(); }
            //else
            //{
            //    if (!UNIT.IsAnAirUnit)
            //        if (gameObject.GetComponent<Pilot>()) { Component.Destroy(gameObject.GetComponent<Pilot>()); }
            //}
            //_ismooving = value;
        }
    }
  //  public List<Vector3> WayPoints;

    //protected Vector3 CalculateDirection()
    //{
    //    return MovingDirection = (MoveToPoint - gameObject.transform.position).normalized;
    //}

    //void OnCollisionExit(Collision collision)
    //{
    //    Movement.SetKinematic();
    //    Movement.CalculateDirection();
    //}


    public virtual float Distance
    {
        get
        {
            return movement.Distance;
          //  return Vector3.Distance(gameObject.transform.position, MoveToPoint);
        }
        protected set
        {
            movement.Distance = value;
            //if (value != distance)
            //{
            //    if (IsMovingAsGroup)
            //    {
            //        if (Target)
            //        {
            //            MoveToPoint = Target.transform.position;
            //            gameObject.transform.position += (MovingDirection * Speed);
            //        }
            //        else IsGroupLeader = true;
            //    }
            //}
        }
    }
    public bool IsMovingAsGroup
    {
        get { return movement.IsMovingAsGroup; }
        set
        {
            movement.IsMovingAsGroup = value;
            //if (value) IsMoving = true;
            //_groupmove = value;
        }
    }

    public bool IsGroupLeader
    {
        get { return movement.IsGroupLeader; }
        set { movement.IsGroupLeader = value; }
    }

    internal override void MoveAsGroup(GameObject leader)
    {

        movement.MoveAsGroup(leader);
        IsAttacking = false;
    }


    //private OPTIONS movingUnitState;
    public int unitstateint=0;
    public override System.Enum UnitState
    {
        get
        {
            return base.UnitState;
        }
        set
        {
            //Debug.Log("MovingUnitOIptions StateChange called!");
            base.UnitState = value;
            //OPTIONS order = (OPTIONS)value;
            //if (unitstateint != (int)order)
            //{

            //    if (!standardOrder)
            //    {
            //        switch (order)
            //        {
            //            case OPTIONS.MoveTo:
            //                {
            //                    SetKinematic();
            //                    WayPoints.Clear();
            //                    LockOnFocus();
            //                    break;
            //                }
            //            case OPTIONS.Patrol:
            //                {
            //                    SetKinematic();
            //                    LockOnFocus();
            //                    WayPoints.Add(gameObject.transform.position);
            //                    break;
            //                }
            //            case OPTIONS.Stay:
            //                {
            //                    SetKinematic();
            //                    UnlockFocus();
            //                    WayPoints.Clear();
            //                    IsAttacking = false;
            //                    MoveToPoint = gameObject.transform.position;
            //                    break;
            //                }
            //            case OPTIONS.Guard:
            //                {
            //                    SetKinematic();
            //                    LockOnFocus();
            //                    WayPoints.Clear();
            //                    break;
            //                }
            //        }
            //    }
                unitstateint = (int)(EnumProvider.ORDERSLIST)value;
                //movingUnitState = (OPTIONS)value;
    
        }
    }

    //internal override void MouseEvents_LEFTCLICK(Ray qamRay, bool hold)
    //{
        //if (movingUnitState == OPTIONS.MoveTo)
        //{
        //    MoveToPoint = standardOrder ? ActionPoint ?? gameObject.transform.position : MouseEvents.State.Position.AsWorldPointOnMap;
        //    CalculateDirection();
        //    IsMoving = true;
        //    if (!standardOrder)
        //    {
        //        UnlockFocus();
        //    }
        //}
        //else if (movingUnitState == OPTIONS.Guard)
        //{
        //        if (UNIT.IsAllied(MouseEvents.State.Position.AsUnitUnderCursor.gameObject))
        //        {
        //            Target = UnitUnderCursor.UNIT.SetInteracting(this.gameObject);
        //            MoveToPoint = Target.transform.position;
        //            IsMoving = true;
        //        }

        //    DestroyFocus();
        //}
        //else if (movingUnitState == OPTIONS.Patrol)
        //{
        //    WayPoints.Add(standardOrder ? ActionPoint ?? gameObject.transform.position : MouseEvents.State.Position.AsWorldPointOnMap);
        //    IsMoving = true;

        //    if (!standardOrder)
        //        UnlockFocus();
        //    else
        //        DestroyFocus();
        //}
    //}

    //internal override void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold)
    //{
    //    if (!hold)
    //    {
    //        if (HasFocus)
    //        {
    //            if (movingUnitState == OPTIONS.Patrol)
    //            {
    //                if (WayPoints.Count >= 2) WayPoints.Insert(WayPoints.Count - 1, MouseEvents.State.Position.AsWorldPointOnMap);
    //                else WayPoints.Add(MouseEvents.State.Position.AsWorldPointOnMap);
    //                CalculateDirection();
    //                IsMoving = true;
    //            }
    //        }
    //    }
    //}

    protected bool stillWorkDoDo = false;
    protected override bool ProcessAllOrders()
    {
        stillWorkDoDo=base.ProcessAllOrders();
        if (GotToDoWhatGotToDo)
        {
            if (ActionPoint != null)
                MouseEvents_LEFTCLICK(MouseEvents.State.Position.AsRay, false);
        }
        return stillWorkDoDo;
    }




    internal override void DoStart()
    {

        foreach (int KeyValue in System.Enum.GetValues(typeof(OPTIONS)))
            if (!OptionalStatesOrder.ContainsKey(KeyValue))
                OptionalStatesOrder.Add(KeyValue, ((OPTIONS)KeyValue).ToString());

        standardYPosition = gameObject.transform.position.y;
        MoveToPoint = gameObject.transform.position;
        //unitstateint = 20;
        //movingUnitState = (OPTIONS)unitstateint;
        //UnitState = movingUnitState;
        IsMoving = true;
   //     Movement = this.gameObject.GetComponent<Movability>();
    }

    internal override void DoUpdate()
    {
      //  Movement.DoUpdate();

    }

}
