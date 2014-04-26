using UnityEngine;
using System.Collections;

public class UpdateHandler : MonoBehaviour
{

    public delegate void UpdateEvent();
    public static event UpdateEvent OnUpdate;
	
	void Update()
    {
        DoEventStuff();
	}

    private void DoEventStuff()
    {
        if (OnUpdate != null)
        {
            OnUpdate();
        }
    }

    
}
