using UnityEngine;

/*
 * RocketLauncher
 * Author: Kalle Münster
 * Description: Add it to a Unit to make it can fire Rockets
 *              It uses RocketObjects as Amunition...
 */
[AddComponentMenu("Program-X/Weapons/Rocket Launcher")]
public class RocketLauncher : UnitWeapon
{

    public int NumberOfRockets;
    public override bool IsOutOfAmu
    {
        get { return NumberOfRockets == 0; }
    }

    private float Interval
    {
        get { return (prefabSlot.amunition == WeaponObject.AMUNITONTYPE.Missiles) ? 0.5f : 3.0f; }
    }

    public float countdown=0f;
    private RocketObject rocket;
    
	void Start ()
    {
        if (GetComponent<UnitScript>())
            UNIT = GetComponent<UnitScript>();
        else if (transform.parent.gameObject.GetComponent<UnitScript>())
            UNIT = transform.parent.gameObject.GetComponent<UnitScript>();
        //NumberOfRockets = (prefabSlot.amunition == WeaponObject.AMUNITONTYPE.Missiles) ? 100 : 15;
	}

    public override void Engage(GameObject targetUnit)
    {
        if (targetUnit)
            Engage(targetUnit.transform.position);
    }

    public override void Engage(Vector3 targetPoint)
    {
        if (rocket)
        {
            rocket.SetTarget(targetPoint, UNIT.GoodOrEvil);
            rocket.LaunchButton = true;
            rocket = null;
            countdown = Interval;
        }
    }

    public override void Reload()
    {
        if (!rocket && NumberOfRockets > 0)
        {
            float offset = 1f;
            if (prefabSlot.amunition == WeaponObject.AMUNITONTYPE.Rocket)
                offset = 15f;
            if ((countdown -= Time.deltaTime) <= 0f)
            {
                rocket = (GameObject.Instantiate(prefabSlot, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + offset, this.gameObject.transform.position.z), this.gameObject.transform.rotation) as RocketObject).GetComponent<RocketObject>();

                NumberOfRockets--;
                
                if (prefabSlot.amunition == WeaponObject.AMUNITONTYPE.Missiles)
                {
                    rocket.transform.forward = (rocket.transform.forward + new Vector3(Random.Range(-0.2f, 0.2f), 0.85f, Random.Range(-0.2f, 0.2f))).normalized;
                }
                //    rocket.transform.forward = (rocket.transform.forward + new Vector3(0, 1f, 0)).normalized;
                //  rocket.transform.forward = Vector3.up;
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
