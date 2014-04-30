using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Character/Unit Sqript")]
public class UnitScript : MonoBehaviour 
{
    public UNITTYPE unitType;
    public enum UNITTYPE : int
    {
        Tank,
        Worker,
        RocketMan,
        Airport = 1000,
        Fabrik,
    }

    public enum GOODorEVIL : byte
    {
        Good = 0,
        Evil = 1
    }
    public GOODorEVIL GoodOrEvil;
    public Weapon weapon;
    public UnitAnimation unitAnimation;
    
    public bool IsBuilding
    {
        get
        {
            if ((int)unitType >= 1000)
                return true;
            else return false;
        }
    }


    public UnitOptions Options;
    public O OptionsAs<O>() where O : UnitOptions
    {
        return (Options as O);
    }
    public System.Type OptionTypeFor(UNITTYPE type) 
    {
        switch (type)
        {
            case UNITTYPE.Tank: return typeof(GroundUnitOptions);
            case UNITTYPE.Fabrik: return typeof(ProductionBuildingOptions);
            default: return Options.GetType();
        }
    }
	void Start () 
    {
     //   gameObject.name = gameObject.name + " " + gameObject.GetInstanceID();
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
                    weapon = gameObject.GetComponent<LightLaser>();
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
            case UNITTYPE.RocketMan:
                {
                    Options = gameObject.GetComponent<GroundUnitOptions>();
                    weapon = gameObject.GetComponent<RocketLauncher>();
                    break;
                }
        }
		//UpdateManager.OnUpdate += DoUpdate;
        UpdateManager.UNITUPDATE += UpdateManager_UNITUPDATE;
	}

    [SerializeField]
    private int life;
    public int Life
    {
        get { return life; }
        private set
        {
            if (life != value)
            {
                if (value < 0) Die();
                life = value;
            }
        }
    }
    [SerializeField]
    private float speed;
    public float Speed
    {
        get { return speed; }
        private set { speed = value; }
    }
    virtual public float AttackRange
    {
        get { return weapon.GetMaximumRange(); }
    }

    public Vector3 MovingDirection=Vector3.zero;

    public void Hit(int power)
    {
        Life -= power;
    }
    private void Die()
    {
        //todo: code for dieing (explosion etc.)
        UpdateManager.UNITUPDATE -= UpdateManager_UNITUPDATE;
        foreach (Component component in this.gameObject.GetComponents<Component>()) 
            Component.Destroy(component);
        GameObject.Destroy(this.gameObject);
	}

    void DoUpdate()
    {
        //if (unitAnimator) unitAnimator.DoUpdate();
        if (weapon) weapon.Reloade();
        Options.OptionsUpdate();

        if (life < 0)
			GameObject.Destroy(this.gameObject);
    }

    [SerializeField]
    private List<int> interactingUnits = new List<int>();
    public List<int> InteractingUnits
    {
        get { return interactingUnits; }
    }
    public GameObject SetInteracting(GameObject unit)
    {
        if (unit.GetComponent<UnitScript>().GoodOrEvil == this.GoodOrEvil)
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

    public void AskForOrder()
    {
        //RightClickMenu.PopUpGUI(this);
    }

    public string[] Orders
    {
        get;
        private set;
    }

    void UpdateManager_UNITUPDATE()
    {
        if (unitAnimation) unitAnimation.DoUpdate();
        weapon.Reloade();
        Options.OptionsUpdate();
    }
}
