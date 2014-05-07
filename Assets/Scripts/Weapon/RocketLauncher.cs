///<summary>RocketLauncher
///by: Kalle Münster
///
/// Add it to a Unit to make it can Fire Rockets...
/// uses RocketObjects as Amunition...
/// 
///</summary>

using UnityEngine;
using System.Collections;
using System;

[AddComponentMenu("Program-X/Weapons/Rocket Launcher")]
public class RocketLauncher : UnitWeapon {



    private float Interval
    { get { return (prefabSlot.amunition == WeaponObject.AMUNITONTYPE.Missiles) ? 2f : 15.0f; } }
    public float countdown=0f;
    private RocketObject rocket;
    
	void Start ()
    {
 
	}

    public override void Engage(GameObject targetUnit)
    {
        
        Engage(targetUnit.gameObject.transform.position);
     //   rocket.TargetUpdatePosition(targetUnit.gameObject.transform.position);
    }
    public override void Engage(Vector3 targetPoint)
    {
        if (rocket)
        {
            rocket.SetTarget(targetPoint, this.gameObject.GetComponent<UnitScript>().GoodOrEvil);
            rocket.LaunchButton = true;
            rocket = null;
            countdown = Interval;
        }
    }
    public override void Reloade()
    {
        if (!rocket)
        {
            if ((countdown -= Time.deltaTime) < 0f)
            {
                rocket = (GameObject.Instantiate(prefabSlot, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 10f, this.gameObject.transform.position.z), this.gameObject.transform.rotation) as RocketObject).GetComponent<RocketObject>();
                rocket.transform.up = new Vector3(0, -1, 0);
            }
        }
    }
    public override float GetMaximumRange()
    {
        return prefabSlot.MAX_RANGE;
    }
}
