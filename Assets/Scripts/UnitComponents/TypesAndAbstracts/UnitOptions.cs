using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System;


abstract public class UnitOptions : MonoBehaviour
{
    abstract public EnumProvider.UNITCLASS UNIT_CLASS
    {
        get;
    }
    public delegate EnumProvider.ORDERSLIST Extension(EnumProvider.ORDERSLIST selection);
    public static event Extension PRIMARY_STATE_CHANGE;
    protected int CurrentSIDEMENUoption = 0;

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



 


    protected EnumProvider.ORDERSLIST baseUnitState;
    virtual public System.Enum UnitState
    {
        get { return baseUnitState; }
        set 
        {
            EnumProvider.ORDERSLIST order = (EnumProvider.ORDERSLIST)value;
            if ((PRIMARY_STATE_CHANGE!=null)&&(order != baseUnitState))
            {

                baseUnitState = PRIMARY_STATE_CHANGE(order);
            }
        }
    }
    //-----------------------------------------------------Orders and Interaction...
    protected ChainedOrders OrdersInMind = new ChainedOrders();
    private int UOID = -1;
    public void GiveChainedOrder(EnumProvider.ORDERSLIST[] orders)
    {
        foreach (EnumProvider.ORDERSLIST order in orders)
            OrdersInMind.AddOrder(order, Vector3.zero, ++UOID);
    }
    protected bool? GotToDo = false;
    protected bool CheckedAllert
    {
        get { return IsUnderAttack | TargetUnderAttack; }
        set 
        {
            if (value)
            {
                UNIT.TriggerAllert(++UNIT.ALARM);
            }
        }
    }
    protected virtual bool ProcessAllOrders()
    {
        ActionPoint = null;
        if((!CheckedAllert)&&(!GotToDoPrimaryOrders))
        {
            while (!OrdersInMind.AllDone)
            {
                standardOrder = true;
                UnitState = OrdersInMind.Next().order;
                switch ((int)((EnumProvider.ORDERSLIST)UnitState))
                {
                    case 0:  //---------------------------  OneClick Vector3 Orders
                    case 1:
                    case 2:
                    case 3:
                        {
                            ActionPoint = OrdersInMind.Current().Vector;
                            CurrentSIDEMENUoption = OrdersInMind.Current().data;
                            break;
                        }
                    case 10: //---------------------------  MultiRightclick Orders;

                        break;
                    case 20:  //--------------------------  Other Unit Click Orders;
                        break;
                }
            }
            //for (int i = ChainedOrders.Count-1; i >=0; i--)
            //{
                
            //    standardOrder = true;
            //    UnitState = ChainedOrders[i].order;
            //    switch ((int)((EnumProvider.ORDERSLIST)UnitState))
            //    {
            //        case 0:  //---------------------------  OneClick Vector3 Orders
            //        case 1:
            //        case 2:
            //        case 3:
            //            {
            //                ActionPoint = ChainedOrders[i].Vector;
            //                CurrentSIDEMENUoption = ChainedOrders[i].data.Value;
            //                break;
            //            }
            //        case 10: //---------------------------  MultiRightclick Orders;

            //            break;
            //        case 20:  //--------------------------  Other Unit Click Orders;
            //            break;
            //    }
            //    ChainedOrders.RemoveAt(i);
 
            //}
            GotToDoWhatGotToDo = true;
        }
        return GotToDoWhatGotToDo;
    }

    virtual internal string[] GetUnitsMenuOptions()
    {
        string[] buffer = new string[optionalstateIDs.Length];
        for (int i = 0; i < optionalstateIDs.Length; i++)
            buffer[i] = OptionalStatesOrder[optionalstateIDs[i]];

        return buffer;
    }
    virtual internal EnumProvider.ORDERSLIST[] GetUnitsMenuOptionIDs()
    {
        return optionalStates;
    }
    virtual internal Object[] GetUnitsSIDEMenuObjects()
    {
        if (UNIT.weapon.HasArsenal)
        {
            Object[] objectArray = new Object[UNIT.weapon.arsenal];
            for (int i = 0; i < UNIT.weapon.arsenal; i++)
                objectArray[i] = UNIT.weapon.arsenal[i];
            return objectArray;
        }
        else return new Object[0];
    }
    virtual public void GiveOrder(int orderNumber)
    {
           UnitState = (EnumProvider.ORDERSLIST)optionalstateIDs[orderNumber];
    }
    virtual internal void GiveOrder(EnumProvider.ORDERSLIST order)
    {
        UnitState = order;
    }
    virtual public void SetSIDEOption(int SIDEoptionNumber)
    {
        if (UNIT.weapon.HasArsenal)
            UNIT.weapon.prefabSlot = UNIT.weapon.arsenal[SIDEoptionNumber];
    }
    virtual internal void SetSIDEObject(Object returned)
    {
        if (UNIT.weapon.HasArsenal)
            UNIT.weapon.prefabSlot = returned as WeaponObject;
    }
    protected bool standardOrder = false;


    virtual protected bool GotToDoPrimaryOrders
    { get;  set; }







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
    protected SortedDictionary<int, string> OptionalStatesOrder = new SortedDictionary<int, string>();
    protected List<string> optionalStateStrings = new List<string>();
    protected int[] optionalstateIDs;
    protected EnumProvider.ORDERSLIST[] optionalStates;
    
    void Start()
    {
        DoStart();
        optionalstateIDs = RefreshStatelist();
        UNIT = gameObject.GetComponent<UnitScript>();
    }

