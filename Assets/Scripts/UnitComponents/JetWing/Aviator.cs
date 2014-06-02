using UnityEngine;
using System.Collections;

public class Aviator : MonoBehaviour 
{
    AirUnitOptions options;
    private Transform blib;
    private bool firstUpdate = true;
    public FaceDirection fd; 
    public float MAXIMUM_FLIGHT_HEIGHT = 15f;
    public float CYCLE = 0.5f;
    public float Z_ROTATION = 33;
    public bool faceDirection = false;
	void Start () 
    {
        options = this.transform.parent.gameObject.GetComponent<UnitScript>().Options as AirUnitOptions;
        blib = GetComponentInChildren<SpriteRenderer>().gameObject.transform;
        
	}
	

	internal void Aviate() 
    {
        if (firstUpdate)
        {
            options.gameObject.GetComponent<Pilot>().mySpace = options.ColliderContainingChildObjects[0].GetComponent<SphereCollider>();
            firstUpdate = false;
        }
        
        fd.IsActive = (options.movement.IsMoving);


        float throttle = options.movement.Throttle;
        
        Vector3 setter = this.transform.position;
        setter.y = MAXIMUM_FLIGHT_HEIGHT*2 - throttle * MAXIMUM_FLIGHT_HEIGHT;
        this.transform.position = setter;

        setter = this.transform.transform.eulerAngles;
        setter.z = (throttle * 45) - 45;
        this.transform.transform.eulerAngles = setter;

        setter = options.transform.eulerAngles;
        setter.y -= (CYCLE - throttle*CYCLE);
        setter.z = Z_ROTATION - throttle * Z_ROTATION;
        options.transform.eulerAngles = setter;

        BlibIt();
	}

    private void BlibIt()
    {
        blib.forward = Vector3.up;
    }
}
