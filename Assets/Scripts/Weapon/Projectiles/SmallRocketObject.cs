using UnityEngine;
using System.Collections;

[AddComponentMenu("Program-X/Weapons/Amunition/Rockets/Kurze Dicke")]
public class SmallRocketObject : RocketObject
{
    private string HITinfo="";
    public const int DAMAGE = 500;
    public const float EXPLOSION_RADIUS = 10f;
    public override float MAX_RANGE
    {
        get { return 100f; }
    }
    public override WeaponObject.AMUNITONTYPE amunition
    {
        get
        {
            return WeaponObject.AMUNITONTYPE.Missiles;
        }
    }
    private bool IsExploading = false;
    public Vector3 A,B;
    public float Z,z;
    public AudioClip BOOMsound;

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
                gameObject.GetComponent<AudioSource>().Play();
            }
            return launch;
        }
        set { launch = value; }
    }
    [SerializeField]
    private bool launch = false;
    private UnitScript HitUNIT;
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

    public float rotation;
    internal override void StartUp()
    {
        IsExploading = false;
        UpdateManager.WEAPONUPDATES += UpdateManager_WEAPONUPDATES;

       for (int i = 0; i < transform.childCount; i++)
			{
                if (transform.GetChild(i).gameObject.name == "Emission") emission = transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();
                if (transform.GetChild(i).gameObject.name == "Sprite") spriteRenderer = transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
			}

    //   Target = new Vector3(-290, 100, 290);
    }

    void UpdateManager_WEAPONUPDATES()
    {
        this.gameObject.transform.GetChild(1).gameObject.GetComponent<rotary>().Rotation(InlineRotation);
        this.gameObject.transform.Rotate(RotatorAmount.x, RotatorAmount.y, RotatorAmount.z);
        if (LaunchButton) Throttle();
        if (HITinfo != "")
        {
            GUIScript.AddTextLine(HITinfo);
            HITinfo = "";
        }

    }
    
    

    private bool IsEnemy(UnitScript hit)
    {
        if (hit)
        {
            HitUNIT = hit;
            return hit.IsEnemy(GoodOrEvil);
        }
        else return false;
    }
    private UnitScript Ground(Collider hit)
    {
        UnitScript returner = (hit.gameObject.layer == ((int)EnumProvider.LAYERNAMES.Ignore_Raycast)) ? null : hit.GetComponent<UnitScript>();
        GUIScript.AddTextLine(HITinfo);
        if ((returner==null)&&(!hit.isTrigger)&&(returner.IsMySelf(this.gameObject))) 
            return hit.gameObject.GetComponent<UnitScript>();
        else if(hit.gameObject.layer==(int)EnumProvider.LAYERNAMES.Ignore_Raycast)
        {
            HITinfo="GroundHit at: "+this.gameObject.transform.position.ToString();
            (this.gameObject.collider as SphereCollider).radius = EXPLOSION_RADIUS;
            Exploade(EXPLOSION_RADIUS);
        }
        return null;
    }

    public void SpriteColliderEnter(GameObject hitten)
    {
        hitten.GetComponent<UnitScript>();
        hitten.GetComponent<UnitScript>().Options.IsUnderAttack = true;
        hitten.gameObject.GetComponent<UnitScript>().Hit(DAMAGE);
        GameObject.Destroy(gameObject);
    }

    void OnTriggerEnter(Collider hit)
    {
      if(!hit.isTrigger)  HITinfo = hit.name + hit.gameObject.GetInstanceID() + " Hit at: " + this.gameObject.transform.position.ToString();
        if (!IsExploading)
        {
            if (IsEnemy(HitUNIT = Ground(hit)))
            {
                HITinfo = hit.name + hit.gameObject.GetInstanceID() + " Hit at: " + this.gameObject.transform.position.ToString();


            }
        }       
    }

    private float Exploade(float radius)
    {
        if (!IsExploading)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(BOOMsound);
            IsExploading = true;
        }

        if (radius <= 0f) GameObject.Destroy(this.gameObject);
        else if (radius < 0.01f)
        {
            emission.GetComponent<TimedDestructor>().DestroyByTimer(1);
            GameObject.Destroy(transform.FindChild("Sprite").gameObject);
            this.transform.DetachChildren();
            radius = 0f;
            (this.gameObject.collider as SphereCollider).radius = radius;
            return Exploade(radius);
        }
        else
        {
            (this.gameObject.collider as SphereCollider).radius = radius;
            return Exploade(radius * 0.9f);
        }
        return 0f;
    }
    void OnTriggerExit(Collider hit)
    {
        if (IsExploading)
        {
            if (IsEnemy(Ground(hit)))
            {
                HITinfo = HitUNIT.name + " " + HitUNIT.gameObject.GetInstanceID() + " Hit at: " + hit.collider.ClosestPointOnBounds(this.gameObject.transform.position);
                
                HitUNIT.Hit((int)(DAMAGE / ((EXPLOSION_RADIUS / this.gameObject.transform.localScale.x) - (this.gameObject.collider as SphereCollider).radius)));
            }
        }
    }

    public float THROTTLETIME = 8;

    internal override void Engage()
    {
        
    }

    private void Throttle()
    {
        timer += Time.deltaTime;
        if (timer > THROTTLETIME) Exploade(EXPLOSION_RADIUS);
        RotatorAmount = Vector3.Lerp(A, B,Mathf.Clamp( timer/Duration,0,1));
        InlineRotation.z = Mathf.Lerp(z, Z,Mathf.Clamp( timer / Duration,0f,1f));
        this.gameObject.transform.forward += Aim(Target) / HalfDistance;
        this.gameObject.transform.position += (this.transform.forward * (Speed *= SPEED_FACTOR)) ;
        emission.startSpeed = Speed;
    }

    public override Vector3 Aim(Vector3 targetPosition)
    {
        return (targetPosition - this.gameObject.transform.position).normalized;
    }
    public Vector3 InlineRotation;
    public Vector3 RotatorAmount;


    public override UnitWeapon.WEAPON WEAPON
    {
        get
        {
            return UnitWeapon.WEAPON.RocketLauncher;
        }
    }

    void OnDestroy()
    {
        UpdateManager.WEAPONUPDATES -= UpdateManager_WEAPONUPDATES;
    }

}
