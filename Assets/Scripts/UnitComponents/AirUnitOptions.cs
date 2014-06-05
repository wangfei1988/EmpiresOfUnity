using UnityEngine;
using System.Collections;

[AddComponentMenu("Program-X/UNIT/UnitOptions (Air-Flying Units)")]
public class AirUnitOptions : UnitOptions
{
    public GameObject fdObj;
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
        this.gameObject.GetComponent<Pilot>().mySpace = this.gameObject.GetComponent<SphereCollider>();
        GetComponent<Pilot>().Controlls = movement;
        //  GetComponent<SphereCollider>().isTrigger = true;
        //movement.WayPoints.Add(new Vector3(120, 0, 25));
        //movement.WayPoints.Add(new Vector3(70, 0, -25));
    }
    internal override void DoUpdate()
    {
        fdObj.GetComponent<FaceDirection>().IsActive = movement.IsMoving;
        if (IsFlying)
            IsFlying = Flight();
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
    //private float lastYps=0;

    public override bool IsAttacking
    {
        get
        {
            if (GetComponent<Attackability>())
            {
                if (this.GetComponent<Attackability>().IsAttacking)
                {
                    IsFlying = true;
                    movement.IsMoving = true;
                    movement.Throttle += 0.01f;
                    return true;
                }
                else
                    return false;
            }

            else
                return base.IsAttacking;


        }
        protected set
        {
            if (GetComponent<Attackability>())
            {
                GetComponent<Attackability>().IsAttacking = value;
                IsFlying = value;
                movement.IsMoving = value;
                movement.Throttle += 0.01f;
            }
            else
                base.IsAttacking = value;
        }
    }

    private bool Flight()
    {
        Vector2 MoveToPosition;
        if (movement.IsMoving)
        {

            MoveToPosition = new Vector2(MoveToPoint.x, MoveToPoint.z);
            //if (IsAttacking)
            //{
            //    if (Vector2.Distance(UnitPosition, MoveToPosition) > 20f)
            //        UnitPosition += ((MoveToPosition - UnitPosition).normalized * movement.Speed);
            //    else
            //        this.gameObject.transform.position += (this.gameObject.transform.forward * movement.Speed);

            //    return true;
            //}
            if (!IsAttacking)
            {
                if (Vector2.Distance(UnitPosition, MoveToPosition) > 0.5f)
                    UnitPosition += (MoveToPosition - UnitPosition).normalized * movement.Speed;
                else
                    movement.IsMoving = false;


                if (baseUnitState == (EnumProvider.ORDERSLIST.LandOnGround))
                    if (Vector3.Distance(this.transform.position, MoveToPoint) > 0.5)
                        this.transform.position += (MoveToPoint - this.transform.position).normalized * movement.Speed;
                    else
                    {
                        this.transform.position = MoveToPoint;
                        return movement.IsMoving = false;

                    }
            }
        }
        else
        {

            //MoveToPosition = new Vector2(movement.WayPoints[0].x,movement.WayPoints[1].z);
            //if(UnitPosition == MoveToPosition)
            //    if (lastYps >= this.transform.position.y)
            //    {
            //        movement.WayPoints.Add(new Vector3(movement.WayPoints[0].z, movement.WayPoints[0].y, movement.WayPoints[0].x));
            //        movement.WayPoints.RemoveAt(0);
            //    }
            //lastYps = this.transform.position.y; 

            return true;
        }
        return false;
    }

    new public enum OPTIONS : int
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
            if (!standardOrder)
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

    internal override void FocussedLeftOnEnemy(GameObject enemy)
    {
        if (this.gameObject.GetComponent<Attackability>())
        {
            standardOrder = true;
            this.gameObject.GetComponent<Attackability>().attackState = Attackability.OPTIONS.Attack;
            UnitState = (this.GetComponent<Attackability>()) ? EnumProvider.ORDERSLIST.Attack : EnumProvider.ORDERSLIST.Cancel;
            Target = enemy;
            if (!UNIT.IsABuilding)
            {
                movement.SetKinematic();
                MoveToPoint = Target.transform.position;
                movement.IsMoving = true;
            }
            IsAttacking = true;
            //    GetComponent<Movability>().IsMoving = true;
            //     airUnitState = (OPTIONS)EnumProvider.ORDERSLIST.Attack;
            standardOrder = false;
        }
    }

    internal override void FocussedLeftOnGround(Vector3 worldPoint)
    {
        standardOrder = true;
        //   IsMovingAsGroup = true;
        movement.SetKinematic();

        movement.MoveToPoint = worldPoint;
        movement.MovingDirection = worldPoint;
        //     gameObject.transform.position += (Movement.MovingDirection * Movement.Speed);
        IsAttacking = false;
        Target = null;

        movement.IsMoving = true;
        UnitState = OPTIONS.MoveTo;
        standardOrder = false;
    }

    internal override void MoveAsGroup(GameObject leader)
    {

    }
}

