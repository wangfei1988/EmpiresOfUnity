using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * AI to  improve the movement of MovingUnits
 */
[AddComponentMenu("Program-X/UNIT/AI - Pilot")]
public class Pilot : UnitComponent
{
    public override string IDstring
    {
        get { return "Pilot"; }
    }
    private const float MIN_LOOKAHEAD = 4f;
    private float Min_LookAhead
    {
        get { return MIN_LOOKAHEAD * Throttle; }
    }
    private const float MAX_LOOKAHEAD = 20f;
    private float Accselerator = 0.01f;
    public bool IsSlowingDown = false;
    public float slopeDistance;
    private float LOOKAHEAD
    {
        get
        {
            if (Controlls.Distance > MAX_LOOKAHEAD)
                return MAX_LOOKAHEAD;
            else
            {


                if (Controlls.Distance < Min_LookAhead)
                    return Min_LookAhead;

            }

            return Controlls.Distance;


        }
    }

    public bool IsPermanent = false;

    [SerializeField]
    private float lookAheadDistance = 4;

    public float LookAheadDistance
    {
        get
        {
            if (lookAheadDistance < Min_LookAhead)
                return lookAheadDistance = Min_LookAhead;
            else if (lookAheadDistance > LOOKAHEAD)
                return lookAheadDistance = LOOKAHEAD;
            return lookAheadDistance;
        }
        set
        {
            if (value != lookAheadDistance)
            {
                if (value < Min_LookAhead)
                    lookAheadDistance = Min_LookAhead;
                else if (value > LOOKAHEAD)
                    lookAheadDistance = LOOKAHEAD;
                else
                    lookAheadDistance = value;
            }
        }
    }
    public SphereCollider mySpace;
    private bool _gunnercontrolled = false;
    public bool PerceptionIsGunnerControlled
    {
        get
        {
            return _gunnercontrolled;
        }
        set
        {
            if (value)
            {
                IsAcselerating = false;
            }
            _gunnercontrolled = value;
        }
    }
    private UnitScript My;
    public Movability Controlls;
    private bool Triggerd
    {
        get
        {
            if (triggerd > 0)
                return true;
            else
                return false;
        }
        set
        {
            if (value && triggerd < 100)
                triggerd++;
            else if (triggerd > 0)
                triggerd--;

        }
    }
    [SerializeField]
    private int triggerd;

    public bool IsAForwarder;
    private bool isHeadin = false;
    public bool IsHeadin
    {
        get
        {
            if (My.IsAnAirUnit)
                return true;
            else
                return isHeadin;
        }
        set
        {
            isHeadin = value;
        }
    }

    private float _throttle = 0;
    public float Throttle
    {
        get
        {
            if (!Controlls.IsMoving)
            {
                IsSlowingDown = false;
                return _throttle = 0;

            }
            else
                return _throttle;
        }
        set { Controlls.Speed = _throttle = value; }
    }
    public Vector3 Rudder = Vector3.zero;
    public bool IsAcselerating
    {
        get
        {
            if (My.Options.IsAttacking)
                return true;

            if ((Controlls.IsMoving)&&(!IsSlowingDown))
                return Throttle < 1f;
            else
                return false;
        }
        set
        {
            if (value)
            {
                IsSlowingDown = false;
                if (!Controlls.IsMoving)
                    Controlls.IsMoving = true;
            }
        }
    }

    private float? MAXIMUM_SPEED = null;
    public float MAX_SPEED
    {
        get
        {
            if (MAXIMUM_SPEED == null)
            {
                float buffer = Throttle;
                Throttle = 1f;
                MAXIMUM_SPEED = Controlls.Speed;
                Throttle = buffer;
            }
            return MAXIMUM_SPEED.Value;
        }
    }
    private bool ok=false;

    /* Start & Awake & Update */
    void Awake()
    {

        Controlls = this.gameObject.GetComponent<Movability>();
        My = gameObject.GetComponent<UnitScript>();

        if (!mySpace)
            if ((!this.gameObject.GetComponent<SphereCollider>())&&(!this.gameObject.GetComponentInChildren<SphereCollider>()))
                this.gameObject.AddComponent<SphereCollider>().isTrigger = true;



    }
    void Start()
    {
        if (this.gameObject.GetComponent<SphereCollider>())
            mySpace = this.gameObject.GetComponent<SphereCollider>();
        else
        {
            foreach (GameObject SubUnit in My.Options.ColliderContainingChildObjects)
                if (SubUnit.GetComponent<SphereCollider>())
                {
                    mySpace = SubUnit.GetComponent<SphereCollider>();
                    break;
                }
        }
        triggerd = 0;
        IsAcselerating = true;
        IsSlowingDown = false;
        mySpace.isTrigger = true;
        SetRadius(Min_LookAhead);

        if (My.GetComponent<FaceDirection>())
            IsAForwarder = My.GetComponent<FaceDirection>().faceMovingDirection;
        else
            IsAForwarder = false;


        Throttle = 0;

        this.PflongeOnUnit();
        if (mySpace!=null)
            ok=true;
    }

    private bool SlowDown(float distance)
    {
        if (!IsSlowingDown)
        {
            slopeDistance = distance;
            IsAcselerating = false;
        }
        Throttle = (distance / slopeDistance);
        return Throttle > 0f;
    }

