using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Character/Unit Sqript")]
public class UnitScript : MonoBehaviour 
{
    public enum UNITTYPE : int
    {
        Tank,
        Airport,
        Fabrik,
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
    public UnitAnimation unitAnimator;
    public UNITTYPE unitType;
    public UnitOptions Options;
	void Start () 
    {
     //   gameObject.name = gameObject.name + " " + gameObject.GetInstanceID();
        switch (unitType)
        {
            case UNITTYPE.Tank:
                {
                    IsBuilding = false;
                    if (!gameObject.GetComponent<GroundUnitOptions>()) gameObject.AddComponent<GroundUnitOptions>();
                    Options = gameObject.GetComponent<GroundUnitOptions>();
                //    if (!gameObject.GetComponent<LightLaser>()) gameObject.AddComponent<LightLaser>();
                    weapon = gameObject.GetComponent<LightLaser>();
                    Options.SetUp(800, 0.2f);
                    break;
                }
            case UNITTYPE.Fabrik:
                {
                    if (gameObject.GetComponent<ProductionBuildingOptions>() == null) gameObject.AddComponent<BuildingOptions>();
                    Options = gameObject.GetComponent<ProductionBuildingOptions>();
                //    if (!gameObject.GetComponent<NoWeapon>()) gameObject.AddComponent<NoWeapon>();
                    weapon = gameObject.GetComponent<RocketLauncher>();
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
