using UnityEngine;
using System.Collections;

abstract public class Weapon : MonoBehaviour
{


    //public class Status
    //{
    //    public int RANGE=0;
    //    public int MAXIMUM_POWER=0;
    //    //-------------------------------   todo...
    //    public bool IsLoadet=false;
    //    //------------------------ ...
    //    public Status()
    //    { }

    //    public Status(int range, int maxP, bool loadet)
    //    {
    //        RANGE = range;
    //        MAXIMUM_POWER = maxP;
    //        IsLoadet = loadet;
    //    }
    //}

    public enum WEAPON : byte
    {
        None=0,
        RayGun,
        RocketLauncher,
    }
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
                if (value) this.gameObject.AddComponent<WeaponArsenal>();
                else Component.Destroy(this.gameObject.GetComponent<WeaponArsenal>());
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

    abstract public void Engage(Vector3 targetPoint);
    abstract public void Engage(GameObject targetUnit);

    abstract public float GetMaximumRange();

    abstract public void Reloade();

}
