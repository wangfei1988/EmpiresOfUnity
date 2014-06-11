using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Program-X/UNIT/Extensions/Movability")]
public class Movability : UnitExtension
{

    public override string IDstring
    {
        get { return "KingJulian"; }
    }

    public enum OPTIONS : int
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
        this.PflongeOnUnit(typeof(OPTIONS));
        MoveToPoint = this.gameObject.transform.position;
        WayPoints = new List<Vector3>();
        standardYPosition=this.gameObject.transform.position.y;
        //   IsMoving = true;
        Speed = 1;
    }

    public Pilot pilot
    {
        get
        {
            return this.GetComponent<Pilot>();
        }
    }

    public void AddPilot()
    {
        if (!pilot)
            this.gameObject.AddComponent<Pilot>();
    }

    protected override EnumProvider.ORDERSLIST on_UnitStateChange(EnumProvider.ORDERSLIST stateorder)
    {
        //Debug.Log("OnUnitStateChange called!!!!!!!!!!!!!!!");
        movingUnitState = (OPTIONS)stateorder;
        if (!UNIT.Options.standardOrder)
        {
            if (System.Enum.IsDefined(typeof(OPTIONS), (int)stateorder))
            {
                switch (movingUnitState)
                {
                case OPTIONS.MoveTo:
                    SetKinematic();
                    WayPoints.Clear();
                    UNIT.InteractingUnits.Clear();
                    UNIT.Options.LockOnFocus();
                    return stateorder;
                case OPTIONS.Patrol:
                    SetKinematic();
                    UNIT.InteractingUnits.Clear();
                    UNIT.Options.LockOnFocus();
                    return stateorder;
                case OPTIONS.Guard:
                    SetKinematic();
                    WayPoints.Clear();
                    UNIT.InteractingUnits.Clear();
                    UNIT.Options.LockOnFocus();
                    return stateorder;
                case OPTIONS.Hide:
                    WayPoints.Clear();
                    SetKinematic();
                    //todo:-----------------
                    return stateorder;
                case OPTIONS.Stay:
                    SetKinematic();
                    WayPoints.Clear();
                    MoveToPoint = gameObject.transform.position;
                    MovingDirection = MoveToPoint;
                    UNIT.InteractingUnits.Clear();

                    return stateorder;
                }
            }
        }
        return stateorder;
    }

    public void StayOrder()
    {
        UNIT.Options.standardOrder=false;
        UNIT.Options.UnitState=OPTIONS.Stay;
    }

    public float standardYPosition=0.1f;
    public bool AlwaysKeepStandardYpsPosition = true;
    private void KeepStandardYpsPosition()
    {
        if (AlwaysKeepStandardYpsPosition)
        {
            Vector3 position = this.transform.position;
            position.y = standardYPosition;
            this.transform.position = position;
        }
    }

    internal override void OptionExtensions_OnLEFTCLICK(bool hold)
    {
        if (!hold)
        {
            if (movingUnitState == OPTIONS.MoveTo)
            {
                MoveToPoint = MouseEvents.State.Position.AsWorldPointOnMap;
                MovingDirection = MoveToPoint;
                SetKinematic();
                IsMoving = true;

                UNIT.Options.UnlockFocus();
            }
            else if (movingUnitState == OPTIONS.Guard)
            {
                if (UNIT.IsAllied(MouseEvents.State.Position.AsUnitUnderCursor.gameObject))
                {
                    Target = MouseEvents.State.Position.AsUnitUnderCursor.SetInteracting(this.gameObject);
                    MoveToPoint = Target.transform.position;
                    IsGuarding=true;
                    UNIT.Options.UnlockAndDestroyFocus();
                }
            }
            else if (movingUnitState == OPTIONS.Patrol)
            {
                WayPoints.Add(MouseEvents.State.Position.AsWorldPointOnMap);
                IsMoving = true;

                UNIT.Options.UnlockAndDestroyFocus();
            }
        }
    }

    internal override void OptionExtensions_OnRIGHTCLICK(bool hold)
    {
        if (!hold)
        {
            if (movingUnitState == OPTIONS.Patrol)
            {
                if (WayPoints.Count >= 2)
                    WayPoints.Insert(WayPoints.Count - 1, MouseEvents.State.Position.AsWorldPointOnMap);
                else
                    WayPoints.Add(MouseEvents.State.Position.AsWorldPointOnMap);

                MovingDirection = MoveToPoint;
                IsMoving = true;
            }
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
                Throttle = 0;
                if (__moving)
                    pilot.GetOff();
            }
            else if (!__moving)
            {
                Throttle = 1;
                AddPilot();
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
            if (value)
                IsMoving = true;
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
            return 0;
        }
        set
        {
            Throttle = Mathf.Clamp01(value);
            if (value > 0f)
                IsMoving = true;
        }
    }

    public Vector3 MoveToPoint
    {
        get { return UNIT.Options.MoveToPoint; }
        set
        {
            //if (UNIT.IsAnAirUnit)
            //    value.y = this.transform.parent.transform.position.y;
            //else
            value.y = standardYPosition;

            UNIT.Options.MoveToPoint = value;
        }
    }
    public GameObject Target;
    public List<Vector3> WayPoints;

    private Vector3 movingDirection = Vector3.zero;
    public Vector3 Rudder = Vector3.zero;
    public bool NormalizedRudder = true;
    public Vector3 MovingDirection
    {
        get
        {
            if (pilot)
            {
                if (NormalizedRudder)
                    return (movingDirection + Rudder).normalized;
                else
                {
                    NormalizedRudder = true;
                    return movingDirection + Rudder;
                }
            }
            else
            {
                return movingDirection;
            }
        }
        set
        {
            movingDirection = (value - this.gameObject.transform.position).normalized;
        }
    }

    internal void SetKinematic()
    {
        if (UNIT.Options.ColliderContainingChildObjects.Length > 0)
        {
            foreach (GameObject subUnitPart in UNIT.Options.ColliderContainingChildObjects)
                if (subUnitPart.rigidbody)
                    subUnitPart.rigidbody.isKinematic = true;
        }

        if (this.gameObject.rigidbody)
            gameObject.rigidbody.isKinematic = true;

        kinematicFrames = 2;
    }

    private void checkKinematic()
    {
        _distance = null;
        if (UNIT.Options.ColliderContainingChildObjects.Length > 0)
        {
            bool ISKINEMATIC = false;
            foreach (GameObject subUnitPart in UNIT.Options.ColliderContainingChildObjects)
            {
                if ((subUnitPart.rigidbody)&&(subUnitPart.rigidbody.isKinematic))
                    ISKINEMATIC = true;
            }
            if (ISKINEMATIC)
            {
                if (kinematicFrames <= 0)
                {
                    foreach (GameObject subUnitPart in UNIT.Options.ColliderContainingChildObjects)
                        subUnitPart.rigidbody.isKinematic = false;
                }
            }
            else
            {
                if (kinematicFrames <= 0)
                    gameObject.rigidbody.isKinematic = false;
                --kinematicFrames;
            }
        }
        else if (gameObject.rigidbody.isKinematic)
        {
            if (kinematicFrames <= 0)
                gameObject.rigidbody.isKinematic = false;
            else
                --kinematicFrames;
        }
    }
    private int kinematicFrames=0;

    protected float? _distance = null;
    public virtual float Distance
    {
        get
        {

            return (_distance == null) ? (_distance = Vector3.Distance(gameObject.transform.position, MoveToPoint)).Value : _distance.Value;
        }
        internal set
        {
            if (value != _distance)
            {
                if (IsMovingAsGroup)
                {
                    if (Target)
                    {
                        MoveToPoint = Target.transform.position;
                        gameObject.transform.position += (MovingDirection * Speed);
                    }
                    else
                        IsGroupLeader = true;
                }
            }
        }
    }

    public void MoveAsGroup(GameObject leader)
    {
        Target = leader;
        MoveToPoint = leader.transform.position;
        IsMovingAsGroup = true;
    }
    private Vector3 attackDirection = Vector3.zero;
    private bool anflug = false;

    private bool _isGuarding=false;
    public bool IsGuarding
    {
        get
        {
            if (_isGuarding)
                return IsMoving = _isGuarding;
            return _isGuarding;
        }
        set
        {
            if (value)
                IsMoving = true;
            _isGuarding=value;
        }
    }
    private bool Move()
    {
        bool stopMoving=false;

        if (pilot)
            pilot.DoUpdate();

        if (IsGuarding)
        {
            MoveToPoint = Target.transform.position;

            if (Distance >= 20)
                gameObject.transform.position += (MovingDirection * Speed);
            else if (Distance <= 15)
                gameObject.transform.position -= (MovingDirection * Speed);
        }
        else if (IsMovingAsGroup)
        {
            if (IsGroupLeader)
                IsMovingAsGroup = false;
            else
                Distance = Vector3.Distance(gameObject.transform.position, MoveToPoint);
        }
        else if ((UNIT.Options.IsAttacking))
        {
            if (Distance >= (UNIT.AttackRange / 2))
            {
                this.gameObject.transform.position += MovingDirection * Speed;
                anflug = false;
            }
            else
            {
                if (UNIT.IsAnAirUnit)
                {
                    if (Distance<10)
                        if (!anflug)
                        {
                            attackDirection = MovingDirection * (Speed + 0.1f);
                            anflug = true;
                        }
                    this.gameObject.transform.position += attackDirection;

                }
            }


        }
        else if (Distance >= 0.75f)
        {
            this.gameObject.transform.position += (MovingDirection * Speed);
        }
        else
        {
            SetKinematic();
            gameObject.transform.position = MoveToPoint;
            stopMoving=true;
            StayOrder();

            //   UNIT.Options.FocussedLeftOnGround(gameObject.transform.position);
            //   

            if (IsGroupLeader)
                GUIScript.SelectedGroup.GroupState = UnitGroup.GROUPSTATE.Waiting;

            if (movingUnitState == OPTIONS.Patrol
            && WayPoints.Count>0)
            {
                MoveToPoint = WayPoints[0];
                WayPoints.RemoveAt(0);
                WayPoints.Add(gameObject.transform.position);
                MoveToPoint = WayPoints[0];
                MovingDirection = MoveToPoint;
                stopMoving=false;
            }
            //  else { StayOrder(); }
        }

        KeepStandardYpsPosition();

        return !stopMoving;

    }

    public override void DoUpdate()
    {
        checkKinematic();
        if (IsMoving)
            IsMoving = Move();
        KeepStandardYpsPosition();
    }

}
