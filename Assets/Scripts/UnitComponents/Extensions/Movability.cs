using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movability : UnitExtension
{
    public override string IDstring
    {
        get { return "KingJulian"; }
    }
    public new enum OPTIONS : int
    {
        MoveTo = EnumProvider.ORDERSLIST.MoveTo,
        Patrol = EnumProvider.ORDERSLIST.Patrol,
        Guard = EnumProvider.ORDERSLIST.Guard,
        Hide = EnumProvider.ORDERSLIST.Hide,
        Stay = EnumProvider.ORDERSLIST.Stay
    }
    public OPTIONS movingUnitState = OPTIONS.Stay;
 
	void Start() 
    {

        this.PflongeOnUnit(System.Enum.GetValues(typeof(OPTIONS)));
        standardYPosition = this.gameObject.transform.position.y;
        MoveToPoint = this.gameObject.transform.position;
        WayPoints = new List<Vector3>();
        IsMoving = true;
        Speed = 1;
	}

    protected override EnumProvider.ORDERSLIST on_UnitStateChange(EnumProvider.ORDERSLIST stateorder)
    {
        Debug.Log("OnUnitStateChange called!!!!!!!!!!!!!!!");
        movingUnitState = (OPTIONS)stateorder;
        if (System.Enum.IsDefined(typeof(OPTIONS), (int)stateorder))
        {
            switch (movingUnitState)
            {
                case OPTIONS.MoveTo:
                    UNIT.Options.LockOnFocus();
                    SetKinematic();
                    WayPoints.Clear();
                    return stateorder;
                case OPTIONS.Patrol:
                    SetKinematic();
                    UNIT.Options.LockOnFocus();
                    return stateorder;
                case OPTIONS.Guard:
                    SetKinematic();
                    UNIT.Options.LockOnFocus();
                    WayPoints.Clear();
                    return stateorder;
                case OPTIONS.Hide:
                    WayPoints.Clear();
                    SetKinematic();
                    //todo:-----------------
                    return stateorder;
                case OPTIONS.Stay:
                    SetKinematic();
                    StayOrder();
                    return stateorder;
            }
        }
        return stateorder;
    }

    public void StayOrder()
    {
        SetKinematic();
        WayPoints.Clear();
        MoveToPoint = gameObject.transform.position;
        MovingDirection = MoveToPoint;
        movingUnitState = OPTIONS.Stay;
    }

    public float standardYPosition=0f;

    internal override void OptionExtensions_OnLEFTCLICK(bool hold)
    {
        if (movingUnitState == OPTIONS.MoveTo)
        {
            MoveToPoint = MouseEvents.State.Position.AsWorldPointOnMap;
            MovingDirection = MoveToPoint;
            UNIT.Options.UnlockFocus();
            SetKinematic();
            IsMoving = true;
        }
        else if (movingUnitState == OPTIONS.Guard)
        {
            if (UNIT.IsAllied(MouseEvents.State.Position.AsUnitUnderCursor.gameObject))
            {
                Target = MouseEvents.State.Position.AsUnitUnderCursor.SetInteracting(this.gameObject);
                MoveToPoint = Target.transform.position;
                IsMoving = true;
            }

            UNIT.Options.DestroyFocus();
        }
        else if (movingUnitState == OPTIONS.Patrol)
        {
            WayPoints.Add(MouseEvents.State.Position.AsWorldPointOnMap);
            IsMoving = true;


            UNIT.Options.DestroyFocus();
            Debug.Log("Units Movabilitys LeftclickHandler executed");
        }
    }

    internal override void OptionExtensions_OnRIGHTCLICK(bool hold)
    {
        Debug.Log("Movebility rightclick");
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
                //if (this.gameObject.GetComponent<Pilot>())
                //    Component.Destroy(gameObject.GetComponent<Pilot>());
                Throttle = 0;
            }
            else if (!__moving)
            {
                Throttle = 1;
       //         gameObject.AddComponent<Pilot>();
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
    public bool IsGroupLeader=false;

    [SerializeField]
    private float speed=0.2f;
    public float Throttle;

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


    public Vector3 MoveToPoint
    {
        get { return UNIT.Options.MoveToPoint; }
        set 
        {
            //if (UNIT.IsAnAirUnit)
            //    value.y = this.transform.parent.transform.position.y;
        //    else
                value.y = standardYPosition;

            UNIT.Options.MoveToPoint = value;
            
        }
    }
    public GameObject Target;
    public List<Vector3> WayPoints;

    private Vector3 movingDirection = Vector3.zero;
    public Vector3 Rudder = Vector3.zero;
    public bool RudderIsNormalizised = true;
    public Vector3 MovingDirection
    {
        get 
        {
            if (RudderIsNormalizised)
                return (movingDirection + Rudder).normalized;
            else
            {
                return movingDirection + Rudder;
                RudderIsNormalizised = true;
            }
        }
        set
        { 
            movingDirection = (value - this.gameObject.transform.position).normalized; 
        }
    }
    internal void SetKinematic()
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
    private int kinematicFrames=0;

    protected float distance=0f;
    public virtual float Distance
    {
        get
        {
            return Vector3.Distance(gameObject.transform.position, MoveToPoint);
        }
        internal set
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
         //   if (gameObject.GetComponent<Pilot>()) UnitComponent.Destroy(gameObject.GetComponent<Pilot>());
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
                MoveToPoint = WayPoints[0];
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
        checkKinematic();
        if (IsMoving) IsMoving = Move();
        Vector3 position = this.transform.position;
        position.y = standardYPosition;
    }
}
