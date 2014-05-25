/// <summary> WingsAndJets
/// 
/// "WingsAndJets" can be addet to any MovingUnit
/// to make it possible to "Fly" for it.
/// 
/// 
/// </summary>


using UnityEngine;
using System.Collections;

[AddComponentMenu("Program-X/UNIT/WingsAndJets Extension")]
public class WingsAndJets : UnitExtension
{
    public override string IDstring
    {
        get { return "WingsAndJets"; }
    }
    new public enum OPTIONS : int
    {
        GlideFlight = EnumProvider.ORDERSLIST.GlideFlight,
        FullThrottle = EnumProvider.ORDERSLIST.FullThrottle,
        LandOnGround = EnumProvider.ORDERSLIST.LandOnGround
    }
    public OPTIONS flightState=OPTIONS.LandOnGround;


    public Movability Movement;
    public AirUnitOptions Options;
    public Pilot pilot;
    [SerializeField]
    private EnumProvider.DIRECTION face;
    public EnumProvider.DIRECTION Face
    {
        get
        {
            return this.gameObject.GetComponent<FaceDirection>().forwardIs = face;
        }
        set
        {
                this.gameObject.GetComponent<FaceDirection>().forwardIs = face = value;
        }
    }
    public EnumProvider.DIRECTION Forward;
    [SerializeField]
    private float MAX_SPEED = 3f;
    [SerializeField]
    private float JET_SPEED = 0.1f;
    [SerializeField]
    private float SPEED_FACTOR = 1.05f;
 //   private float speed=0.1f;
    public float Speed
    {
        get 
        {
            return Options.movement.Speed; 
        }
        set
        {
            if
              (value > MAX_SPEED) Options.movement.Speed = MAX_SPEED;
            else Options.movement.Speed = value;
        }
    }

    protected override EnumProvider.ORDERSLIST on_UnitStateChange(EnumProvider.ORDERSLIST stateorder)
    {

        switch ((OPTIONS)stateorder)
        {
            case OPTIONS.LandOnGround:
                flightState=OPTIONS.LandOnGround;
                IsAccselerating = false;
                Options.movement.standardYPosition = 0.3f;
                SPEED_FACTOR = 0.95f;
                stateorder = EnumProvider.ORDERSLIST.Stay;
                break;
            case OPTIONS.FullThrottle:
                flightState=OPTIONS.FullThrottle;
                SPEED_FACTOR = 1.05f;
                IsAccselerating = true;
                this.gameObject.transform.Rotate(this.gameObject.transform.forward, 30);
                break;
            case OPTIONS.GlideFlight:
                SPEED_FACTOR = 1.001f;
                flightState=OPTIONS.GlideFlight;
                isAccselerating = false;
                break;
        }

            
        return stateorder;
    }

    internal override void OptionExtensions_OnLEFTCLICK(bool hold)
    {

    }
    internal override void OptionExtensions_OnRIGHTCLICK(bool hold)
    {

    }

    public bool IsFlying
    {
        get 
        {
            if ((Options.movement.IsMoving) && ((int)((EnumProvider.ORDERSLIST)Options.UnitState) < 100))
                return true;
            else return false;
        }
        set { Options.movement.IsMoving = value; if ((value) && ((int)((EnumProvider.ORDERSLIST)Options.UnitState) >= 100))Options.UnitState = (OPTIONS)99; }
    }

    private bool isAccselerating;
    public bool IsAccselerating
    {
        get
        {
            if (flightState != OPTIONS.LandOnGround)
            {
                return isAccselerating = Speed < MAX_SPEED;
            }
            else return isAccselerating = false;
        }
        set
        {
            if (isAccselerating != value)
            {
                if (value) IsFlying = true;
                else MAX_SPEED = Speed;
            }
        }
    }

    void Awake()
    {

        System.Enum[] buffer = new System.Enum[3];
        buffer[0] = OPTIONS.GlideFlight;
        buffer[1] = OPTIONS.FullThrottle;
        buffer[2] = OPTIONS.LandOnGround;

        this.PflongeOnUnit(buffer);
    }

