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
    public delegate void Extensions_On_LeftClick(bool hold);
    public delegate void Extensions_On_RightClick(bool hold);
    public event Extension PRIMARY_STATE_CHANGE;
    public event Extensions_On_LeftClick Extensions_OnLEFTCLICK;
    public event Extensions_On_RightClick Extensions_OnRIGHTCLICK;

    protected Vector3? ActionPoint;
    public enum OPTIONS : int
    {
        SellectedOption = 0,
        Cancel=10000
    }

    public UnitScript UNIT;
    public BuildingSetting SettingFile;
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
                if (!UNIT.IsEnemy(Target))
                    CheckedAllert = value;
            }
            __targetunderattack = value;
        }
    }



    //-----------------------------------------------------Orders and Interaction...

    //- Standard oneclick Orders:

    virtual internal void FocussedLeftOnGround(Vector3 worldPoint)
    {
        if (standardOrder)
        {
            if (this.gameObject.GetComponent<Focus>())
            {
                if (Focus.IsLocked)
                {
                    if (gameObject.GetComponent<Focus>().Unlock(this.gameObject))
                        Component.Destroy(gameObject.GetComponent<Focus>());
                }
                else
                {
                    Component.Destroy(gameObject.GetComponent<Focus>());
                }
            }
        }
    }
    virtual internal void FocussedRightOnGround(Vector3 worldPoint)
    {
        if (standardOrder)
        {
            if (this.gameObject.GetComponent<Focus>())
            {
                if (Focus.IsLocked)
                {
                    if (gameObject.GetComponent<Focus>().Unlock(this.gameObject))
                        Component.Destroy(gameObject.GetComponent<Focus>());
                }
                else
                {
                    Component.Destroy(gameObject.GetComponent<Focus>());
                }
            }
        }
    }
    virtual internal void FocussedLeftOnEnemy(GameObject enemy)
    { enemy.AddComponent<Focus>(); }
    virtual internal void FocussedRightOnEnemy(GameObject enemy)
    { enemy.AddComponent<Focus>(); }
    virtual internal void FocussedLeftOnAllied(GameObject friend)
    { friend.AddComponent<Focus>(); }
    virtual internal void FocussedRightOnAllied(GameObject friend)
    { friend.AddComponent<Focus>(); }
    
    //- stateorders:
    [SerializeField]
    protected EnumProvider.ORDERSLIST baseUnitState;
    virtual public System.Enum UnitState
    {
        get { return baseUnitState; }
        set 
        {
            EnumProvider.ORDERSLIST order = (EnumProvider.ORDERSLIST)value;
            if ((PRIMARY_STATE_CHANGE!=null))
            {
                Debug.Log("STATE_CHANGE event triggerd");
                baseUnitState = PRIMARY_STATE_CHANGE(order);
            }
        }
    }

    //-- Menu functionality:_....
    protected int CurrentSIDEMENUoption = 0;
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

    //- ChainedOrders   ...  (not in use yet, and even not tested well...)
    //- will give posibillity to put several orders in one package.
    //- then they will be processed after each other and could be
    //- used for Units givin orders to other units,processing them as group e.t.c....
    protected ChainedOrders OrdersInMind = new ChainedOrders();
    protected bool GotToDoWhatGotToDo = false;
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
    virtual protected bool GotToDoPrimaryOrders
    { get; set; }





    //------------------------------------------------- Navigation..
    [SerializeField]
    private Vector3 moveToPoint = Vector3.zero;
    virtual public Vector3 MoveToPoint
    {
        get { return moveToPoint; }
        internal set { moveToPoint = value; }
    }

    abstract internal void MoveAsGroup(GameObject leader);

    
    public GameObject Target;



    //---------------------------------------- Enginal stuff and functions...
    protected SortedDictionary<int, string> OptionalStatesOrder = new SortedDictionary<int, string>();
    [SerializeField]
    protected List<string> optionalStateStrings = new List<string>();
    [SerializeField]
    protected int[] optionalstateIDs;
    [SerializeField]
    protected EnumProvider.ORDERSLIST[] optionalStates;
    
    void Start()
    {
        DoStart();
        optionalstateIDs = RefreshStatelist();
        UNIT = gameObject.GetComponent<UnitScript>();
    }

    protected void RegisterInheridedOrderStateOptions(System.Type optionsEnum)
    {
        foreach (int KeyValue in System.Enum.GetValues(optionsEnum))
            if (!OptionalStatesOrder.ContainsKey(KeyValue))
                OptionalStatesOrder.Add(KeyValue, ((EnumProvider.ORDERSLIST)KeyValue).ToString());
    }

    abstract internal void DoStart();
    abstract internal void DoUpdate();

    internal void OptionsUpdate()
    {
        //if (GotToDo != null)
        //{
        //    if (GotToDo.Value)
        //        GotToDo = ProcessAllOrders();

        DoUpdate();

        foreach (UnitComponent component in PluggedStateExtendingComponents.Values)
            component.DoUpdate();  
    }

    [SerializeField]
    internal GameObject[] ColliderContainingChildObjects = new GameObject[0];

    public Dictionary<int, UnitComponent> PluggedStateExtendingComponents = new Dictionary<int, UnitComponent>(0);
    private int[] RefreshStatelist()
    {
        int index = -1;
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
    public int RegisterUnitComponent(UnitComponent component, System.Enum[] stateExtensions)
    {
        bool add = true;
        int UCcompID = -1;
        if (PluggedStateExtendingComponents.ContainsValue(component))
        {
            foreach (int key in PluggedStateExtendingComponents.Keys)
            {
                if (component == PluggedStateExtendingComponents[key])
                {
                    UCcompID = key;
                    Component.Destroy(component);
                    return UCcompID;
                }
            } 
        }
        else
        {
            foreach (System.Enum extension in stateExtensions)
            {

                EnumProvider.ORDERSLIST KEY = (EnumProvider.ORDERSLIST)extension;
                if (KEY < EnumProvider.ORDERSLIST.Cancel)
                {
                    string VALUE = System.Enum.GetName(typeof(EnumProvider.ORDERSLIST), KEY);
                    if ((int)KEY > (int)EnumProvider.ORDERSLIST.Stay)
                        OptionalStatesOrder.Remove((int)KEY);
                    if (!OptionalStatesOrder.ContainsKey((int)KEY))
                        OptionalStatesOrder.Add((int)KEY, VALUE);
                }
                else
                    add = false;
            }
            if (add)
            {
                UCcompID = PluggedStateExtendingComponents.Count;
                PluggedStateExtendingComponents.Add(UCcompID,component);

                optionalstateIDs = RefreshStatelist();
            }
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
            PluggedStateExtendingComponents.Remove(componentID);
        
    }


    //- Focus-Handling..
    public bool HasFocus
    {
        get
        {
            return (this.gameObject.GetComponent<Focus>());

        }
    }
    public bool IsLockedOnFocus
    {
        get
        {
            return HasFocus && Focus.IsLocked;
        }
    }

    public bool LockOnFocus()
    {
        if (!IsLockedOnFocus)
        {
            if (!HasFocus)
                gameObject.AddComponent<Focus>();

            return gameObject.GetComponent<Focus>().Lock();
        }
        else return true;
    }

    public bool UnlockFocus()
    {
        if (IsLockedOnFocus)
        {
            return GetComponent<Focus>().Unlock(this.gameObject);
        }
        else return true;
    }
    public bool UnlockAndDestroyFocus()
    {
        if (UnlockFocus())
        {
            Component.Destroy(this.gameObject.GetComponent<Focus>());
            return true;
        }
        else return false;
    }


    internal void OptionsBase_LEFTCLICK(Ray qamRay, bool hold)
    {
        MouseEvents_LEFTCLICK(qamRay, hold);
        if (Extensions_OnLEFTCLICK != null)
            Extensions_OnLEFTCLICK(hold);
    }
    internal void OptionsBase_RIGHTCLICK(Ray qamRay, bool hold)
    {
        MouseEvents_RIGHTCLICK(qamRay, hold);
        if (Extensions_OnRIGHTCLICK != null)
            Extensions_OnRIGHTCLICK(hold);
    }

    virtual internal void MouseEvents_LEFTCLICK(Ray qamRay, bool hold)
    {

    }

    virtual internal void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold)
    {

    }


    //public void abstract_LEFTRELEASE()
    //{
    //    if (gameObject.GetComponent<Focus>())
    //    {
    //        gameObject.GetComponent<Focus>().Unlock(this.gameObject);

    //        if(!IsLockedOnFocus)
    //            Component.Destroy(gameObject.GetComponent<Focus>());
    //    }
    //}
}
