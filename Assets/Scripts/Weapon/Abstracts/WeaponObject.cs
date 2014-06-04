using UnityEngine;
using System.Collections;

public abstract class WeaponObject : MonoBehaviour
{
    public enum AMUNITONTYPE : byte
    {
        None = 0,
        Missiles = 1,
        Rocket = 2,
        Laser = 3,
        smallMGbullets = 4,
    }
    public UnitScript UNIT;
    public virtual AMUNITONTYPE amunition
    { 
      get { return AMUNITONTYPE.None; } 
    }
    public virtual UnitWeapon.WEAPON WEAPON
    {
        get { return UnitWeapon.WEAPON.None; }
    }
    abstract public float MAX_RANGE
    { get; }
 //   protected UnitScript UNIT;
    public FoE GoodOrEvil;
    public Vector3 Target;

    public void SetShooter(GameObject unit)
    {
        UNIT = unit.GetComponent<UnitScript>();
        GoodOrEvil = unit.GetComponent<UnitScript>().GoodOrEvil;
    }

    void Start()
    {
        StartUp();
    }
    abstract internal void StartUp();

    abstract internal void Engage();



}