    private int[] RefreshStatelist()
    {
        int index= -1;
        int[] keylist = new int[OptionalStatesOrder.Count];


        optionalStates = new EnumProvider.ORDERSLIST[OptionalStatesOrder.Count];
        optionalStateStrings.Clear();
        foreach (KeyValuePair<int, string> entry in OptionalStatesOrder)
        {
            keylist[++index] = entry.Key;
            optionalStates[index] = (EnumProvider.ORDERSLIST)entry.Key;
            optionalStateStrings.Add(entry.Value);
        }
        return keylist;
    }

    abstract internal void DoStart();
    abstract internal void DoUpdate();
    internal void OptionsUpdate()
    {
        if (GotToDo != null)
        {
            if (GotToDo.Value)
                GotToDo = ProcessAllOrders();
        }
        DoUpdate();
    }

    public int RegisterUnitComponent(UnitComponent component, System.Enum[] stateExtensions)
    {
        bool add = true;
        int UCcompID = -1;
        if (PluggedStateExtendingComponents.Contains(component))
        {
            UCcompID = PluggedStateExtendingComponents.IndexOf(component);
            Component.Destroy(component);
            return UCcompID;
        }
        foreach(System.Enum extension in stateExtensions)
        {

            EnumProvider.ORDERSLIST KEY = (EnumProvider.ORDERSLIST)extension;
            if (KEY < EnumProvider.ORDERSLIST.Cancel)
            {
                string VALUE = System.Enum.GetName(typeof(EnumProvider.ORDERSLIST), extension);
                if (KEY > EnumProvider.ORDERSLIST.Stay)
                    OptionalStatesOrder.Remove((int)KEY);
                if (!OptionalStatesOrder.ContainsKey((int)KEY))
                    OptionalStatesOrder.Add((int)KEY, VALUE);
            }
            else
                add = false;
        }
        if (add)
        {
            if (PluggedStateExtendingComponents == null) 
                PluggedStateExtendingComponents = new List<UnitComponent>();
            else
            {
                UCcompID = PluggedStateExtendingComponents.Count;
                PluggedStateExtendingComponents.Add(component);
            }
            optionalstateIDs = RefreshStatelist();
        }
        return UCcompID; 
    }
    public void UnRegister(int componentID,System.Enum[] extensions)
    {
        bool NeedRefreshList = true;
        for (int i = extensions.Length-1; i >= 0 ; i--)
        {
            EnumProvider.ORDERSLIST KEY = (EnumProvider.ORDERSLIST)extensions[i];
            //string VALUE = System.Enum.GetName(typeof(EnumProvider.ORDERSLIST), extensions[i]);
            if (KEY != EnumProvider.ORDERSLIST.Cancel)
            {
                OptionalStatesOrder.Remove((int)KEY);
                if (KEY > EnumProvider.ORDERSLIST.Stay)
                {
                    if (UNIT.IsABuilding)
                        OptionalStatesOrder.Add((int)EnumProvider.ORDERSLIST.StopProduction, System.Enum.GetNames(typeof(EnumProvider.ORDERSLIST))[(int)EnumProvider.ORDERSLIST.StopProduction]);
                    else
                        OptionalStatesOrder.Add((int)EnumProvider.ORDERSLIST.Stay, System.Enum.GetNames(typeof(EnumProvider.ORDERSLIST))[(int)EnumProvider.ORDERSLIST.Stay]);
                }
            }
            else
                NeedRefreshList = false;  
        }
        if (NeedRefreshList)
            optionalstateIDs = RefreshStatelist();
        if (componentID >= 0)
            PluggedStateExtendingComponents.RemoveAt(componentID);
    }
    public List<UnitComponent> PluggedStateExtendingComponents;

    internal Focus.HANDLING FocusFlag = Focus.HANDLING.None;
    public bool IsLockedOnFocus
    {
        get
        {
            return FocusFlag==Focus.HANDLING.IsLocked;
        }
    }
    public bool HasFocus
    {
        get
        {
            return ((FocusFlag == Focus.HANDLING.HasFocus) | IsLockedOnFocus);
        }
    }
    public void LockOnFocus()
    {
        if (!IsLockedOnFocus)
        {
            if (!(FocusFlag == Focus.HANDLING.HasFocus))
                gameObject.AddComponent<Focus>();
            gameObject.GetComponent<Focus>().Lock(); 
        }
    }
    public void UnlockFocus()
    {
        MouseEvents.LEFTRELEASE += abstract_LEFTRELEASE;
    }
    public void UnlockFocus(Focus.HANDLING andDestroyIt)
    {
        if (IsLockedOnFocus)
        {
            UnlockFocus();
            if (andDestroyIt == Focus.HANDLING.DestroyFocus) FocusFlag = andDestroyIt;
        }
        else if (FocusFlag == Focus.HANDLING.HasFocus) Component.Destroy(gameObject.GetComponent<Focus>());
    }
    protected bool GotToDoWhatGotToDo = false;
    public void abstract_LEFTRELEASE()
    {
        if (gameObject.GetComponent<Focus>())
        {
            gameObject.GetComponent<Focus>().Unlock(gameObject);
            if (FocusFlag <= 0)
                Component.Destroy(gameObject.GetComponent<Focus>());
        }
        MouseEvents.LEFTRELEASE -= abstract_LEFTRELEASE;
    }

    virtual protected void MouseEvents_LEFTCLICK(Ray qamRay, bool hold) { }
    virtual protected void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold) { }


}
