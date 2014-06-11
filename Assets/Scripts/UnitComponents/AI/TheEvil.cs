using UnityEngine;
using System.Collections;

public class TheEvil : MonoBehaviour 
{


	void Start() 
    {
        this.transform.DetachChildren();
        UpdateManager.OnMouseUpdate+=UpdateManager_OnMouseUpdate;
	}

    void UpdateManager_OnMouseUpdate()
    {
        
    }

    void OnDestroy()
    {
        UpdateManager.OnMouseUpdate-=UpdateManager_OnMouseUpdate;
    }
}
