using UnityEngine;
using System.Collections;

public abstract class WeaponObject : MonoBehaviour 
{
    public enum AMUNITON : byte
    {
        None = 0,
        Missiles = 1,
        Rocket = 2,
        Laser = 3,
        PowerLaser = 4
    }
    public AMUNITON amunition;
  //  { get; protected set; }
    public virtual Weapon.WEAPON WEAPON
    {
        get { return Weapon.WEAPON.None; }
    }

    protected GameObject UNIT;
    public UnitSqript.GOODorEVIL GoodOrEvil;
    public Vector3 Target;

    public void SetShooter(GameObject unit)
    {
        UNIT = unit;
    }

    void Start()
    {
        StartUp();
    }
    abstract internal void StartUp();

    abstract internal void Engage();

    abstract internal void DoUpdate();

}
