using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitDestructionManagement : MonoBehaviour 
{

    public static List<GameObject> Removals = new List<GameObject>();
	void Start () 
    {
        UpdateManager.GUIUPDATE += UpdateManager_GUIUPDATE;
	}

    void UpdateManager_GUIUPDATE()
    {
        if (Removals.Count > 0)
            for (int i = Removals.Count-1; i >= 0; i--)
                 GameObject.Destroy(Removals[i]);
        Removals.Clear();
    }

    public static void SignInForDestruction(GameObject unit)
    {
        Removals.Add(unit);
    }
}
