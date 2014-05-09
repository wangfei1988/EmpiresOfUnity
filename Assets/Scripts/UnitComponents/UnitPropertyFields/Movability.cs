using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movability : UnitComponent
{
    
    public new enum OPTIONS : int
    {
        MoveTo = EnumProvider.ORDERSLIST.MoveTo,
        Patrol = EnumProvider.ORDERSLIST.Patrol,
        Guard = EnumProvider.ORDERSLIST.Guard,
        Hide = EnumProvider.ORDERSLIST.Hide,
        Seek = EnumProvider.ORDERSLIST.Seek,
        Stay = EnumProvider.ORDERSLIST.Stay
    }
    public OPTIONS movingUnitState = OPTIONS.Stay;
 
	void Start() 
    {
        this.ComponentExtendsTheOptionalstateOrder = true;
        this.PflongeOnUnit(System.Enum.GetValues(typeof(OPTIONS)));
        standardYPosition = this.gameObject.transform.position.y;
        MoveToPoint = this.gameObject.transform.position;
        WayPoints = new List<Vector3>();
	}

    protected override EnumProvider.ORDERSLIST on_UnitStateChange(EnumProvider.ORDERSLIST stateorder)
    {
        movingUnitState = (OPTIONS)stateorder;
        if (System.Enum.IsDefined(typeof(OPTIONS), stateorder))
        {
            switch (movingUnitState)
            {
                case OPTIONS.MoveTo:
                    UNIT.Options.LockOnFocus();
                    WayPoints.Clear();
                    MouseEvents.LEFTCLICK += MouseEvents_LEFTCLICK;
                    return stateorder;
                case OPTIONS.Patrol:
                    UNIT.Options.LockOnFocus();
                    MouseEvents.LEFTCLICK += MouseEvents_LEFTCLICK;
                    MouseEvents.RIGHTCLICK += MouseEvents_RIGHTCLICK;
                    return stateorder;
                case OPTIONS.Guard:
                    SetKinematic();
                    UNIT.Options.LockOnFocus();
                    WayPoints.Clear();
                    MouseEvents.LEFTCLICK += MouseEvents_LEFTCLICK;
                    return stateorder;
                case OPTIONS.Hide:
                    WayPoints.Clear();
                    //todo:-----------------
                    return stateorder;
                case OPTIONS.Seek:
                    WayPoints.Clear();
                    //todo:-----------------
                    return stateorder;
                case OPTIONS.Stay:
                    StayOrder();
                    return stateorder;
            }
        }
        return stateorder;
    }

    public void StayOrder()
    {
        SetKinematic();
        UNIT.Options.UnlockFocus(Focus.HANDLING.UnlockFocus);
        WayPoints.Clear();
        MoveToPoint = gameObject.transform.position;
        MovingDirection = MoveToPoint;
        movingUnitState = OPTIONS.Stay;
    }

    public float standardYPosition;

    void MouseEvents_LEFTCLICK(Ray qamRay, bool hold)
    {
        if (movingUnitState == OPTIONS.MoveTo)
        {
            MoveToPoint = MouseEvents.State.Position.AsWorldPointOnMap;
            MovingDirection = MoveToPoint;
            MouseEvents.LEFTCLICK -= MouseEvents_LEFTCLICK;
            UNIT.Options.UnlockFocus();
            SetKinematic();
            IsMoving = true;
        }
        else if (movingUnitState == OPTIONS.Guard)
        {
            if (UNIT.IsAllied(UnitUnderCursor.gameObject))
            {
                Target = UnitUnderCursor.UNIT.SetInteracting(this.gameObject);
                MoveToPoint = Target.transform.position;
                IsMoving = true;
            }
            MouseEvents.LEFTCLICK -= MouseEvents_LEFTCLICK;
            UNIT.Options.UnlockFocus(Focus.HANDLING.DestroyFocus);
        }
        else if (movingUnitState == OPTIONS.Patrol)
        {
            WayPoints.Add(MouseEvents.State.Position.AsWorldPointOnMap);
            IsMoving = true;
                MouseEvents.LEFTCLICK -= MouseEvents_LEFTCLICK;
                MouseEvents.RIGHTCLICK -= MouseEvents_RIGHTCLICK;

            UNIT.Options.UnlockFocus(Focus.HANDLING.DestroyFocus);
        }
    }

    void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold)
    {
        if ((!hold)
            && (gameObject.GetComponent<Focus>())
                && (movingUnitState == OPTIONS.Patrol))
        {
            if (WayPoints.Count >= 2) 
                WayPoints.Insert(WayPoints.Count - 1, MouseEvents.State.Position.AsWorldPointOnMap);
            else 
                WayPoints.Add(MouseEvents.State.Position.AsWorldPointOnMap);

            MovingDirection = MoveToPoint;
            IsMoving = true;
        }
    }

    private bool __moving = false;
    public virtual bool IsMoving
    {
        get
        {
            return __moving;
        }
        set
        {
            if (!value)
            {
                _groupmove = false;
                if (this.gameObject.GetComponent<Pilot>())
                    Component.Destroy(gameObject.GetComponent<Pilot>());
                Throttle = 0;
            }
            else if (!__moving)
            {
                Throttle = 1;
                gameObject.AddComponent<Pilot>();
            }
            __moving = value;
        }
    }




    void OnCollisionExit(Collision collision)
    {
        SetKinematic();
        MovingDirection = MoveToPoint;
    }
    public bool IsMovingAsGroup
    {
        get { return _groupmove; }
        set
        {
            if (value) IsMoving = true;
            _groupmove = value;
        }
    }
    private bool _groupmove = false;
    public bool IsGroupLeader;

    [SerializeField]
    private float speed;
    private float Throttle = 1f;
    public float Speed
    {
        get
        {
            if (IsMoving)
                return speed * Throttle;
            else return 0;
        }
        set
        {
            Throttle = Mathf.Clamp01(value);
            if (value > 0f) IsMoving = true;
        }
    }

    [SerializeField]
    private Vector3 _moveToPoint;
    public Vector3 MoveToPoint
    {
        get { return _moveToPoint; }
        set 
        {
            value.y = standardYPosition;
            _moveToPoint = value;
        }
    }
    public GameObject Target;
    public List<Vector3> WayPoints;

    private Vector3 movingDirection = Vector3.zero;
    public Vector3 Rudder = Vector3.zero;
    public Vector3 MovingDirection
    {
        get { return (movingDirection + Rudder).normalized; }
        set
        { 
            movingDirection = (value - this.gameObject.transform.position).normalized; 
        }
    }
    protected void SetKinematic()
    {
        gameObject.rigidbody.isKinematic = true;
        kinematicFrames = 2;
    }
    private void checkKinematic()
    {
        if (gameObject.rigidbody.isKinematic)
            if (kinematicFrames <= 0) gameObject.rigidbody.isKinematic = false;
            else --kinematicFrames;
    }
    private int kinematicFrames;

    protected float distance;
    public virtual float Distance
    {
        get
        {
            return Vector3.Distance(gameObject.transform.position, MoveToPoint);
        }
        protected set
        {
            if (value != distance)
            {
                if (IsMovingAsGroup)
                {
                    if (Target)
                    {
                        MoveToPoint = Target.transform.position;
                        gameObject.transform.position += (MovingDirection * Speed);
                    }
                    else IsGroupLeader = true;
                }
            }
        }
    }

    public void MoveAsGroup(GameObject leader)
    {
        Target = leader;
        MoveToPoint = leader.transform.position;
        IsMovingAsGroup = true;
        //     IsAttacking = false;
    }

    private bool Move()
    {
        checkKinematic();
        if (this.gameObject.GetComponent<Pilot>()) gameObject.GetComponent<Pilot>().DoUpdate();

        if (movingUnitState == OPTIONS.Guard)
        {
            if (gameObject.GetComponent<Pilot>()) UnitComponent.Destroy(gameObject.GetComponent<Pilot>());
            MoveToPoint = Target.transform.position;

            if (Distance >= 20) gameObject.transform.position += (MovingDirection * Speed);
            else if (Distance <= 15) gameObject.transform.position -= (MovingDirection * Speed);
        }
        else if (IsMovingAsGroup)
        {
            if (IsGroupLeader) IsMovingAsGroup = false;
            else Distance = Vector3.Distance(gameObject.transform.position, MoveToPoint);
        }
        else if (Distance >= 0.5)
        {
            this.gameObject.transform.position += MovingDirection * Speed;
        }
        else
        {
            SetKinematic();
            gameObject.transform.position = MoveToPoint;

            if (IsGroupLeader) GUIScript.main.SelectedGroup.GroupState = UnitGroup.GROUPSTATE.Waiting;
            if (movingUnitState == OPTIONS.Patrol)
            {
                WayPoints.RemoveAt(0);
                WayPoints.Add(gameObject.transform.position);
                MoveToPoint = WayPoints[0];
                MovingDirection=MoveToPoint;
            }
            else { StayOrder(); }
        }
        return this.gameObject.transform.position != MoveToPoint;
    }

    public override void DoUpdate()
    {
        
        if (IsMoving) IsMoving = Move();
    }
}
