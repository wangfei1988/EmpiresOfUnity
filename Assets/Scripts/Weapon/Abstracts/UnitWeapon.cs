using UnityEngine;

abstract public class UnitWeapon : MonoBehaviour
{
    public UnitScript UNIT;
    public enum WEAPON : byte
    {
        None=0,
        RayGun,
        RocketLauncher,
        MachineGun
    }
    abstract public WEAPON ID
    { get; }
    public WeaponObject prefabSlot;
    [SerializeField]
    private bool hasArsenal = false;
    public bool HasArsenal
    {
        get { return hasArsenal; }
        set 
        {
            if(value!=hasArsenal)
            {
                if (value) 
                    this.gameObject.AddComponent<WeaponArsenal>();
                else 
                    Component.Destroy(this.gameObject.GetComponent<WeaponArsenal>());

                hasArsenal = value;
            }
    }
    }
    public WeaponArsenal arsenal
    {
        get 
        {
            if (HasArsenal) return this.gameObject.GetComponent<WeaponArsenal>();
            else return null;
        }
    }
    public abstract bool IsOutOfAmu
    { get; }

    //------------------------------------------------Fires the weapon...
    abstract public void Engage(Vector3 targetPoint);//    ... at any Point,
    abstract public void Engage(GameObject targetUnit);//   ...at an Object

    abstract public float GetMaximumRange();


    //------------------------------The Weapon's Updatefunction...
    abstract public void Reload();

}
