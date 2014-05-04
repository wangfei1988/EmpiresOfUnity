using UnityEngine;
using System.Collections;

public class ObjToCamera : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	    UpdateManager.OnUpdate += DoUpdate;
	}
	
	// Update is called once per frame
	void DoUpdate () {
        transform.up = Camera.main.transform.position - transform.position;
        transform.forward = -Camera.main.transform.up;
	}

}
