using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System;


abstract public class UnitOptions : MonoBehaviour
{

    protected SortedDictionary<int, string> OPTIONSlist = new SortedDictionary<int, string>();
    public enum OPTIONS : int
    {
        SellectedOption = 0,
        Cancel=10000
    }

    public UnitScript UNIT;

    //---------------------------------------- State Flags...
    
    public virtual bool IsAttacking
    { get { return false; } protected set { } }
    public bool IsUnderAttack
    { get; set; }
    public virtual bool IsMoving
    {
        get
        {
            return __moving;
        }
        protected set
        {
            if (!value) _groupmove = false;
            __moving = value;
        }
    }
    private bool __moving = false;
    public bool IsMovingAsGroup
    {
        get { return _groupmove; }
        protected set
        {
            if (value) IsMoving = true;
            _groupmove = value;
        }
    }
    private bool _groupmove = false;
    public bool IsGroupLeader;
    
    //-----------------------------------------------------Orders and Interaction...
    virtual internal string[] GetUnitsMenuOptions()
    {
    //    return System.Enum.GetNames(UnitState.GetType());
        string[] buffer = new string[OPTIONSlist.Count];
        int i = 0;
        foreach (KeyValuePair<int, string> entry in OPTIONSlist)
        {
            buffer[i] = entry.Value;
            i++;
        }
        return buffer;
    }
    virtual internal string[] GetUnitsSIDEMenuOptions()
    {
        if (UNIT.weapon.HasArsenal)
        {
            string[] weaponNames = new string[UNIT.weapon.arsenal];
            for (int i = 0; i < UNIT.weapon.arsenal; i++)
                weaponNames[i] = UNIT.weapon.arsenal[i].name;
            return weaponNames;
        }
        else return new string[0];
    }
    virtual public void GiveOrder(int orderNumber)
    {
        int i=0;
        foreach (int key in OPTIONSlist.Keys)
        {
            if (i == orderNumber)
                UnitState = (OPTIONS)key;
            i++;
        }
    }
    virtual public void SetSIDEOption(int SIDEoptionNumber)
    {
        if (UNIT.weapon.HasArsenal)
            UNIT.weapon.prefabSlot = UNIT.weapon.arsenal[SIDEoptionNumber];
    }
    protected bool standardOrder = false;

    abstract public System.Enum UnitState
    { get; set; }
    virtual internal void FocussedLeftOnGround(Vector3 worldPoint)
    {
        if (this.gameObject.GetComponent<Focus>())
        {
            if (Focus.IsLocked)
                gameObject.GetComponent<Focus>().Unlock(gameObject);

            Component.Destroy(gameObject.GetComponent<Focus>());
        }
    }
    virtual internal void FocussedRightOnGround(Vector3 worldPoint)
    {
        if (this.gameObject.GetComponent<Focus>())
        {
            if (Focus.IsLocked)
                gameObject.GetComponent<Focus>().Unlock(gameObject);

            Component.Destroy(gameObject.GetComponent<Focus>());
        }
    }
    virtual internal void FocussedLeftOnEnemy(GameObject enemy)
    { enemy.AddComponent<Focus>(); }
    virtual internal void FocussedRightOnEnemy(GameObject enemy)
    { }
    virtual internal void FocussedLeftOnAllied(GameObject friend)
    { friend.AddComponent<Focus>(); }
    virtual internal void FocussedRightOnAllied(GameObject friend)
    { }
    abstract internal void MoveAsGroup(GameObject leader);
    

    //------------------------------------------------- Navigation..
    [SerializeField]
    private Vector3 moveToPoint = Vector3.zero;
    virtual public Vector3 MoveToPoint
    {
        get { return moveToPoint; }
        protected set { moveToPoint = value; }
    }



    
    public GameObject Target;

    //public GameObject MoveToPointMarker;
    //public GameObject AttackPointMarker;
    //public GameObject WayPointMarker;

    //---------------------------------------- Enginal stuff and functions...
    void Start()
    {
        
        DoStart();
   //     OPTIONSlist.Add(10000,"Cancel");
        testlist.AddRange(OPTIONSlist.Values);
        UNIT = gameObject.GetComponent<UnitScript>();
    }
    public List<string> testlist = new List<string>();
    abstract internal void DoStart();
    abstract internal void DoUpdate();
    internal void OptionsUpdate()
    {
        DoUpdate();
    }

    internal Focus.HANDLING FocusFlag = Focus.HANDLING.None;
    protected bool IsLockedOnFocus
    {
        get
        {
            return FocusFlag==Focus.HANDLING.IsLocked;
        }
    }
    protected bool HasFocus
    {
        get
        {
            return ((FocusFlag == Focus.HANDLING.HasFocus) | IsLockedOnFocus);
        }
    }
    protected void LockOnFocus()
    {
        if (!IsLockedOnFocus)
        {
            if (!(FocusFlag == Focus.HANDLING.HasFocus))
                gameObject.AddComponent<Focus>();
            gameObject.GetComponent<Focus>().Lock(); 
        }
    }
    protected void UnlockFocus()
    {
        MouseEvents.LEFTRELEASE += abstract_LEFTRELEASE;
    }
    protected void UnlockFocus(Focus.HANDLING andDestroyIt)
    {
        if (IsLockedOnFocus)
        {
            UnlockFocus();
            if (andDestroyIt == Focus.HANDLING.DestroyFocus) FocusFlag = andDestroyIt;
        }
        else if (FocusFlag == Focus.HANDLING.HasFocus) Component.Destroy(gameObject.GetComponent<Focus>());
    }

    private void abstract_LEFTRELEASE()
    {
        gameObject.GetComponent<Focus>().Unlock(gameObject);
        if (FocusFlag<=0)
            Component.Destroy(gameObject.GetComponent<Focus>());
        MouseEvents.LEFTRELEASE -= abstract_LEFTRELEASE;
    }

    virtual protected void MouseEvents_LEFTCLICK(Ray qamRay, bool hold) { }
    virtual protected void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold) { }

    protected bool TargetIsEnemy(GameObject target)
    {
        return target.GetComponent<UnitScript>().GoodOrEvil != UNIT.GoodOrEvil;
    }
    protected bool TargetIsAllied(GameObject target)
    {
        if (target.GetInstanceID() != gameObject.GetInstanceID())
            return target.GetComponent<UnitScript>().GoodOrEvil == UNIT.GoodOrEvil;
        else return false;
    }
}
