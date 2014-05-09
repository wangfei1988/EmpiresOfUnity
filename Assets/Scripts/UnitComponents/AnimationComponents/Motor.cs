using UnityEngine;
using System.Collections;

public class Motor : UnitAnimation 
{

	void Start ()
    {
	
	}

    internal override void Animate()
    {
        this.gameObject.transform.position += this.gameObject.transform.forward * GetComponent<WingsAndJets>().Speed;
    }

}
