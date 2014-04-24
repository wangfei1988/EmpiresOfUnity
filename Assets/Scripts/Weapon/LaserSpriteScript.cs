using UnityEngine;
using System.Collections;

public class LaserSpriteScript : WeaponObject 

{
    public enum dir : byte
    {
        forward,
        Right,
        up
    }
    private Vector3 YAxis = new Vector3(0f,1f,0f);
    public dir richtung;
    public Vector3 t;
    private const float SPEED = 50f;
    public AudioClip sound2;
    private int count;
    public float Step;
    public override Weapon.WEAPON WEAPON
    {
        get
        {
            return Weapon.WEAPON.RayGun;
        }
    }
    public Flare Hitflare;
    public float rotation = 90f;
    private Vector3 originPosition;
    public float beamPosition = 20;
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
    public float BeamPosition
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
    public float MAX_POSITION;
    private int Power;

    private Vector3 GetBeamPositionScale(float beampos)
    {
        return new Vector3(this.gameObject.transform.localScale.x,beampos , this.gameObject.transform.localScale.z);
    }

    internal override void StartUp()
    {
        ammunition = AMMUNITON.Laser;
        count = (int)SPEED;
        Visible = false;
        Direction = this.gameObject.transform.forward;
        originPosition = this.gameObject.transform.position;
        beamPosition = 10;

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
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 11f, this.gameObject.transform.position.z);
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
           if(++count >=102)GameObject.Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer != 2) && (other.gameObject.GetInstanceID() != UNIT.gameObject.GetInstanceID()) && (other.gameObject.layer!=8) && (other.GetComponent<UnitScript>().GoodOrEvil!=GoodOrEvil))
        {
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(sound2);
            Camera.main.GetComponent<Cam>().mainGUI.guiText.text = other.gameObject.name + other.gameObject.GetInstanceID().ToString();
            HIT = true;
            other.GetComponent<UnitScript>().Options.Hit(this.Power); 
        }
    }

    internal override void DoUpdate()
    {
        Beam();
    }


}
