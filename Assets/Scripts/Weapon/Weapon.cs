using UnityEngine;
using System.Collections;

abstract public class Weapon : MonoBehaviour
{
    public class None : Weapon
    {
        public override void Engage(GameObject targetUnit)
        {

        }
        public override void Engage(Vector3 targetPoint)
        {

        }
        public override float GetMaximumRange()
        {
            return 0f;
        }
        public override void Reloade()
        {

        }
    }

    public class Status
    {
        public int RANGE=0;
        public int MAXIMUM_POWER=0;
        //-------------------------------   todo...
        public bool IsLoadet=false;
        //------------------------ ...
        public Status()
        { }

        public Status(int range, int maxP, bool loadet)
        {
            RANGE = range;
            MAXIMUM_POWER = maxP;
            IsLoadet = loadet;
        }
    }

    public enum WEAPON : byte
    {
        None=0,
        RayGun,
        RocketLauncher,
    }
    public WeaponObject prefabSlot;
    public bool HasArsenal = false;

    public WeaponArsenal arsenal
    {
        get {return this.gameObject.GetComponent<WeaponArsenal>(); }
    }

    abstract public void Engage(Vector3 targetPoint);
    abstract public void Engage(GameObject targetUnit);

    abstract public float GetMaximumRange();

    abstract public void Reloade();

}
