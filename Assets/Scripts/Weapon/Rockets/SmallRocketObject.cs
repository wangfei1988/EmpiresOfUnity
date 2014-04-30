using UnityEngine;
using System.Collections;

public class SmallRocketObject : Rocket
{
    private string HITinfo="";
    public const int DAMAGE = 500;
    public const float EXPLOSION_RADIUS = 100f;
    private bool IsExploading = false;
    public Vector3 A,B;
    public float Z,z;
    
    public override bool LaunchButton
    {
        get 
        {
            if (launch&&!Visible)
            {
                A = new Vector3(Random.Range(-0.5f, 0.53f), Random.Range(-0.23f, 0.2f), Random.Range(-10, 8));
                InlineRotation.y = 90f;
                z = Random.Range(-5, 5);
                B = new Vector3(Random.Range(-0.002f, 0.001f), Random.Range(0f, 0.0005f), Random.Range(-1.5f, 1.5f));
                Z = Random.Range(-56f, 46f);

                Visible = true;

                emission.Play();
                movingDirection = this.gameObject.transform.forward;


            //    Target = new Vector2(0, 0);

                HalfDistance = Vector3.Distance(Target, this.gameObject.transform.position) / 100f;
                
            }
            return launch;
        }
        set { launch = value; }
    }
    [SerializeField]
    private bool launch = false;
    private UnitScript Hit;
    public float Speed
    {
        get { return speed; }
        set
        {
            if (value > MAXIMUM_SPEED) speed = MAXIMUM_SPEED;
            else speed = value;
        }
    }
    public float timer=0f;
    public float Duration;
    public float speed;
    public float SPEED_FACTOR;
    public float MAXIMUM_SPEED;
    private Vector3 movingDirection;
    private Renderer spriteRenderer;
    private float HalfDistance;
    public ParticleSystem emission;
    private bool visible = false;
    public bool Visible
    {
        get { return spriteRenderer.enabled = this.gameObject.collider.enabled = visible; }
        set { visible = this.gameObject.collider.enabled = spriteRenderer.enabled = value; }
    }
 //   public UnitScript.GOODorEVIL GoodOrEvil;
    public float rotation;
    internal override void StartUp()
    {
        UpdateManager.WEAPONUPDATES += UpdateManager_WEAPONUPDATES;
        amunition = AMUNITON.Missiles;
       for (int i = 0; i < transform.childCount; i++)
			{
                if (transform.GetChild(i).gameObject.name == "Emission") emission = transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();
                if (transform.GetChild(i).gameObject.name == "Sprite") spriteRenderer = transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
			}

       Target = new Vector3(-290, 100, 290);
    }

    void UpdateManager_WEAPONUPDATES()
    {
        if (HITinfo != "")
            GUIScript.AddTextLine(HITinfo);
        this.gameObject.transform.GetChild(1).gameObject.GetComponent<rotary>().Rotation(InlineRotation);
        this.gameObject.transform.Rotate(RotatorAmount.x, RotatorAmount.y, RotatorAmount.z);
        if (LaunchButton) Throttle();
    }
    
    
    void OnTriggerEnter(Collider hit)
    {
        if (!IsExploading)
        {
            if (IsEnemy(Hit = Ground(hit)))
            {
                HITinfo = Hit.name + hit.gameObject.GetInstanceID() + " Hit at: " + this.gameObject.transform.position.ToString();
                Hit.Options.IsUnderAttack = true;
                Hit.Hit(DAMAGE);
                GameObject.Destroy(this.gameObject);

            }
        }

      //      
    }
    private bool IsEnemy(UnitScript hit)
    {
        if (hit) return hit.GoodOrEvil != this.gameObject.GetComponent<UnitScript>().GoodOrEvil;
        else return false;
    }
    private UnitScript Ground(Collider hit)
    {
        if (hit.gameObject.layer != 2) return hit.gameObject.GetComponent<UnitScript>();
        else
        {
            HITinfo="GroundHit at: "+this.gameObject.transform.position.ToString();
            (this.gameObject.collider as SphereCollider).radius = EXPLOSION_RADIUS / this.gameObject.transform.localScale.x;
            if (Exploade(EXPLOSION_RADIUS) == 0f) {  GameObject.Destroy(this.gameObject); };
        }
        return null;
    }
    private float Exploade(float radius)
    {
        if (radius < 0.1f) return 0;
        else
        {


            return Exploade(radius * 0.99f);
        }
    }
    void OnTriggerExit(Collider hit)
    {
        if (IsEnemy(Ground(hit)))
        {
            HITinfo = Hit.name + " " + Hit.gameObject.GetInstanceID() + " Hit at: " + hit.collider.ClosestPointOnBounds(this.gameObject.transform.position);
            Hit.Options.IsUnderAttack = true; 
            Hit.Hit( (int)(DAMAGE / ((EXPLOSION_RADIUS / this.gameObject.transform.localScale.x) - (this.gameObject.collider as SphereCollider).radius))); 
        }
    }



    internal override void Engage()
    {
        
    }

    private void Throttle()
    {
        timer += Time.deltaTime;
        RotatorAmount = Vector3.Lerp(A, B,Mathf.Clamp( timer/Duration,0,1));
        InlineRotation.z = Mathf.Lerp(z, Z,Mathf.Clamp( timer / Duration,0f,1f));
        this.gameObject.transform.forward += Aim(Target) / HalfDistance;
        this.gameObject.transform.position += (this.transform.forward * (Speed *= SPEED_FACTOR)) ;
        emission.startSpeed = Speed;
    }

    public Vector3 Aim(Vector3 targetPosition)
    {
        return (targetPosition - this.gameObject.transform.position).normalized;
    }
    public Vector3 InlineRotation;
    public Vector3 RotatorAmount;


    public override Weapon.WEAPON WEAPON
    {
        get
        {
            return Weapon.WEAPON.RocketLauncher;
        }
    }


}
