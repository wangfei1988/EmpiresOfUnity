using UnityEngine;
using System.Collections;

public class releasePoin : MonoBehaviour 
{

    public UnitScript UNIT;

	void Start () 
    {
        UNIT = this.transform.parent.gameObject.GetComponent<UnitScript>();
	}

    public Vector3 TakenOff()
    {
         
         UNIT.GetComponent<Animator>().SetBool("Release", false);
         return this.transform.TransformPoint(this.gameObject.transform.localPosition);
    }
}
