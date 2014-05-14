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

        //--- All Flying Units:
        JetFighter = EnumProvider.UNITCLASS.AIR_UNIT + 1,
        JetWing,

        //--- All Non-Production Buildings:
        NaniteMine = EnumProvider.UNITCLASS.BUILDING + 1,
        MatterMine,
        SolarTower,

        //--- All Production-Buildings:
        Airport = EnumProvider.UNITCLASS.PRODUCTION_BUILDING + 1,
        Fabrik,
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
    public FoE.GOODorEVIL goodOrEvil;
    public FoE GoodOrEvil;

    public bool IsEnemy(FoE other)
    {
        return this.GoodOrEvil+other;
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
        switch (unitType)
        {
            case UNITTYPE.Worker:
                {
                    Options = gameObject.GetComponent<GroundBuilderOptions>();
                    weapon = gameObject.AddComponent<NoWeapon>();
                    break;
                }
            case UNITTYPE.Tank:
                {
                    Options = gameObject.GetComponent<GroundUnitOptions>();
                    weapon = gameObject.GetComponent<LightLaserGun>();
                    break;
                }
            case UNITTYPE.Fabrik:
                {
                    Options = gameObject.GetComponent<ProductionBuildingOptions>();
                    weapon = gameObject.AddComponent<NoWeapon>();
                    break;
                }
            case UNITTYPE.Airport:
                {
                    Options = gameObject.GetComponent<ProductionBuildingOptions>();
                    weapon = gameObject.AddComponent<NoWeapon>();
                    break;
                }
            case UNITTYPE.JetFighter:
                {
                    Options = gameObject.GetComponent<MovingUnitOptions>();
                    weapon = gameObject.GetComponent<RocketLauncher>();
                    this.gameObject.AddComponent<WingsAndJets>().PflongeOnUnit();
                    break;
                }
            case UNITTYPE.RocketMan:
                {
                    Options = gameObject.GetComponent<GroundUnitOptions>();
                    weapon = gameObject.GetComponent<RocketLauncher>();
                    break;
                }
            case UNITTYPE.MatterMine:
                {
                    Options = gameObject.GetComponent<MatterMine>();
                    weapon = gameObject.AddComponent<NoWeapon>();
                    break;
                }
            case UNITTYPE.NaniteMine:
                {
                    Options = gameObject.GetComponent<NaniteMine>();
                    weapon = gameObject.AddComponent<NoWeapon>();
                    break;
                }
            case UNITTYPE.SolarTower:
                {
                    Options = gameObject.GetComponent<SolarTower>();
                    weapon = gameObject.AddComponent<NoWeapon>();
                    break;
                }
        }
	}

    void Start()
    {
        UpdateManager.UNITUPDATE += UpdateManager_UNITUPDATE;
        UpdateManager.OnUpdate += UpdateLifebar;
    }

    void OnDestroy()
    {
        UpdateManager.UNITUPDATE -= UpdateManager_UNITUPDATE;
        UpdateManager.OnUpdate -= UpdateLifebar;
    }

    //--- Update function:  
    //--- the Main-Entrypoint to the Units GameObject UpdateLoop.
    //--- if everything is set correctly, It' should call all Updates in their
    //--- right Updateorder in all Subcomponents and Childobjects.
    //########################################################################################################
    void UpdateManager_UNITUPDATE()
    {
        if (unitAnimation)
            unitAnimation.DoUpdate();
        Options.OptionsUpdate();
    }

    //--- Component-Slots (these Components are accsessed almost everytime, so ithink References to them are very usefull...) 
    public UnitOptions Options; //-Contains everythin whats Optional... different on every Unit-Type
    public UnitAnimation unitAnimation; //- first UnitAnimation of the UnitAnimations-Chain
    public UnitWeapon weapon; //- if the Unit is a Type of Unit thats unable to fight(Constuction Units i.e.) a "NoWeapon"-component will addet automaticly... 

    // LIFE
    // Data Fields for properties that almost every Unit uses:
    //#############################################################
    [SerializeField]
    private int life;
    public int Life
    {
        get { return life; }
        private set
        {
            if (life != value)
            {
                if (value <= 0) Die();
                life = value;
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
            if (this.gameObject.transform.position != LifebarScript.Position)
                LifebarScript.Position = gameObject.transform.position;
            UpdateLifebarLife();
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
            LifebarScript.Position = gameObject.transform.position;
            LifebarScript.LifeDefault = this.Life;
            LifebarScript.Activated = true;
        }
    }
    /* LIFEBAR END */
    public void HideLifebar()
    {
        if (LifebarScript != null)
        {
            LifebarScript.Activated = false;
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


            for (int i = 0; i < count-1; i++)
            {
                CurrentOrder[i] = Options.GetUnitsMenuOptionIDs()[i];
            }
            CurrentOrder[count-1] = EnumProvider.ORDERSLIST.Upgrade;

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
        //todo: code for dieing (explosion etc.)
        UpdateManager.UNITUPDATE -= UpdateManager_UNITUPDATE;
        foreach (Component component in this.gameObject.GetComponents<Component>()) 
            Component.Destroy(component);
        GameObject.Destroy(this.gameObject);
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
            else return this.gameObject;
        }
        else return unit;
    }
}
