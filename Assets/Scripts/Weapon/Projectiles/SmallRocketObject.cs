using UnityEngine;
using System.Collections;

[AddComponentMenu("Program-X/Weapons/Amunition/Rockets/Kurze Dicke")]
public class SmallRocketObject : RocketObject
{
    public const int DAMAGE = 50;
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
    public Vector3 InlineRotation;
    public Vector3 RotatorAmount;
    public override bool LaunchButton
    {
        get 
        {
            if (launch&&!Visible)
            {
                A = new Vector3(Random.Range(-5.5f, 10f), Random.Range(-5.23f, 5.2f), Random.Range(5,20));
                InlineRotation.y = 90f;
          //      z = Random.Range(-5, 5);
               B = new Vector3(0, 0, 0);
           //     Z = Random.Range(-56f, 46f);

                Visible = true;

                emission.Play();

          //      this.gameObject.transform.forward = new Vector3(0f, 1f, 0f);

                HalfDistance = Vector3.Distance(Target, this.gameObject.transform.position) / 2;
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
    public float timer=0f;
    public float Duration;
    public float speed;
    public float SPEED_FACTOR;
    public float MAXIMUM_SPEED;
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
    }

    void UpdateManager_WEAPONUPDATES()
    {//  Flying...
        if (!IsExploading)
        {
            this.gameObject.transform.GetChild(1).gameObject.GetComponent<rotary>().Rotation(InlineRotation);
            this.gameObject.transform.Rotate(RotatorAmount.x, RotatorAmount.y, RotatorAmount.z);
            if (LaunchButton) Throttle();
        }
        else
        {// Exploading !


            if (this.audio.isPlaying)
            {
                if (emission.enableEmission)
                    emission.enableEmission = false;
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
    
    

    private bool IsEnemy(Collider hit)
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
        if (!hitten.collider.isTrigger)
        {
            if (!IsExploading)
            {
                IsExploading = true;
                hitten.gameObject.GetComponent<UnitScript>().Hit(Exploade(DAMAGE));
            }
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        if (!hit.isTrigger)
        {
            if (!IsExploading)
            {
                if (IsEnemy(hit))
                {
                    HitUNIT.Hit(Exploade(DAMAGE));
                }
            }
        }
    }

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

    //void OnTriggerExit(Collider hit)
    //{
    //    if (IsExploading)
    //    {
    //        if (IsEnemy(hit))
    //        {
    //            HITinfo = HitUNIT.name + " " + HitUNIT.gameObject.GetInstanceID() + " Hit at: " + hit.collider.ClosestPointOnBounds(this.gameObject.transform.position);
                
    //            HitUNIT.Hit((int)(DAMAGE / ((EXPLOSION_RADIUS / this.gameObject.transform.localScale.x) - (this.gameObject.collider as SphereCollider).radius)));
    //        }
    //    }
    //}

    public float THROTTLETIME = 8;

    internal override void Engage()
    {
        
    }

    private void Throttle()
    {

        timer += Time.deltaTime;
        if (timer > THROTTLETIME) Exploade(DAMAGE);
        RotatorAmount = Vector3.Lerp(A, B,Mathf.Clamp( timer/Duration,0,1));
     //   InlineRotation.z = Mathf.Lerp(z, Z,Mathf.Clamp( timer / Duration,0f,1f));
        this.gameObject.transform.forward += Aim(Target);
        this.transform.position += (this.transform.forward * (Speed *= SPEED_FACTOR));
        emission.startSpeed = Speed;
        if (this.transform.position.y <= 0f)
            this.Exploade(DAMAGE);
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
