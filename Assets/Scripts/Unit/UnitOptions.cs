using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System;


abstract public class UnitOptions : MonoBehaviour
{
    protected int CurrentSIDEMENUoption = 0;
    protected SortedDictionary<int, string> OPTIONSlist = new SortedDictionary<int, string>();
    protected Vector3? ActionPoint;
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
    private bool __targetunderattack = false;
    public bool TargetUnderAttack
    {
        get { return CheckedAllert = __targetunderattack; }
        set 
        {
            if (value)
            {
                if (!(UNIT.GoodOrEvil+Target.GetComponent<UnitScript>().GoodOrEvil))
                    CheckedAllert = value;
            }
            __targetunderattack = value;
        }
    }
    //-----------------------------------------------------Orders and Interaction...
    protected List<Orderble> ChainedOrders = new List<Orderble>();
    protected bool GotToDo = false;
    protected bool CheckedAllert
    {
        get { return IsUnderAttack | TargetUnderAttack; }
        set 
        {
            if (value)
            {
                UNIT.TriggerAllert(3);
            }
        }
    }
    protected virtual bool ProcessAllOrders()
    {
        ActionPoint = null;
        if((!CheckedAllert)&&(!GotToDoPrimaryOrders))
        {
            for (int i = ChainedOrders.Count-1; i >=0; i--)
			{
                
                standardOrder = true;
                UnitState = ChainedOrders[i].order;
                switch ((int)((EnumProvider.ORDERSLIST)UnitState))
                {
                    case 0:  //---------------------------  OneClick Vector3 Orders
                    case 1:
                    case 2:
                    case 3:
                        {
                            ActionPoint = ChainedOrders[i].Vector;
                            CurrentSIDEMENUoption = ChainedOrders[i].data.Value;
                            break; 
                        }
                    case 10: //---------------------------  MultiRightclick Orders;

                        break;
                    case 20:  //--------------------------  Other Unit Click Orders;
                        break;
                }
 
			}
            GotToDoWhatGotToDo = true;
        }
        return GotToDoWhatGotToDo;
    }

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
    abstract protected bool GotToDoPrimaryOrders
    { get;  set; }







    virtual internal void FocussedLeftOnGround(Vector3 worldPoint)
    {
        if (this.gameObject.GetComponent<Focus>())
        {
            if (Focus.IsLocked)
                gameObject.GetComponent<Focus>().Unlock(gameObject);


            // Remove Focus
            DestroyFocus();
        }
    }
    virtual internal void FocussedRightOnGround(Vector3 worldPoint)
    {
        if (this.gameObject.GetComponent<Focus>())
        {
            if (Focus.IsLocked)
                gameObject.GetComponent<Focus>().Unlock(gameObject);

            // Remove Focus
            DestroyFocus();
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
        //OPTIONSlist.Add(10000,"Cancel");
        testlist.AddRange(OPTIONSlist.Values);
        UNIT = gameObject.GetComponent<UnitScript>();
    }
    public List<string> testlist = new List<string>();
    abstract internal void DoStart();
    abstract internal void DoUpdate();
    internal void OptionsUpdate()
    {
        if (GotToDo) GotToDo = ProcessAllOrders();
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
            if (andDestroyIt == Focus.HANDLING.DestroyFocus)
                FocusFlag = andDestroyIt;
        }
        else if (FocusFlag == Focus.HANDLING.HasFocus)
        {
            // Remove Focus
            DestroyFocus();
        }
    }
    protected bool GotToDoWhatGotToDo = false;
    private void abstract_LEFTRELEASE()
    {
        if (gameObject.GetComponent<Focus>())
        {
            gameObject.GetComponent<Focus>().Unlock(gameObject);
            if (FocusFlag <= 0)
            {
                DestroyFocus();
            }
        }
        MouseEvents.LEFTRELEASE -= abstract_LEFTRELEASE;
    }

    virtual protected void MouseEvents_LEFTCLICK(Ray qamRay, bool hold) { }
    virtual protected void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold) { }

    protected bool TargetIsEnemy(GameObject target)
    {
        return target.gameObject.GetComponent<UnitScript>();
    }
    protected bool TargetIsAllied(GameObject target)
    {
        if (target.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
            return !TargetIsEnemy(target);
        else return false;
    }

    protected void DestroyFocus()
    {
        Component.Destroy(gameObject.GetComponent<Focus>());
        gameObject.gameObject.GetComponent<UnitScript>().HideLifebar();
    }
}
