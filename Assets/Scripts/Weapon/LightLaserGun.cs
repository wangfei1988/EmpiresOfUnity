///<summary>LightLaserGun
///by: Kalle Münster
///
///Component for shooting Laser Rays... uses "Laser" PreFabs as Amunition...
///
///</summary>
using UnityEngine;
using System.Collections;

[AddComponentMenu("Program-X/Weapons/Laser Gun")]
public class LightLaserGun : UnitWeapon 
{

    public const int LOADINGFACTOR = 50;          //-----------millisecons the lasergun need to regain 1 Laserenergy.. 
    public const int MAX_LASER_ENERGY = 1000;    //------------Maximum amount of Laserenergy the lasergun can hold.
    public const int DURATION=250;              //-------------Timeperiod a laserbeam needs to reach it's target...
    public const int MAXIMUM_POWER = 100;      //--------------Maximum Damage a fired Laser can cause / howmuch Laserenergy a fired Laserbeam needs..
    public const int MINIMUM_POWER = 33;      //---------------Minimum Laserenergy a Laserbeam need to be fired...
    [SerializeField]
    public static float MAXIMUM_DISTANCE = 50;  //-------------Maximum Range...

    public override bool IsOutOfAmu
    {
        get { return LaserEnergie < MINIMUM_POWER; }
    }

    private LaserObject laser; // variable that will hold the fired "Laser"-Projectile...

    public int LaserEnergie;          
    private int frameCounter;
    private bool IsLoadedt;

    void Start()
    {
        IsLoadedt = false;
        LaserEnergie = MAX_LASER_ENERGY;
        frameCounter = 0;
    }

    // Engage functions. to fire the "Laser" - projectiles...
    public override void Engage(GameObject targetUnit)
    {
        if (targetUnit != null)
            Engage(targetUnit.transform.position);
    }
    public override void Engage(Vector3 targetPoint)
    {
        if ((!IsLoadedt)&&(Vector3.Distance(gameObject.transform.position,targetPoint)<MAXIMUM_DISTANCE))
        {
            int Power;

            if (LaserEnergie < MAXIMUM_POWER && LaserEnergie > MINIMUM_POWER)
            {
                Power = LaserEnergie;
                LaserEnergie = 0;
            }
            else
            {
                Power = MAXIMUM_POWER;
                LaserEnergie -= MAXIMUM_POWER;
            }
            laser = (GameObject.Instantiate(prefabSlot, gameObject.transform.position, gameObject.transform.rotation) as WeaponObject).GetComponent<LaserObject>();
            laser.gameObject.name = "Laser " + this.gameObject.GetInstanceID();
            laser.GoodOrEvil = this.gameObject.GetComponent<UnitScript>().GoodOrEvil;
            IsLoadedt = laser.Load((targetPoint-this.gameObject.transform.position).normalized, Power, MAXIMUM_DISTANCE);  
        }
        laser.Engage();
    }

    public override float GetMaximumRange()
    {
        // This Method is called by the Units "UnitScript"-Proerty:"AttackRange"
        return MAXIMUM_DISTANCE;
    }

    public override void Reload()
    {
        // The Updatefunction for updating the LightLaser component...
        if (++frameCounter == LOADINGFACTOR)
        {
           if(LaserEnergie<MAX_LASER_ENERGY)  LaserEnergie++;
            frameCounter = 0;
        }
        if (laser == null) IsLoadedt = false;
    }
}
