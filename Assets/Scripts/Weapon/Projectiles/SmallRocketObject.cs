using UnityEngine;
using System.Collections;


public class SmallRocketObject : RocketObject
{
    public int DAMAGE = 50;
    public const float EXPLOSION_RADIUS = 10f;
    public float _mAX_RANGE=100;
    public override float MAX_RANGE
    {
        get { return _mAX_RANGE; }
    }
    public override WeaponObject.AMUNITONTYPE amunition
    {
        get
        {
            return WeaponObject.AMUNITONTYPE.Missiles;
        }
    }
    private bool IsExploading = false;
    public float wobbleFactor = 10f;
    private Vector3 A,B;
    private float Z,z;
    public AudioClip BOOMsound;
    private Vector3 InlineRotation;
    private Vector3 RotatorAmount;
    public override bool LaunchButton
    {
        get 
        {
            if (launch&&!Visible)
            {
                A = new Vector3(Random.Range(-wobbleFactor, wobbleFactor), Random.Range(-wobbleFactor, wobbleFactor), Random.Range(5,20));
                InlineRotation.y = 90f;
          //      z = Random.Range(-5, 5);
               B = new Vector3(0, 0, 0);
           //     Z = Random.Range(-56f, 46f);

                Visible = true;

                emission.Play();

          //      this.gameObject.transform.forward = new Vector3(0f, 1f, 0f);

                //HalfDistance = Vector3.Distance(Target, this.gameObject.transform.position) / 2;
                gameObject.audio.Play();
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
    private float timer=0f;
    public float Duration=2;
    public float speed;
    public float SPEED_FACTOR;
    public float MAXIMUM_SPEED;
    private Renderer spriteRenderer;
    //private float HalfDistance;
    public ParticleSystem emission;
    private bool visible = false;
    public bool Visible
    {
        get { return spriteRenderer.enabled = visible; }
        set { visible = spriteRenderer.enabled = this.transform.GetChild(1).collider.enabled = value; }
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
    }


 //   public bool staticExplosionStarted = false;
    void UpdateManager_WEAPONUPDATES()
    {//  Flying...
        if (!IsExploading)
        {
            Rotation();
            if (LaunchButton) Throttle();
        }
        else
        {// Exploading !


            if (this.audio.isPlaying)
            {
                if (emission.enableEmission)
                {
                    emission.enableEmission = false;
                    StaticExploader.Exploade(0, this.gameObject.transform.position);
                }
                // later an ID for playing an AudioClip could be addedt in the StaticExploasionsManager....
            }
            else
            {
                if (this.transform.GetChild(1).gameObject)
                    GameObject.Destroy(this.transform.GetChild(1).gameObject);
                if (this.transform.GetChild(0).gameObject)
                    GameObject.Destroy(this.transform.GetChild(0).gameObject);

                GameObject.Destroy(this.gameObject);
            }
        }
    }



    private bool IsEnemy(GameObject hit)
    {
        if (hit.gameObject.GetComponent<UnitScript>())
        {
            HitUNIT = hit.gameObject.GetComponent<UnitScript>();
            return this.GoodOrEvil + HitUNIT.GoodOrEvil;
        }
        else return false;
    }


    public void SpriteColliderEnter(GameObject hitten)
    {
        if ((!IsExploading)
        && (!hitten.collider.isTrigger)
        && (IsEnemy(hitten)))
        {
            if (IsEnemy(hitten))
            {
                HitUNIT.Hit(Exploade(DAMAGE));
            }
            else
                Exploade(0);
        }
    }

    //void OnTriggerEnter(Collider hit)
    //{
    //    if (!hit.isTrigger)
    //    {
    //        if (!IsExploading)
    //        {
    //            if (IsEnemy(hit))
    //            {
    //                HitUNIT.Hit(Exploade(DAMAGE));
    //            }
    //        }
    //    }
    //}

    private int Exploade(int damage)
    {
        if (!IsExploading)
        {
            this.gameObject.audio.clip=BOOMsound;
            this.gameObject.audio.Play();
            this.IsExploading = true;
            this.Visible = false;
            this.launch = false;
            return damage;
        }
        else return 0;
    }



    public float THROTTLETIME = 8;

    internal override void Engage()
    {
        
    }


    private void Rotation()
    {
        this.gameObject.transform.GetChild(1).gameObject.GetComponent<Rotary>().Rotation(InlineRotation);
        this.gameObject.transform.Rotate(RotatorAmount.x, RotatorAmount.y, RotatorAmount.z);
    }

    private void Throttle()
    {

        timer += Time.deltaTime;
        if (timer > THROTTLETIME) Exploade(DAMAGE);
        RotatorAmount = Vector3.Lerp(A, B,Mathf.Clamp( timer/Duration,0,1));
     //   InlineRotation.z = Mathf.Lerp(z, Z,Mathf.Clamp( timer / Duration,0f,1f));
        this.transform.forward += (Vector3.up / (this.transform.position.y * 10));
        this.gameObject.transform.forward += Aim(Target);
        this.transform.position += (this.transform.forward * (Speed *= SPEED_FACTOR));
        emission.startSpeed = Speed;
        if (this.transform.position.y <= 0f)
        {
            this.transform.position = new Vector3(this.transform.position.x,0.1f,this.transform.position.z);
            this.Exploade(DAMAGE);
        }
    }

    public override Vector3 Aim(Vector3 targetPosition)
    {
        return (this.transform.forward * Mathf.SmoothStep(1f, 0f, Mathf.Clamp(timer / Duration, 0, 1)) + (targetPosition - this.gameObject.transform.position).normalized * Mathf.SmoothStep(0f, 1f, Mathf.Clamp(timer / Duration, 0f, 1f))).normalized;
    }

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
