using UnityEngine;
using System.Collections;

/*
public class LargeRocketObject : Rocket {

    public ParticleSystem emission;
    public string HitInfo = "";

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
        get { return visible = gameObject.collider.enabled = gameObject.renderer.enabled; }
        set
        {
            if (visible != value)
            {
                gameObject.GetComponent<TimedObjectDestructorCS>().enabled = true;
                foreach(Renderer visibility in flights) visibility.enabled = value;
                gameObject.renderer.enabled = gameObject.collider.enabled = visible = value;
            }
        }
    }
    public float MAXIMUM_SPEED;
    public bool launch;

    public override bool LaunchButton
    {
        get
        {
            if (launch)
            {
                Visible = true;

                emission.Play();
                movingDirection = -this.gameObject.transform.up;
                lastMovedDirection = movingDirection;

                HalfDistance = Vector3.Distance(Target, this.gameObject.transform.position) / 2f;
                timer = 0f;
            }
            return launch;
        }
        set
        {

            launch = value;

        }


    }

    
    [SerializeField]
    private int counter;
    [SerializeField]
    private float timer,ticker;


    internal override void StartUp()
    {
        Visible = false;
        counter = 0;
        timer = ticker = 0f;
        amunition = AMUNITON.Rocket;
        UpdateManager.WEAPONUPDATES += UpdateManager_WEAPONUPDATES;
    }

    void UpdateManager_WEAPONUPDATES()
    {
        ticker = Time.deltaTime;
        timer = timer + ticker;
        Rotatate();
        if (LaunchButton) Throttle();
    }

    internal override void Engage()
    {

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
        this.gameObject.transform.Rotate(Yrotary, rotation);

   //     emission.transform.Rotate(-Yrotary, rotation);
    }




    public void test()
    {
        Target = new Vector3(2400, 0, 2400);
    }

    private void Throttle()
    {
        float distance = Vector3.Distance(Target, this.gameObject.transform.position);



        if (distance > HalfDistance && timer < 4000)
        {
            Speed *= SPEED_FACTOR;
            movingDirection = (movingDirection + Aim() /100).normalized;
             // Vector3.Distance(Target, gameObject.transform.position);

        }
        else
        {
            movingDirection = (movingDirection + (Aim() / ((distance / 2) / HalfDistance))).normalized;
       //     movingDirection = (Aim() + lastMovedDirection)/2f;
       //     lastMovedDirection = movingDirection;
        }
        lastMovedDirection = movingDirection;
            this.gameObject.transform.up = -movingDirection;
            this.gameObject.transform.position += (movingDirection * Speed);
            
            GUIScript.AddTextLine(HitInfo);
    }

    public Vector3 Aim()
    {

        return  (Target - this.gameObject.transform.position ).normalized; // / Vector3.Distance(Target, this.gameObject.transform.position)).normalized;
       
    }
    private Vector3 Aim(Vector3 target)
    {
       return Target = target;
  //      return (target - this.gameObject.transform.position).normalized; // / Vector3.Distance(Target, this.gameObject.transform.position)).normalized;

    }


    //void OnTriggerEnter(Collider other)
    //{
    //    HitInfo = "RocketHit at: " + other.gameObject.name + " " + other.gameObject.GetInstanceID();

    //    if (other.gameObject.tag == "Ground") GameObject.Destroy(this.gameObject);
    //    if (other.gameObject.GetComponent<UnitSqript>().GoodOrEvil != this.GoodOrEvil)
    //    { other.gameObject.GetComponent<UnitSqript>().Options.Hit(1000); GameObject.Destroy(this.gameObject); }

    //}

    //void Update()
    //{
    //    DoUpdate();
    //}





}
*/