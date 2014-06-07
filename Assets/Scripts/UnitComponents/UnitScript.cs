using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Program-X/UNIT/Unit Script (Unit's Main Component)")]
public class UnitScript : MonoBehaviour 
{

    //--- Type-Handling stuff:
    //###############################################################################
    public enum UNITTYPE : int  //-The List Of ALL Unit-Types in Game
    {
        //--- All Ground Units:
        Tank = EnumProvider.UNITCLASS.GROUND_UNIT + 1,
        RocketMan,

        //--- All Construction Units:
        Worker = EnumProvider.UNITCLASS.CONSTRUCTION_UNIT + 1,
        Medic,
        //--- All Flying Units:
        JetFighter = EnumProvider.UNITCLASS.AIR_UNIT + 1,
        JetWing,

        //--- All Non-Production Buildings:
        NaniteMine = EnumProvider.UNITCLASS.BUILDING + 1,
        MatterMine,
        SolarTower,
        LivingHouse,

        //--- All Production-Buildings:
        Airport = EnumProvider.UNITCLASS.PRODUCTION_BUILDING + 1,
        Fabrik,
        MainBuilding,
    }
    public UNITTYPE unitType;

    public bool IsABuilding
    {
        get
        {
            return unitType > (UNITTYPE)EnumProvider.UNITCLASS.BUILDING;
        }
    }

    public bool IsAnAirUnit
    {
        get 
        {
            return ((unitType > (UNITTYPE)EnumProvider.UNITCLASS.AIR_UNIT) & ((int)unitType < ((int)EnumProvider.UNITCLASS.AIR_UNIT)+100)); 
        }
    }

    // Friend or Enemy
    //###############################################################################
    [SerializeField]
    private FoE.GOODorEVIL goodOrEvil;
    public FoE GoodOrEvil;

    public bool IsEnemy(FoE other)
    {
        return this.GoodOrEvil+other;
    }
    public bool IsEnemy(GameObject other)
    {
        if (other.GetComponent<UnitScript>())
            return IsEnemy(other.GetComponent<UnitScript>().GoodOrEvil);
        else return false;
    }
    public bool IsEnemy(UnitScript other)
    {
        return IsEnemy(other.GoodOrEvil);
    }
    
    public bool IsMySelf(GameObject target)
    {
        return (target.gameObject.GetInstanceID() == this.gameObject.GetInstanceID());
    }
    
    public bool IsAllied(GameObject target)
    {
        if (!IsMySelf(target))
            return !IsEnemy(target.GetComponent<UnitScript>().GoodOrEvil);
        return false;
    }
    public bool IsAllied(UnitScript target)
    {
        if (!IsMySelf(target.gameObject))
            return !IsEnemy(target);
        return false;
    }


    // Alarm-System:
    //###############################################################################
    public enum ALLERT_LEVEL : int
    {
        GREEN = 0,
        a,
        b,
        c,
        d,
        A,
        B,
        C,
        D,
        RED
    }

    public ALLERT_LEVEL ALARM = ALLERT_LEVEL.GREEN;
    public delegate void Allerter(ALLERT_LEVEL allertLevel);
    public event Allerter MAIN_ALLERT;
    public event Allerter GROUP_ALLERT;
    public void TriggerAllert(ALLERT_LEVEL allertLevel)
    {
        if (allertLevel > ALLERT_LEVEL.d)
        { if (MAIN_ALLERT != null) MAIN_ALLERT(allertLevel); }
        else if (allertLevel < ALLERT_LEVEL.A)
        { if (GROUP_ALLERT != null) GROUP_ALLERT(allertLevel); }
    }

    private bool isUnderAttack = false;
    public bool IsUnderAttack
    {
        get { return isUnderAttack; }
        set
        {
            if (value && !isUnderAttack && MAIN_ALLERT != null)
                MAIN_ALLERT(++ALARM);
            isUnderAttack = value;
        }
    }


    //--- Startup and Main-functionality:
    //###############################################################################
	void Awake() 
    {
        GoodOrEvil = new FoE(goodOrEvil);
        LifebarScript = ScriptableObject.CreateInstance<Lifebar>();
        Options = gameObject.GetComponent<UnitOptions>();
        if (!gameObject.GetComponentInChildren<UnitWeapon>())
            weapon = gameObject.AddComponent<NoWeapon>();
        else
            weapon = gameObject.GetComponentInChildren<UnitWeapon>();
	}

    void Start()
    {
        this.DefaultLife = Life;
        UpdateManager.UNITUPDATE += UpdateManager_UNITUPDATE;
    }

    void OnDestroy()
    {
        UpdateManager.UNITUPDATE -= UpdateManager_UNITUPDATE;
    }

    // Update function:  
    // the Main-Entrypoint to the Units GameObject UpdateLoop.
    // if everything is set correctly, It' should call all Updates in their
    // right Updateorder in all Subcomponents and Childobjects.
    //########################################################################################################
    void UpdateManager_UNITUPDATE()
    {
        if (unitAnimation)
            unitAnimation.DoUpdate();
        Options.OptionsUpdate();
    }

    // Component-Slots (these Components are accsessed almost everytime, so ithink References to them are very usefull...) 
    public UnitOptions Options; //-Contains everythin whats Optional... different on every Unit-Type
    public UnitAnimation unitAnimation; //- first UnitAnimation of the UnitAnimations-Chain
    public UnitWeapon weapon; //- if the Unit is a Type of Unit thats unable to fight(Constuction Units i.e.) a "NoWeapon"-component will addet automaticly... 
    

    // LIFE
    // Data Fields for properties that almost every Unit uses:
    //#############################################################
    [SerializeField]
    private int life;
    private int DefaultLife;
    public int Life
    {
        get { return life; }
        private set
        {
            if (life != value)
            {
                life = value;
                if (life <= 0)
                    Die();
            }
        }
    }

    // LIFEBAR
    //#############################################################

    /* LIFEBAR UPDATE */
    private Lifebar LifebarScript;
    public void UpdateLifebar()
    {
        if (LifebarScript != null)
        {
            //if (Vector3.Distance(this.gameObject.transform.position, LifebarScript.Position) > 0.1f)
            // TODO Here check if Building / Unit moved manuelly (one line above) OR Camera / Map Changed (must build it)
            if(true)
            {
                LifebarScript.Position = gameObject.transform.position;
            }
        }
    }
    private void UpdateLifebarLife()
    {
        if (LifebarScript != null)
        {
            LifebarScript.Life = this.Life;
        }
    }
    /* LIFEBAR START */
    public void ShowLifebar()
    {
        if (LifebarScript != null)
        {
            LifebarScript.LifeDefault = this.DefaultLife;
            LifebarScript.Life = this.Life;
            if (GoodOrEvil == FoE.GOODorEVIL.Evil)
                LifebarScript.IsEnemy = true;
            LifebarScript.Activated = true;
            LifebarScript.Position = gameObject.transform.position;
            UpdateManager.OnUpdate += UpdateLifebar;
        }
    }
    /* LIFEBAR END */
    public void HideLifebar()
    {
        if (LifebarScript != null)
        {
            LifebarScript.Activated = false;
            UpdateManager.OnUpdate -= UpdateLifebar;
        }
    }

    [SerializeField]
    private int level;
    public int Level
    {
        get { return level; }
        private set 
        {
            if (value != level)
                level = value; 
        }
    }

    [SerializeField]
    private float sightWidth;
    public float SightWidth
    {
        get { return sightWidth; }
        set { sightWidth = value; }
    }
    
    [SerializeField]
    private float resourceFactor;
    public float ResourceFactor
    {
        get
        {
            return resourceFactor;
        }
       private set
        {
            resourceFactor = value;
        }
    }

    public float AttackRange
    {
        get { return weapon.GetMaximumRange(); }
    }

    public string[] RightClickMenuOptions
    {
        get { return Options.GetUnitsMenuOptions(); }
    }

    public EnumProvider.ORDERSLIST[] RightClickMenuOptionStates
    {
        get
        {
            int count = Options.GetUnitsMenuOptionIDs().Length;
            EnumProvider.ORDERSLIST[] CurrentOrder = new EnumProvider.ORDERSLIST[++count];

            for (int i = 0; i < count - 1; i++)
            {
                CurrentOrder[i] = Options.GetUnitsMenuOptionIDs()[i];
            }
            CurrentOrder[count - 1] = EnumProvider.ORDERSLIST.Upgrade;
            return Options.GetUnitsMenuOptionIDs();

        }
        set
        {
            if (value.Length == 1)
                Options.GiveOrder(value[0]);
            else
                Options.GiveChainedOrder(value);
        }
    }

    public Object[] SellectableObjects
    {
        get { return Options.GetUnitsSIDEMenuObjects(); }
    }

    public void Sellect(Object returned)
    {
        Options.SetSIDEObject(returned);
    }

    public void AskForOrder()
    {
        RightClickMenu.PopUpGUI(this);
    }
    
    public void Hit(int power)
    {
        // calld if The Unit is hitten 
        IsUnderAttack = true;
        Life -= power;
        this.UpdateLifebarLife();
    }
    
    private void Die()
    {
        UpdateManager.UNITUPDATE -= UpdateManager_UNITUPDATE;
        HideLifebar();

        // Explosion does the ExplosionsManager...
        StaticExploader.Exploade(1, this.transform.position);

        //Destruction now does the UnitDestructionsManagement...
        UnitDestructionManagement.SignInForDestruction(this.gameObject);
	}

    //--- Stuff for interaction with other Units like Guarding,Seeking,GroupMove and every other kind of Groupbehaviour...
    //#####################################################################################################################
    [SerializeField]
    private List<int> interactingUnits = new List<int>();
    public List<int> InteractingUnits
    {
        get { return interactingUnits; }
    }

    public GameObject SetInteracting(GameObject unit)
    {
        if (IsAllied(unit))
        {
            if (!interactingUnits.Contains(unit.gameObject.GetInstanceID()))
            {
                interactingUnits.Add(unit.gameObject.GetInstanceID());
                return unit.GetComponent<UnitScript>().SetInteracting(this.gameObject);
            }
            return this.gameObject;
        }
        return unit;
    }

}
