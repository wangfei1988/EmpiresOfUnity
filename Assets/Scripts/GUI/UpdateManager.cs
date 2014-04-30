using UnityEngine;
using System.Collections;

public class UpdateManager : MonoBehaviour {

    public delegate void UnitUpdates();
    public static event UnitUpdates UNITUPDATE;
	
	// Update is called once per frame
    internal void UpdateUnits()
    {
        if (UNITUPDATE != null) UNITUPDATE();
    }
}
