using UnityEngine;
using System.Collections;

public class LightLaser : Weapon 
{
    public const int LOADINGFACTOR = 2;
    public const int MAX_LASER_ENERGY = 1000;
    public const int DURATION=250;
    public const int MAXIMUM_POWER = 100;
    public const int MINIMUM_POWER = 33;
    public const float MAXIMUM_DISTANCE = 100;

    private LaserWeaponObject laser;
    public int LaserEnergie;
    private int frameCounter;
    private bool IsLoadedt;

    void Start()
    {
        IsLoadedt = false;
        LaserEnergie = MAX_LASER_ENERGY;
        frameCounter = 0;
    }

    public override void Engage(GameObject targetUnit)
    {
        Engage(targetUnit.transform.position);
    }
    public override void Engage(Vector3 targetPoint)
    {
        if (!IsLoadedt)
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
            laser = (GameObject.Instantiate(prefabSlot, gameObject.transform.position, gameObject.transform.rotation) as WeaponObject).GetComponent<LaserWeaponObject>();
            laser.gameObject.name = "Laser " + this.gameObject.GetInstanceID();
            laser.GoodOrEvil = this.gameObject.GetComponent<UnitScript>().GoodOrEvil;
            IsLoadedt = laser.Load((targetPoint-this.gameObject.transform.position).normalized, Power, MAXIMUM_DISTANCE);  
        }
        laser.Engage();
    }

    public override float GetMaximumRange()
    {
        return MAXIMUM_DISTANCE;
    }

    public override void Reloade()
    {
        if (++frameCounter == LOADINGFACTOR)
        {
           if(LaserEnergie<MAX_LASER_ENERGY)  LaserEnergie++;
            frameCounter = 0;
        }
        if (laser == null) IsLoadedt = false;
    }
}
