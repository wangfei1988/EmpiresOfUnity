using UnityEngine;
using System.Collections;

public abstract class UnitComponent : MonoBehaviour
{
    public enum OPTIONS : int
    {
        Cancel=EnumProvider.ORDERSLIST.Cancel
    }
    virtual public string IDstring
    {
        get { return "NoWeapon"; }
    }

    private int ID;
    protected bool ComponentExtendsTheOptionalstateOrder = false;
    private System.Enum[] StateExtensions;

    public UnitComponent PflongeOnUnit()
    {
            if (ComponentExtendsTheOptionalstateOrder)
            {
                int i = -1;
                StateExtensions = new System.Enum[System.Enum.GetNames(typeof(OPTIONS)).Length];
                foreach (System.Enum extension in System.Enum.GetValues(typeof(OPTIONS)))
                    StateExtensions[++i] = extension;
            }
            else
            {
                StateExtensions = new System.Enum[1];
                StateExtensions[0] = (System.Enum)OPTIONS.Cancel;
            }

            ID = this.gameObject.GetComponent<UnitScript>().Options.RegisterUnitComponent(this, StateExtensions);
            
            return this;
    }

    public UnitComponent PflongeOnUnit(System.Enum[] newextensions)
    {
        StateExtensions = newextensions;
        ID = this.gameObject.GetComponent<UnitScript>().Options.RegisterUnitComponent(this, newextensions);
        UnitOptions.PRIMARY_STATE_CHANGE += on_UnitStateChange;
        return this;
    }



    public void StlontshOff()
    {
        this.gameObject.GetComponent<UnitScript>().Options.UnRegister(ID, StateExtensions);
        UnitOptions.PRIMARY_STATE_CHANGE -= on_UnitStateChange;
    }

    virtual protected EnumProvider.ORDERSLIST on_UnitStateChange(EnumProvider.ORDERSLIST stateorder)
    {
        return stateorder;
    }

    void OnDestroy()
    {
        if (ComponentExtendsTheOptionalstateOrder)
            StlontshOff();
    }

    abstract public void DoUpdate();
}
