using UnityEngine;
using System.Collections;

[AddComponentMenu("Program-X/Weapons/Amunition/Rockets/Lange Dünne")]
public class LargeRocketObject : RocketObject {

    private string HitInfo = "";
    public ParticleSystem emission;
    public override float MAX_RANGE
    {
        get { return 1000f; }
    }
    public Vector3 Yrotary = new Vector3(0f, 1f, 0f);
    public float step, range;
    private float Step
    { get 
    {
        if (plusminus) return step / 1000f;
        else return -step / 1000f;
    }
    }
    private bool plusminus = true;
    public float rotation;
    public Vector3 movingDirection,lastMovedDirection;
    [SerializeField]
    private float speed;
    public float Speed
    {
        get { return speed; }
        set
        {
            if (value > MAXIMUM_SPEED) speed = MAXIMUM_SPEED;
            else speed = value;
        } 
    }
    public float SPEED_FACTOR;
    float HalfDistance;
    public Renderer[] flights = new Renderer[3];
    [SerializeField]
    private bool visible = false;
    public bool Visible
    {
        get { return gameObject.collider.enabled = gameObject.renderer.enabled = visible; }
        set
        {
            //if (visible != value)
            //{
                
                foreach(Renderer visibility in flights) visibility.enabled = value;
                gameObject.renderer.enabled = gameObject.collider.enabled = visible = value;
         //   }
        }
    }
    public float MAXIMUM_SPEED;

    [SerializeField]
    private bool launch = false;
    private bool launched = false;

    public override bool LaunchButton
    {
        get
        {
            if ((!launched)&&(launch))
            {
                Visible = true;
                gameObject.GetComponent<TimedObjectDestructorCS>().enabled = true;
                emission.Play();
                movingDirection = -this.gameObject.transform.up;
                lastMovedDirection = movingDirection;
                
                HalfDistance = Vector3.Distance(Target, this.gameObject.transform.position) / 2f;
                timer = 0f;
                launched = true;
            }
            return launch;
        }
        set
        {

            launch = value;

        }


    }

    public override WeaponObject.AMUNITONTYPE amunition
    {
        get
        {
            return WeaponObject.AMUNITONTYPE.Rocket;
        }
    }

    [SerializeField]
    private float timer,ticker;


    internal override void StartUp()
    {
        Visible = false;

        timer = ticker = 0f;
        UpdateManager.WEAPONUPDATES += UpdateManager_WEAPONUPDATES;
    }

    void UpdateManager_WEAPONUPDATES()
    {
        ticker = Time.deltaTime;
        timer += ticker;
        Rotatate();
        if (LaunchButton) Throttle();
   //     GUIScript.AddTextLine(HitInfo);
    }

    internal override void Engage()
    {
        LaunchButton = true;
    }

    private float LFO()
    {
        
        Yrotary.x += Step;
        if (Yrotary.x < -range) { plusminus = true;  Yrotary.x = -range; }
        if (Yrotary.x > range) { plusminus = false;  Yrotary.x = range; }
        return Mathf.Sin(Yrotary.x)*range;
    }

    private void Rotatate()
    {

        Yrotary.z= -LFO()*2;
        rotation = ((Yrotary.z*5) + 2f);
        if(gameObject) gameObject.transform.Rotate(Yrotary, rotation);

   //     emission.transform.Rotate(-Yrotary, rotation);
    }

    private void Throttle()
    {
        float distance = Vector3.Distance(Target, this.gameObject.transform.position);



        if (distance > HalfDistance && timer < 4)
        {
            Speed *= SPEED_FACTOR;
            movingDirection = (movingDirection + Aim(Target) /100).normalized;
             // Vector3.Distance(Target, gameObject.transform.position);

        }
        else
        {
            movingDirection = (movingDirection + (Aim(Target) / ((distance / 2) / HalfDistance))).normalized;
       //     movingDirection = (Aim() + lastMovedDirection)/2f;
       //     lastMovedDirection = movingDirection;
        }
        lastMovedDirection = movingDirection;
            this.gameObject.transform.up = -movingDirection;
            this.gameObject.transform.position += (movingDirection * Speed);
            
            GUIScript.AddTextLine(HitInfo);
    }

    public override Vector3 Aim(Vector3 targetPosition)
    {

        return (targetPosition - this.gameObject.transform.position).normalized; // / Vector3.Distance(Target, this.gameObject.transform.position)).normalized;
       
    }


    void OnDestroy()
    {
        UpdateManager.WEAPONUPDATES -= UpdateManager_WEAPONUPDATES;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            if (!(other.gameObject.layer != (int)EnumProvider.LAYERNAMES.Ignore_Raycast))
            {
                if (other.gameObject.GetComponent<UnitScript>().GoodOrEvil != this.GoodOrEvil)
                {
                    HitInfo = "RocketHit at: " + other.gameObject.name + " " + other.gameObject.GetInstanceID();
                    other.gameObject.GetComponent<UnitScript>().Hit(1000);
                    gameObject.GetComponent<TimedObjectDestructorCS>().DestroyImmideate();
                }
            }
            else
            {//-------------------------------------------Groundhit!
                GameObject.Destroy(this.gameObject);
            }
        }
    }







}
