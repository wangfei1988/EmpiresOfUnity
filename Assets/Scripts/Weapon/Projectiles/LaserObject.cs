/* LaserObject
 * by: Kalle Münster
 * 
 * Script for Handling a Laser "Projectile" wich
 * will be fired by the LightLaserGun.
 */
using UnityEngine;
using System.Collections;

[AddComponentMenu("Program-X/Weapons/Amunition/LaserObject")]
public class LaserObject : WeaponObject 

{
    public enum dir : byte
    {
        forward,
        Right,
        up
    }
    public override WeaponObject.AMUNITONTYPE amunition
    {
        get
        {
            return WeaponObject.AMUNITONTYPE.Laser;
        }
    }
    public override float MAX_RANGE
    {
        get {return LightLaserGun.MAXIMUM_DISTANCE; }
    }
    private Vector3 YAxis = new Vector3(0f,1f,0f);
    public dir richtung;
    public Vector3 t;
    private const float SPEED = 50f;
    public AudioClip sound2;
    private int count;
    private float Step;
    public override UnitWeapon.WEAPON WEAPON
    {
        get
        {
            return UnitWeapon.WEAPON.RayGun;
        }
    }
    public Flare Hitflare;
    private float rotation = 90f;
    private Vector3 originPosition;
    private float beamPosition = 0.5f;
    [SerializeField]
    private bool hit = false;
    public bool HIT
    {
        get { return hit; }
        set
        {
            if (value)
            {
                this.gameObject.light.intensity = 1f;
                this.gameObject.light.flare = Hitflare;
            }
            hit = value;
        }
    }
    private float BeamPosition
    {
        get 
        {
            
            if (beamPosition >= MAX_POSITION) { hit = true; Step = -Step; }
            else if(!hit) this.gameObject.light.range = beamPosition;

            return this.gameObject.light.range;
        }
        set 
        {
            if (value <= 0f) Object.Destroy(this);
            else if (!hit) beamPosition = value;
            else
            {
                if (value < this.gameObject.light.range) this.gameObject.light.range = value; ;
            }
        }
    }
    private bool IsLoadedt = false;
    public Vector3 Direction;
    public bool Visible
    {
        get { return (this.gameObject.renderer.enabled & this.gameObject.light.enabled); }
        set { this.gameObject.renderer.enabled = this.gameObject.light.enabled = this.collider.enabled=value; }
    }
    private float MAX_POSITION;
    private int Power;

    private Vector3 GetBeamPositionScale(float beampos)
    {
        return new Vector3(this.gameObject.transform.localScale.x,beampos , this.gameObject.transform.localScale.z);
    }

    internal override void StartUp()
    {
        count = (int)SPEED;
        Visible = false;
        Direction = this.gameObject.transform.forward;
        originPosition = this.gameObject.transform.position;
        beamPosition = 0.5f;
        UpdateManager.UNITUPDATE += UpdateManager_UNITUPDATE;
    }



    internal override void Engage()
    {
        if(IsLoadedt)       
        Visible = true;
    }

    public bool Load(Vector3 direction, int power, float maximumdistance)
    {
        if (!IsLoadedt)
        {
            Power = power;
            MAX_POSITION = maximumdistance;
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.9f, this.gameObject.transform.position.z);
            this.gameObject.transform.up = direction;
            Direction = this.gameObject.transform.forward;
            Step = MAX_POSITION / SPEED;
            return IsLoadedt = true;
        }
        else return false;
    }

    private void Beam()
    {
        if (Visible)
        {
          
            BeamPosition += Step;
            this.gameObject.transform.localScale = GetBeamPositionScale(BeamPosition);
            this.gameObject.transform.position = originPosition + (this.beamPosition * this.gameObject.transform.up);
            this.gameObject.transform.Rotate(YAxis, rotation);
            if (++count >= SPEED * 2)
            {
                UpdateManager.UNITUPDATE -= UpdateManager_UNITUPDATE;
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ((!other.collider.isTrigger) && (other.gameObject.layer==(int)EnumProvider.LAYERNAMES.Units) && (other.gameObject.GetComponent<UnitScript>().IsEnemy(this.GoodOrEvil)))
        {
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(sound2);
            GUIScript.AddTextLine(other.gameObject.name + other.gameObject.GetInstanceID().ToString());
            HIT = true;
            other.collider.GetComponent<UnitScript>().Hit(this.Power); 
        }
    }

    void UpdateManager_UNITUPDATE()
    {
        Beam();
    }

}
