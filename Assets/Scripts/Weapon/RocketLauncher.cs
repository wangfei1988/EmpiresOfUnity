using UnityEngine;
using System.Collections;
using System;

public class RocketLauncher : Weapon {



 //   public GameObject RocketPreefab;
    private Rocket rocket;
    
    private int counter;
	void Start ()
    {
        counter = 0;
  
	}

    public override void Engage(GameObject targetUnit)
    {
        rocket.Launch(targetUnit.gameObject.transform.position, this.gameObject.GetComponent<UnitSqript>().GoodOrEvil);
        rocket.TargetUpdatePosition(targetUnit.gameObject.transform.position);
    }
    public override void Engage(Vector3 targetPoint)
    {

        rocket.Launch(targetPoint,this.gameObject.GetComponent<UnitSqript>().GoodOrEvil);
    
    }
    public override void Reloade()
    {

        if (!rocket)
        {
            rocket = (GameObject.Instantiate(preefabSlot, new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 100f, this.gameObject.transform.position.z), this.gameObject.transform.rotation) as Rocket).GetComponent<Rocket>();
            rocket.transform.up = new Vector3(0, -1, 0);
        }
        
        rocket.DoUpdate();
        
    }
    public override float GetMaximumRange()
    {
        return 3000f;
    }
}
