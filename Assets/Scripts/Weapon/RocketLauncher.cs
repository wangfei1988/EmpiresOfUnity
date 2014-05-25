///<summary>RocketLauncher
///by: Kalle Münster
///
/// Add it to a Unit to make it can Fire Rockets...
/// uses RocketObjects as Amunition...
/// 
///</summary>

using UnityEngine;
using System.Collections;
//using System;

[AddComponentMenu("Program-X/Weapons/Rocket Launcher")]
public class RocketLauncher : UnitWeapon
{


    public int NumberOfRockets;
    public override bool IsOutOfAmmu
    {
        get { return NumberOfRockets == 0; }
    }

    private float Interval
    { get { return (prefabSlot.amunition == WeaponObject.AMUNITONTYPE.Missiles) ? 0.5f : 3.0f; } }
    public float countdown=0f;
    private RocketObject rocket;
    
	void Start ()
    {
        NumberOfRockets = (prefabSlot.amunition == WeaponObject.AMUNITONTYPE.Missiles) ? 100 : 15;
	}

    public override void Engage(GameObject targetUnit)
    {
        if (targetUnit)
            Engage(targetUnit.transform.position);
     //   rocket.TargetUpdatePosition(targetUnit.gameObject.transform.position);
    }
    public override void Engage(Vector3 targetPoint)
    {
        if (rocket)
        {
            rocket.SetTarget(targetPoint, this.gameObject.GetComponent<UnitScript>().GoodOrEvil);
     //       rocket.transform.forward = Vector3.forward;
            rocket.LaunchButton = true;
            rocket = null;
            countdown = Interval;
        }
    }
    public override void Reloade()
    {
        if (!rocket&&NumberOfRockets>0)
        {
            float offset = 1f;
            if (prefabSlot.amunition == WeaponObject.AMUNITONTYPE.Rocket)
                offset = 5f;
            if ((countdown -= Time.deltaTime) < 0f)
            {
                rocket = (GameObject.Instantiate(prefabSlot, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + offset, this.gameObject.transform.position.z), this.gameObject.transform.rotation) as RocketObject).GetComponent<RocketObject>();
                NumberOfRockets--;
                if (prefabSlot.amunition == WeaponObject.AMUNITONTYPE.Missiles)
                    rocket.transform.forward = (rocket.transform.forward + new Vector3(Random.Range(-0.1f, 0.1f), 1f, Random.Range(0f, 0.1f))).normalized;
                else if (prefabSlot.amunition == WeaponObject.AMUNITONTYPE.Rocket)
                    rocket.transform.up = -((rocket.transform.forward + new Vector3(Random.Range(-0.25f, 0.25f), 1f, Random.Range(-0.25f, 0.25f))).normalized);
            }
        }
    }
    public override float GetMaximumRange()
    {
        return prefabSlot.MAX_RANGE;
    }
}
