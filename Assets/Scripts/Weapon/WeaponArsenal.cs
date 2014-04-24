using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponArsenal : MonoBehaviour 
{
    [SerializeField]
    private List<WeaponObject> Arsenal = new List<WeaponObject>();
    public Weapon weapon
    {
        get { return this.gameObject.GetComponent<UnitScript>().weapon; }
    }
    public bool HasArsenal
    {
        get { return weapon.HasArsenal; }
        set { weapon.HasArsenal = value; }
    }

    public WeaponObject this[int index]
    {
        get { return Arsenal[index]; }
        set { Arsenal[index] = value; }
    }
    public int Count
    {
        get { return Arsenal.Count; }
    }
    public static implicit operator int(WeaponArsenal cast)
    {
        return cast.Arsenal.Count;
    }
    void Start () 
    {
        HasArsenal = true;
	}
	
    void OnDestroy()
    { HasArsenal = false; }


}