    public float Distance;

    public override void DoUpdate()
    {
        if (ok)
        {
            Distance = Controlls.Distance;
            //  Distance -= this.transform.position.y;
            //if (My.IsAnAirUnit)
            //    Distance -= 5;
            //    Controlls.standardYPosition = this.transform.parent.position.y;
            if (IsAcselerating)
                IsAcselerating = ((Throttle += Accselerator) < 1);



            if (IsSlowingDown)
                IsSlowingDown = SlowDown(Distance);
            else if ((Distance < mySpace.radius) && (Distance < Controlls.Speed * 15))
                IsSlowingDown = SlowDown(Distance);

            if (IsHeadin)
                IsHeadin = WatchTarget();
            if (!Triggerd)
            {
                if (LookAheadDistance > LOOKAHEAD)
                    SetRadius(LOOKAHEAD);
                else
                    SetRadius(LookAheadDistance + 0.01f);
            }
            Triggerd = false;
        }
    }

    private void ShrinkRadius(float lookAhead)
    {
        if (!PerceptionIsGunnerControlled)
        {
            if (lookAhead < LookAheadDistance)
            {
                SetRadius(lookAhead);
                //      Throttle -= 2*Accselerator;
            }
        }
    }
    private void SetRadius(float radius)
    {
        if (PerceptionIsGunnerControlled)
        {
            if (radius > My.weapon.GetMaximumRange())
            {
                LookAheadDistance = radius;
                mySpace.radius = lookAheadDistance / My.gameObject.transform.localScale.x;
                if (My.IsAnAirUnit)
                    Controlls.standardYPosition = mySpace.radius * 2;
                //  if (IsAForwarder) mySpace.center = new Vector3(mySpace.center.x, mySpace.center.y, mySpace.radius - 0.5f);
            }
        }
        else
        {
            LookAheadDistance = radius;
            mySpace.radius = lookAheadDistance / this.gameObject.transform.localScale.x;

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer != 2))// || (My.IsAnAirUnit))
        {
            if (!other.isTrigger)
            {
                if ((other.gameObject.layer != 9)
                && (!My.InteractingUnits.Contains(other.gameObject.GetInstanceID())))
                {
                    if (mySpace)
                        Controlls.Rudder += ((My.transform.position - other.transform.position).normalized / (mySpace.radius * 3));
                    //    Controlls.MovingDirection.Normalize();
                    Triggerd = true;
                    ShrinkRadius(Vector3.Distance(other.ClosestPointOnBounds(gameObject.transform.position), gameObject.transform.position));// * 0.95f);
                }
            }
            else if (My.InteractingUnits.Contains(other.gameObject.GetInstanceID()))
            {
                triggerd=99;
            }

        }
    }

    void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.layer != 2))// || (My.IsAnAirUnit))
        {
            if ((!other.isTrigger)
            && (other.gameObject.layer != 9)
            && (!My.InteractingUnits.Contains(other.gameObject.GetInstanceID())))
            {
                Vector3 directionbuffer = (My.transform.position - other.gameObject.transform.position);
                directionbuffer=directionbuffer/100 + this.transform.forward;

                //   Throttle = (directionbuffer.normalized.magnitude - directionbuffer.magnitude);

                //  Controlls.Rudder += directionbuffer.normalized;
                Controlls.NormalizedRudder = false;
                Controlls.Rudder += ((My.transform.position - other.gameObject.transform.position).normalized / (LookAheadDistance *4));
                //    Controlls.Rudder.Normalize();
                //      (My.Options as MovingUnitOptions).MovingDirection.Normalize();
                Triggerd = true;
                ShrinkRadius(Vector3.Distance(other.ClosestPointOnBounds(gameObject.transform.position), gameObject.transform.position)* 0.95f);
                IsHeadin = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.layer != 2))// || (My.IsAnAirUnit))
        {
            if (other.gameObject.layer != 9)
                IsHeadin = true;
        }
    }

    private bool WatchTarget()
    {
        Vector3 targetDirection = ((this.Controlls.MoveToPoint - My.transform.position).normalized / (Vector3.Distance(My.transform.position, this.Controlls.MoveToPoint) / 3f));
        if (Vector3.Distance(Controlls.MovingDirection, targetDirection) > 0.8f)
        {
            this.Controlls.Rudder += (targetDirection);
            this.Controlls.Rudder.Normalize();
            return true;
        }
        else
        {
            Controlls.MovingDirection = Controlls.MoveToPoint;
            Controlls.Rudder = Vector3.zero;
        }

        return false;
    }

    //Destroys Pilot component if's not permanent...
    public bool GetOff()
    {
        if (!IsPermanent)
        {
            Component.Destroy(this.GetComponent<Pilot>());
            return true;
        }
        else
            return false;
    }

    void OnDestroy()
    {
        Controlls.Rudder = Vector3.zero;
        if (!this.gameObject.GetComponent<Gunner>())
            Component.Destroy(this.gameObject.GetComponent<SphereCollider>());
    }

    protected override EnumProvider.ORDERSLIST on_UnitStateChange(EnumProvider.ORDERSLIST stateorder)
    {
        return stateorder;
    }
}
