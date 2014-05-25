using UnityEngine;
using System.Collections;

public class AirUnitOptions : UnitOptions
{
    public float turnDistance=50;
    public override EnumProvider.UNITCLASS UNIT_CLASS
    {
        get { return EnumProvider.UNITCLASS.AIR_UNIT; }
    }

    public Movability movement;

    internal override void DoStart()
    {
        RegisterInheridedOrderStateOptions(typeof(OPTIONS));
        movement = this.GetComponent<Movability>();
        this.gameObject.AddComponent<Pilot>().mySpace = this.gameObject.AddComponent<SphereCollider>();
        GetComponent<Pilot>().Controlls = movement;
        GetComponent<SphereCollider>().isTrigger = true;
    }
    internal override void DoUpdate()
    {
        if (IsFlying) IsFlying = Flight();
    }
    private bool _isflying = false;
    public bool IsFlying
    {
        get { return (movement.IsMoving) ? (_isflying = true) : _isflying; }
        set { _isflying = value ? (movement.IsMoving = true) : false; }
    }
    public Vector2 UnitPosition
    {
        get { return new Vector2(this.transform.position.x, this.transform.position.z); }
        set { this.transform.position = new Vector3(value.x, this.transform.position.y, value.y); }
    }
    private float lastYps=0;

    private bool Flight()
    {
        Vector2 MoveToPosition;
        if(movement.IsMoving)
            switch (airUnitState)
            {
            case OPTIONS.MoveTo:

               MoveToPosition=new Vector2(MoveToPoint.x,MoveToPoint.z);

                if (Vector2.Distance(UnitPosition, MoveToPosition) > 5)
                    UnitPosition += (MoveToPosition - UnitPosition).normalized * movement.Speed;
                else
                    movement.IsMoving = false;
                break;
            case OPTIONS.Patrol:
             //----TODO!.......
                return true;
            case OPTIONS.LandOnGround:
                if (Vector3.Distance(this.transform.position, MoveToPoint) > 0.5)
                    this.transform.position += (MoveToPoint - this.transform.position).normalized * movement.Speed;
                else
                {
                    this.transform.position = MoveToPoint;
                    return movement.IsMoving = false;
                }
                return true;
            }
        else
        {

            MoveToPosition = new Vector2(movement.WayPoints[0].x,movement.WayPoints[1].z);
            if(UnitPosition == MoveToPosition)
                if (lastYps >= this.transform.position.y)
                {
                    movement.WayPoints.Add(new Vector3(movement.WayPoints[0].z, movement.WayPoints[0].y, movement.WayPoints[0].x));
                    movement.WayPoints.RemoveAt(0);
                }
            lastYps = this.transform.position.y; 

            return true;
        }
        return false;
    }

    public enum OPTIONS : int
    {
        MoveTo = EnumProvider.ORDERSLIST.MoveTo,
        Patrol = EnumProvider.ORDERSLIST.Patrol,
        LandOnGround = EnumProvider.ORDERSLIST.LandOnGround,
    }
    public OPTIONS airUnitState;
    public override System.Enum UnitState
    {
        get
        {
            return base.UnitState;
        }
        set
        {
            if (System.Enum.IsDefined(typeof(OPTIONS), (OPTIONS)value))
            {
                airUnitState = (OPTIONS)value;
                switch (airUnitState)
                {
                    case OPTIONS.MoveTo:
                        LockOnFocus();

                        break;
                    case OPTIONS.LandOnGround:
                        LockOnFocus();
                        break;
                    case OPTIONS.Patrol:
                        LockOnFocus();
                        break;
                }
            }
            base.UnitState = value;
        }
    }


    internal override void MouseEvents_LEFTCLICK(Ray qamRay, bool hold)
    {
        if (!hold)
        {
            switch (airUnitState)
            {
                    case OPTIONS.MoveTo:
                        MoveToPoint = MouseEvents.State.Position;
                        Vector3 point = Random.onUnitSphere * (turnDistance / 2);
                        movement.WayPoints.Add((this.transform.position + point));
                        movement.WayPoints.Add((this.transform.position - point));
                        movement.IsMoving = true;
                        UnlockFocus();
                        break;
                    case OPTIONS.LandOnGround:
                        movement.WayPoints.Clear();
                        MoveToPoint = MouseEvents.State.Position;
                        movement.IsMoving = true;
                        UnlockFocus();
                        break;
                    case OPTIONS.Patrol:
                        movement.WayPoints.Clear();
                        movement.WayPoints.Add(MouseEvents.State.Position);
                        movement.IsMoving = true;
                        UnlockFocus();
                        break;
            }
        }
        base.MouseEvents_LEFTCLICK(qamRay, hold);
    }

    internal override void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold)
    {
        if (!hold)
        {
            if (airUnitState == OPTIONS.Patrol)
            {
                movement.WayPoints.Add(MouseEvents.State.Position);
                movement.IsMoving = true;
            }
        }
        base.MouseEvents_RIGHTCLICK(qamRay, hold);
    }

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
        movement.IsMoving = true;
        Debug.Log("MovingUnitOptions->FocussedLeftOnGround");
    }

    internal override void MoveAsGroup(GameObject leader)
    {
        
    }
}
