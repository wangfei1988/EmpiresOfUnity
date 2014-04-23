using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Character/Unit Sqript")]
public class UnitSqript : MonoBehaviour 
{
    public enum UNITTYPE : int
    {
        Tank,
        GroundUnit,
        GroundBuilder,
        GroundHarvester,
        Airport,
        Building,
        Flaptbrik,
        SpzialUnit
    }

    public int life;
    public enum GOODorEVIL : byte
    {
        Good = 0,
        Evil = 1
    }
    public GOODorEVIL GoodOrEvil;
    public bool IsBuilding;
    public Weapon weapon;
    public AnimaQuion unitAnimator;
    public UNITTYPE unitType;
    public UnitQptions Options;
	void Start () 
    {
     //   gameObject.name = gameObject.name + " " + gameObject.GetInstanceID();
        switch (unitType)
        {
            case UNITTYPE.GroundUnit:
                {
                    IsBuilding = false;
                    if (!gameObject.GetComponent<GroundUnitOptions>()) gameObject.AddComponent<GroundUnitOptions>();
                    Options = gameObject.GetComponent<GroundUnitOptions>();
               //     if (!gameObject.GetComponent<LightLaser>()) gameObject.AddComponent<LightLaser>();
              //      weapon = gameObject.GetComponent<LightLaser>();
                    Options.SetUp(2000, 0.9f);
                    
                    
                    break;
                }
            case UNITTYPE.Tank:
                {
                    IsBuilding = false;
                    if (!gameObject.GetComponent<GroundUnitOptions>()) gameObject.AddComponent<GroundUnitOptions>();
                    Options = gameObject.GetComponent<GroundUnitOptions>();
                //    if (!gameObject.GetComponent<LightLaser>()) gameObject.AddComponent<LightLaser>();
                    weapon = gameObject.GetComponent<LightLaser>();
                    Options.SetUp(800, 1.2f);
                    break;
                }
            case UNITTYPE.Building:
                {
                    if (gameObject.GetComponent<BuildingOptions>() == null) gameObject.AddComponent<BuildingOptions>();
                    Options = gameObject.GetComponent<BuildingOptions>();
                //    if (!gameObject.GetComponent<NoWeapon>()) gameObject.AddComponent<NoWeapon>();
                    weapon = gameObject.GetComponent<NoWeapon>();
                    IsBuilding = true;
                    Options.SetUp(20000, 0f);
                    break;
                }
            case UNITTYPE.Airport:
                {
                    if (gameObject.GetComponent<ProductionBuildingOptions>() == null) gameObject.AddComponent<ProductionBuildingOptions>();
                    Options = gameObject.GetComponent<ProductionBuildingOptions>();
                //    if (!gameObject.GetComponent<NoWeapon>()) gameObject.AddComponent<NoWeapon>();
                    weapon = gameObject.GetComponent<RocketLauncher>();
                    IsBuilding = true;
                    Options.SetUp(20000, 0f);
                    break;
                }
        }
  
        
	}

    [SerializeField]
    private List<int> interactingUnits = new List<int>();
    public List<int> InteractingUnits
    {
        get { return interactingUnits; }
    }
    public GameObject SetInteracting(GameObject unit)
    {
        if (unit.GetComponent<UnitSqript>().GoodOrEvil == this.GoodOrEvil)
        {
            if (!interactingUnits.Contains(unit.gameObject.GetInstanceID()))
            {
                interactingUnits.Add(unit.gameObject.GetInstanceID());
                return unit.GetComponent<UnitSqript>().SetInteracting(this.gameObject);
            }
            else return this.gameObject;
        }
        else return unit;
    }

    public void AskForOrder()
    {
        RightClickMenu.PopUpGUI(this);
    }

    public string[] Orders
    {
        get;
        private set;
    }

	void Update () 
    {
        if (unitAnimator) unitAnimator.DoUpdate();
        if (weapon) weapon.Reloade();
        Options.OptionsUpdate();
        life = Options.Life;
        if (life < 0) GameObject.Destroy(this.gameObject);
	}
}
