using UnityEngine;
using System.Collections;

public class Thruster : UnitAnimation 
{

    public Aviator aviator;
    public float Throttle
    {
        //get { return this.GetComponent<Animator>().GetFloat("Throttle"); }
        //set { this.GetComponent<Animator>().SetFloat("Throttle", value); }
        get;
        set;
    }

	void Start () 
    {
        aviator = this.transform.GetChild(0).gameObject.GetComponent<Aviator>();
	}

    public float landingHeight=0;


    internal override void Animate()
    {
   //     Throttle = this.gameObject.GetComponent<Movability>().Throttle;
        aviator.Aviate();

        this.transform.position += this.gameObject.GetComponent<Movability>().Speed * this.transform.forward;
        
    }

}
