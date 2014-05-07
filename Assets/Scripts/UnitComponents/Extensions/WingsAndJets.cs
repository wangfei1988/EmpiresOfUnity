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
public class WingsAndJets : UnitComponent 
{
    new public enum OPTIONS : int
    {
        GlideFlight = EnumProvider.ORDERSLIST.GlideFlight,
        FullThrottle = EnumProvider.ORDERSLIST.FullThrottle,
        LandOnGround = EnumProvider.ORDERSLIST.LandOnGround
    }
    public OPTIONS flightState=OPTIONS.LandOnGround;



    public GroundUnitOptions Options;
    public Pilot pilot;
    public EnumProvider.DIRECTION Forward;
    [SerializeField]
    private float MAX_SPEED = 3f;
    [SerializeField]
    private float START_SPEED = 0.1f;
    [SerializeField]
    private float SPEED_FACTOR = 1.05f;
 //   private float speed=0.1f;
    public float Speed
    {
        get 
        {
            return Options.Speed ; 
        }
        set
        {
            if
              (value > MAX_SPEED) Options.Speed = MAX_SPEED;
            else Options.Speed = value;
        }
    }

    protected override EnumProvider.ORDERSLIST on_UnitStateChange(EnumProvider.ORDERSLIST stateorder)
    {

        switch ((OPTIONS)stateorder)
        {
            case OPTIONS.LandOnGround:
                flightState=OPTIONS.LandOnGround;
                IsAccselerating = false;
                Options.standardYPosition = 0.3f;
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

    public bool IsFlying
    {
        get 
        {
            if ((Options.IsMoving) && ((int)((EnumProvider.ORDERSLIST)Options.UnitState) < 100))
                return true;
            else return false;
        }
        set { Options.IsMoving = value; if ((value) && ((int)((EnumProvider.ORDERSLIST)Options.UnitState) >= 100))Options.UnitState = (OPTIONS)99; }
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
        ComponentExtendsTheOptionalstateOrder = true;
        System.Enum[] buffer = new System.Enum[3];
        buffer[0] = OPTIONS.GlideFlight;
        buffer[1] = OPTIONS.FullThrottle;
        buffer[2] = OPTIONS.LandOnGround;

        this.PflongeOnUnit(buffer);
    }

    void Start()
    {
        if (!this.gameObject.GetComponent<MovingUnitOptions>())
            Component.Destroy(this.GetComponent<WingsAndJets>());
        else
        {
            Options = this.gameObject.GetComponent<GroundUnitOptions>();
            if (!this.gameObject.GetComponent<Pilot>()) this.gameObject.AddComponent<Pilot>();
            pilot = this.gameObject.GetComponent<Pilot>();
            if (!this.GetComponent<FaceDirection>())
                this.gameObject.AddComponent<FaceDirection>();
            Forward = this.gameObject.GetComponent<FaceDirection>().forwardIs;
            Speed = START_SPEED;
        }
    }
    public float mische = 1;
    public bool Throttle()
    {
        if (IsAccselerating)
        {
            START_SPEED *= SPEED_FACTOR;
       //     Options.standardYPosition = Options.Speed * 10;

        }


        Vector3 direction = GetDirection(this.gameObject.GetComponent<FaceDirection>().forwardIs) * mische; ;

        switch (Forward)
        {
            case EnumProvider.DIRECTION.forward:
                this.gameObject.transform.position += (this.gameObject.transform.forward + direction).normalized * Options.Speed;
                break;
            case EnumProvider.DIRECTION.right:
                this.gameObject.transform.position += (this.gameObject.transform.right + direction).normalized * Options.Speed;
                break;
            case EnumProvider.DIRECTION.up:
                this.gameObject.transform.position += (this.gameObject.transform.up + direction).normalized * Options.Speed;
                break;
            case EnumProvider.DIRECTION.backward:
                this.gameObject.transform.position += (-this.gameObject.transform.forward+ direction).normalized * Options.Speed;
                break;
            case EnumProvider.DIRECTION.left:
                this.gameObject.transform.position += (-this.gameObject.transform.right+ direction).normalized * Options.Speed;
                break;
            case EnumProvider.DIRECTION.down:
                this.gameObject.transform.position += (-this.gameObject.transform.up + direction).normalized * Options.Speed;
                break;
        }

        return START_SPEED < MAX_SPEED;
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
        Speed = START_SPEED;
         IsAccselerating = Throttle();
    }


}
