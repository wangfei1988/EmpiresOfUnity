using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * AI to  improve the movement of MovingUnits
 */
[AddComponentMenu("Program-X/UNIT/AI - Pilot")]
public class Pilot : UnitComponent 
{
    private const float MIN_LOOKAHEAD = 3f;
    private const float MAX_LOOKAHEAD = 20f;
    private const float Accselerator = 1.0001f;
    private float? lah=0;
    private float LOOKAHEAD
    {
        get
        {
            if ((lah == null)) lah = Vector3.Distance(My.Options.MoveToPoint, My.gameObject.transform.position);
            if (lah > MAX_LOOKAHEAD) return MAX_LOOKAHEAD;
            else if (lah < MIN_LOOKAHEAD) return MIN_LOOKAHEAD;
            else return lah.Value;
        }
    }
	
    [SerializeField]
    private float lookAheadDistance=MIN_LOOKAHEAD;

    public float LookAheadDistance
    {
        get 
        {
            if (lookAheadDistance < MIN_LOOKAHEAD) return lookAheadDistance = MIN_LOOKAHEAD;
            else if (lookAheadDistance > LOOKAHEAD) return lookAheadDistance = LOOKAHEAD;
            return lookAheadDistance;
        }
        set
        {
            if (value != lookAheadDistance)
            {
                if (value < MIN_LOOKAHEAD) lookAheadDistance = MIN_LOOKAHEAD;
                else if (value > LOOKAHEAD) lookAheadDistance = LOOKAHEAD;
                else lookAheadDistance = value;
            }
        }
    }
    private SphereCollider mySpace;
    private UnitScript My;
    public Movability Controlls;
    private bool Triggerd
    {
        get
        {
            if (triggerd > 0) return true;
            else return false;
        }
        set
        {
            if (value && triggerd<100) triggerd++;
            else if (triggerd > 0) triggerd--;
            
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
            if (My.IsAnAirUnit) return true;
            else return isHeadin;
        }
        set
        {
            isHeadin = value;
        }
    }

    public float Throttle = 0f;
    public Vector3 Rudder = Vector3.zero;
    public bool IsAcselerating
    {
        get
        {
            if (Controlls.IsMoving)
                return Throttle < 1f;
            else return false;
        }
        set { if (value && !this.gameObject.GetComponent<Movability>().IsMoving)
            Controlls.IsMoving = true; }
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

    public override bool ComponentExtendsTheOptionalstateOrder
    {
        get
        {
            return false;
        }
    }

    void Awake()
    {
        Controlls = this.gameObject.GetComponent<Movability>();
        
        My = gameObject.GetComponent<UnitScript>();
        mySpace = this.gameObject.AddComponent<SphereCollider>();

    }
	void Start() 
    {

       triggerd = 0;
       IsAcselerating = true;
       mySpace.isTrigger = true;
       SetRadius(MIN_LOOKAHEAD);

       if (My.GetComponent<FaceDirection>()) IsAForwarder = My.GetComponent<FaceDirection>().faceMovingDirection;
       else IsAForwarder = false;
       
       this.PflongeOnUnit();
	}

    public override void DoUpdate()
    {
        if (IsAcselerating) IsAcselerating = ((Throttle += Accselerator)<1);
        if (IsHeadin) IsHeadin = WatchTarget();
        if (!Triggerd)
        {
            if (LookAheadDistance > LOOKAHEAD) SetRadius(LOOKAHEAD);
            else SetRadius(LookAheadDistance + 0.1f);
        }
        Triggerd = false;
        lah = null;
    }



    private void ShrinkRradius(float lookAhead)
    {
        if (lookAhead < LookAheadDistance)
        {
            SetRadius(lookAhead);
        }
    }
    private void SetRadius(float radius)
    {
        LookAheadDistance = radius;
        mySpace.radius = lookAheadDistance / My.gameObject.transform.localScale.x;
        if (My.IsAnAirUnit)
            (My.Options as FlyingUnitOptions).standardYPosition = mySpace.radius*2;
   //     if (IsAForwarder) mySpace.center = new Vector3(mySpace.center.x, mySpace.center.y, mySpace.radius - 0.5f);
    }



    
    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer != 2))// || (My.IsAnAirUnit))
        {
            if ((!other.isTrigger)
            && (other.gameObject.layer != 9)
            && (!My.InteractingUnits.Contains(other.gameObject.GetInstanceID())))
            {
                Controlls.Rudder += ((My.transform.position - other.transform.position).normalized / (mySpace.radius / 2));
            //    Controlls.MovingDirection.Normalize();
                Triggerd = true;
                ShrinkRradius(Vector3.Distance(other.ClosestPointOnBounds(gameObject.transform.position), gameObject.transform.position) * 0.95f);
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
                Controlls.Rudder += ((My.transform.position - other.gameObject.transform.position).normalized / (LookAheadDistance * 5));
                //      My.MovingDirection.Normalize();
                Triggerd = true;
                ShrinkRradius(Vector3.Distance(other.ClosestPointOnBounds(gameObject.transform.position), gameObject.transform.position));
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
        Vector3 targetDirection = ((this.Controlls.MoveToPoint - My.transform.position).normalized / (mySpace.radius));
        if (Vector3.Distance(Controlls.MovingDirection, targetDirection) > 0.0005f)
        {
            this.Controlls.Rudder = targetDirection;
            return true;
        }
        else
        {
            Controlls.MovingDirection = Controlls.MoveToPoint;
            Controlls.Rudder = Vector3.zero;
        }
            return false;
    }

    void OnDestroy()
    {
        mySpace = null;
        Component.Destroy(gameObject.GetComponent<SphereCollider>());

    //    UpdateManager.OnUpdate -= DoUpdate;
    }

    protected override EnumProvider.ORDERSLIST on_UnitStateChange(EnumProvider.ORDERSLIST stateorder)
    {
        return stateorder;
    }
}
