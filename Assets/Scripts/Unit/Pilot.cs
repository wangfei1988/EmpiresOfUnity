using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pilot : UnitComponent 
{
    private const float MIN_LOOKAHEAD = 0.5f;
    private float MAX_LOOKAHEAD
    { get {return Vector3.Distance(My.Options.MoveToPoint,My.transform.position); } }
    [SerializeField]
    private float lookAheadDistance;
    public float LookAheadDistance
    {
        get 
        {
            if (lookAheadDistance < MIN_LOOKAHEAD) return lookAheadDistance = MIN_LOOKAHEAD;
            else if (lookAheadDistance > MAX_LOOKAHEAD) return lookAheadDistance = MAX_LOOKAHEAD;
            return lookAheadDistance;
        }
        set
        {
            if (value != lookAheadDistance)
            {
                if (value < MIN_LOOKAHEAD) lookAheadDistance = MIN_LOOKAHEAD;
                else if (value > MAX_LOOKAHEAD) lookAheadDistance = MAX_LOOKAHEAD;
                else lookAheadDistance = value;
            }
        }
    }
    private SphereCollider mySpace;
    private UnitScript My;
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
    private bool IsMovingForward
    { get { return My.transform.forward == My.Options.movingDirection; } }
    private bool? isAForwarder;
    public bool IsAForwarder
    {
        get 
        { 
            if (isAForwarder == null) 
            {
                if (My.GetComponent<FaceDirection>()) isAForwarder = My.GetComponent<FaceDirection>().faceMovingDirection;
                else isAForwarder = false;
            }
            return isAForwarder.Value;
        }
    }

	void Start() 
    {
       triggerd = 0;
       My = gameObject.GetComponent<UnitScript>();
       mySpace = this.gameObject.AddComponent<SphereCollider>();
       mySpace.isTrigger = true;
       SetRadius(MIN_LOOKAHEAD);
       UpdateHandler.OnUpdate += DoUpdate;
	}
    internal void DoUpdate()
    {
        if (Triggerd == false)
        {
            if (LookAheadDistance < MAX_LOOKAHEAD)
                SetRadius(LookAheadDistance + 1f);
            else
                SetRadius(MAX_LOOKAHEAD);
            Triggerd = false;
        }
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
        mySpace.radius = lookAheadDistance / gameObject.transform.localScale.x;
        if (IsAForwarder) mySpace.center = new Vector3(mySpace.center.x, mySpace.center.y, mySpace.radius - 0.5f);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer != 2)&&(!My.InteractingUnits.Contains(other.gameObject.GetInstanceID())))
        {
            GUIScript.AddTextLine(this.gameObject.name + " " + this.gameObject.GetInstanceID() + " TriggerEnter-> " + other.gameObject.name + " " + other.gameObject.GetInstanceID());
            My.Options.movingDirection += ((My.transform.position - other.transform.position).normalized / mySpace.radius);
            My.Options.movingDirection.Normalize();
            Triggerd = true;
            ShrinkRradius(Vector3.Distance(other.transform.position, gameObject.transform.position)*0.8f);
        }
    }
    void OnTriggerStay(Collider other)
    {
        
        if ((other.gameObject.layer != 2)&&(!My.InteractingUnits.Contains(other.gameObject.GetInstanceID())))
        {
            GUIScript.AddTextLine(this.gameObject.name+" "+this.gameObject.GetInstanceID()+" TriggerStay-> "+other.gameObject.name+" "+other.gameObject.GetInstanceID());

      //      float factor = (lookAheadDistance - Vector3.Distance(My.transform.position, other.gameObject.transform.position)) / lookAheadDistance;
        //    My.Options.movingDirection += ((My.transform.position - other.transform.position).normalized / Vector3.Distance(My.transform.position+(other.transform.position - My.transform.position).normalized*3,other.transform.position));
            My.Options.movingDirection += ((My.transform.position - other.gameObject.transform.position).normalized / LookAheadDistance);
      //      My.Options.movingDirection += originalDirection / 2;
    //             My.Options.movingDirection.Normalize();
            Triggerd = true;
            ShrinkRradius(Vector3.Distance(other.transform.position, gameObject.transform.position));
                 
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != 2)
        {
            GUIScript.AddTextLine(this.gameObject.name + " " + this.gameObject.GetInstanceID() + " TriggerExit-> " + other.gameObject.name + " " + other.gameObject.GetInstanceID());
            My.Options.movingDirection = (My.Options.MoveToPoint - My.transform.position).normalized;
        }
     
    }



    void OnDestroy()
    {
        mySpace = null;
        Component.Destroy(gameObject.GetComponent<SphereCollider>());
    }
}
