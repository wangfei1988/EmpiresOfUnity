using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour 
{

	void Start ()
	{
	    UpdateManager.OnUpdate += this.DoUpdate;
	}

    void Teleport()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Clickable")
        {
            other.transform.Translate(765, 0, 0);
        }
    }
	
	void DoUpdate () 
    {
	    
	}

    void OnDestroy()
    {
        UpdateManager.OnUpdate -= this.DoUpdate;
    }
}
