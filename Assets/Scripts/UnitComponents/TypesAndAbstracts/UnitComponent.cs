using UnityEngine;
using System.Collections;



public abstract class UnitComponent : MonoBehaviour
{
    virtual public bool ComponentExtendsTheOptionalstateOrder
    {
        get { return false; }
    }

    abstract public string IDstring
    { get; }

    public UnitScript UNIT;
    private int ID;

    private System.Enum[] StateExtensions = new System.Enum[1];

    public UnitComponent PflongeOnUnit()
    {
        
        if (ComponentExtendsTheOptionalstateOrder)
        {
            throw new UnitComponentExeption(this.gameObject.GetInstanceID(), this.IDstring, "tryed Pflontshing on a 'UnitEtension'. It needs it's 'OPTIONS'-enum's Type as parameter !\nTry: 'PflongeOnUnit(typof(OPTIONS))'. !!!");
        }
        else
        {
            UNIT = this.gameObject.GetComponent<UnitScript>();
            StateExtensions[0] = EnumProvider.ORDERSLIST.Cancel;
            this.ID = UNIT.Options.RegisterUnitComponent(this, StateExtensions);

            SignIn();

            return this;
        }
    }

    public UnitComponent PflongeOnUnit(System.Type optionsType)
    {
        if (ComponentExtendsTheOptionalstateOrder)
        {
            UNIT = this.gameObject.GetComponent<UnitScript>();
            StateExtensions = new System.Enum[System.Enum.GetValues(optionsType).Length];
            System.Enum.GetValues(optionsType).CopyTo(StateExtensions, 0);
            this.ID = UNIT.Options.RegisterUnitComponent(this, StateExtensions);
            
            SignIn();
            
            return this;
        }
        else
        {
            return PflongeOnUnit();
        }
    }

    public UnitComponent PflongeOnUnit(System.Array newextensions)
    {
        if (ComponentExtendsTheOptionalstateOrder)
        {
            UNIT = this.gameObject.GetComponent<UnitScript>();
            StateExtensions = new System.Enum[newextensions.Length];
            newextensions.CopyTo(StateExtensions, 0);
            this.ID = UNIT.Options.RegisterUnitComponent(this, StateExtensions);

            SignIn();

            return this;
        }
        else
            return PflongeOnUnit();
    }

    protected virtual void SignIn()
    {
        UNIT.Options.PRIMARY_STATE_CHANGE += on_UnitStateChange;
    }
    protected virtual void SignOut()
    {
        UNIT.Options.PRIMARY_STATE_CHANGE -= on_UnitStateChange;
    }







    public void StlontshOff()
    {
        UNIT.Options.UnRegister(this.ID, StateExtensions);
        SignOut();
    }

    abstract protected EnumProvider.ORDERSLIST on_UnitStateChange(EnumProvider.ORDERSLIST stateorder);


    void OnDestroy()
    {
            StlontshOff();
    }

    abstract public void DoUpdate();


    public class UnitComponentExeption : System.Exception
    {
        public struct UnitExeptionData
        {
            public int ObjectID;
            public string UnitComponentID;
            public string ExeptionMessage;

            public UnitExeptionData(int oid, string ucid,string exeptionMessage)
            {
                ObjectID = oid;
                UnitComponentID = ucid;
                ExeptionMessage = exeptionMessage;
            }
        }

        UnitExeptionData ExeptionData;

        public UnitComponentExeption(int oid, string ucid,string message)
        {
            this.ExeptionData = new UnitExeptionData(oid, ucid, message);
        }
        public override string Message
        {
            get
            {
                string parentObject = "";
                foreach (GameObject unit in GameObject.FindGameObjectsWithTag("Units"))
                    if (unit.GetInstanceID() == this.ExeptionData.ObjectID)
                        parentObject = unit.name + " - ID: " + this.ExeptionData.ObjectID.ToString();

                return "UnitComponent-Exeption in:\n " + parentObject + "\nIn UnitComponent:\n" + ExeptionData.UnitComponentID+"\n..."+this.ExeptionData.ExeptionMessage;
            }
        }
    }
}