    void Start()
    {
      //  if (!this.gameObject.GetComponent<MovingUnitOptions>())
      //      Component.Destroy(this.GetComponent<WingsAndJets>());
   
            Options = this.gameObject.GetComponent<AirUnitOptions>();
            //if (!this.gameObject.GetComponent<Pilot>()) this.gameObject.AddComponent<Pilot>();
            pilot = this.gameObject.GetComponent<Pilot>();
            this.GetComponent<FaceDirection>();
            
            Face = EnumProvider.DIRECTION.forward;
            Forward = this.gameObject.GetComponent<FaceDirection>().forwardIs;
            Speed = JET_SPEED;
            lastA = Forward;
            lastB = Face;
            timerA = timerB = 0f;
            this.PflongeOnUnit(System.Enum.GetValues(typeof(OPTIONS)));
            lerpTime = 2.5f;
      
    }
    public float mische = 1;
    private float Mische
    {
        get { return mische / 1000; }
        set { mische = value * 1000; }
    }

    public float lerpTime=2.5f;
    public bool fading
    {
        get { return (timerA / lerpTime < 1f)||(timerB/lerpTime < 1f); }
    }
    public float timerA,timerB;
    public Vector3 A, B;
    EnumProvider.DIRECTION lastA, lastB;
    public float Speed_Height_Relation = 10;

    public Vector3 fade(EnumProvider.DIRECTION a,bool rotationFade)
    {
        if (rotationFade)
        {
            if (lastB != a)
            {
                timerB = 0f;
                lastB = a;
            }
            if (fading)
            {
                timerB += Time.deltaTime;
                B = GetDirection(a);
                return Vector3.Slerp(GetDirection(lastB), B, Mathf.Clamp01(timerB / lerpTime));
            }
            else return B;
        }
        else
        {
            if (lastA != a)
            {
                timerA = 0f;
                lastA = a;
            }
            if (fading)
            {
                timerA += Time.deltaTime;
                A = GetDirection(a);
                return Vector3.Lerp(GetDirection(lastA), GetDirection(a), Mathf.Clamp01(timerA / lerpTime));
            }
            else
                return A;
        }
    }

    public bool Throttle()
    {
        if (IsAccselerating)
        {
            JET_SPEED *= SPEED_FACTOR;
       //     Options.standardYPosition = Options.Speed * 10;

        }

       
    //    Vector3 direction = GetDirection(this.gameObject.GetComponent<FaceDirection>().forwardIs) * mische; ;

    this.gameObject.transform.position += (fade(Forward,false)  +  ( fade(Face,true) * Mische) ).normalized * JET_SPEED;
        //switch (Forward)
        //{
        //    case EnumProvider.DIRECTION.forward:
        //        this.gameObject.transform.position += (this.gameObject.transform.forward + direction).normalized * Options.Speed;
        //        break;
        //    case EnumProvider.DIRECTION.right:
        //        this.gameObject.transform.position += (this.gameObject.transform.right + direction).normalized * Options.Speed;
        //        break;
        //    case EnumProvider.DIRECTION.up:
        //        this.gameObject.transform.position += (this.gameObject.transform.up + direction).normalized * Options.Speed;
        //        break;
        //    case EnumProvider.DIRECTION.backward:
        //        this.gameObject.transform.position += (-this.gameObject.transform.forward+ direction).normalized * Options.Speed;
        //        break;
        //    case EnumProvider.DIRECTION.left:
        //        this.gameObject.transform.position += (-this.gameObject.transform.right+ direction).normalized * Options.Speed;
        //        break;
        //    case EnumProvider.DIRECTION.down:
        //        this.gameObject.transform.position += (-this.gameObject.transform.up + direction).normalized * Options.Speed;
        //        break;
        //}

        return JET_SPEED < MAX_SPEED;
    }

    public Vector3 GetDirection(EnumProvider.DIRECTION direct)
    {
        switch (direct)
        {
            case EnumProvider.DIRECTION.forward:
                return this.gameObject.transform.forward;
            case EnumProvider.DIRECTION.right:
                return this.gameObject.transform.right;
            case EnumProvider.DIRECTION.up:
                return this.gameObject.transform.up;
            case EnumProvider.DIRECTION.backward:
                return -this.gameObject.transform.forward;
            case EnumProvider.DIRECTION.left:
                return -this.gameObject.transform.right;
            case EnumProvider.DIRECTION.down:
                return -this.gameObject.transform.up;
        }
        return Vector3.zero;
    }

    public override void DoUpdate()
    {
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, (Speed > 0) ? (Speed * Speed_Height_Relation) : (-Speed * Speed_Height_Relation), this.gameObject.transform.position.z);
        Speed = JET_SPEED;
         IsAccselerating = Throttle();
    }


}
