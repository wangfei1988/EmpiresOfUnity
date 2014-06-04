using UnityEngine;
using System.Collections;

public class Cheats : MonoBehaviour {

	void Start ()
	{
	    UpdateManager.OnUpdate += this.DoUpdate;
	}
	
	void DoUpdate () 
    {
	    if (Input.GetKey(KeyCode.L))
	    {
            if (Input.GetKeyDown(KeyCode.U))
            {
                ResourceManager.AddResouce(ResourceManager.Resource.NANITEN, 500);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                ResourceManager.AddResouce(ResourceManager.Resource.MATTER, 500);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                ResourceManager.AddResouce(ResourceManager.Resource.ENERGY, 500);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                ResourceManager.AddResouce(ResourceManager.Resource.LABORER, 500);
            }
	    }
        if (Input.GetKey(KeyCode.K))
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                ResourceManager.SubtractResouce(ResourceManager.Resource.NANITEN, 500);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                ResourceManager.SubtractResouce(ResourceManager.Resource.MATTER, 500);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                ResourceManager.SubtractResouce(ResourceManager.Resource.ENERGY, 500);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                ResourceManager.SubtractResouce(ResourceManager.Resource.LABORER, 500);
            }
        }
	}

    void OnDestroy()
    {
        UpdateManager.OnUpdate -= this.DoUpdate;
    }
}
