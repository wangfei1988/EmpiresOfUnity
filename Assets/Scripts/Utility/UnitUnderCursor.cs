using UnityEngine;
using System.Collections;

public class UnitUnderCursor
{//-------------------------------------------An Object that gives static accses to current hovered Unit-Object...
    public static GameObject gameObject;
    public static bool IsAUnit = false;
    private static int ID;
    public static UnitScript UNIT
    {
        get { return gameObject.GetComponent<UnitScript>(); }
    }

    

//------------------------------------Non-static stuff, used by an instance which is for updating the statics... 
    public void Set(GameObject unit)
    {
            gameObject = unit;
            if (unit.GetComponent<UnitScript>())
                IsAUnit = true;

            else
                IsAUnit = false;
    }


    public bool Changed(int instanceID)
    {
        int lastID = ID;
        if (gameObject != null)
        {
            ID = instanceID;
            return (ID != lastID);
        }
        else
        {
            return true;
        }

    }

    public UnitUnderCursor()
    {

    }


    public static implicit operator bool(UnitUnderCursor cast)
    {
        if (gameObject)
            return ((bool)gameObject.GetComponent<UnitScript>());
        else
            return false;
    }

    public static implicit operator UnitScript(UnitUnderCursor cast)
    {
        if (UnitUnderCursor.IsAUnit)
            return UnitUnderCursor.UNIT;
        else
            return null;
    }

    public static implicit operator GameObject(UnitUnderCursor cast)
    {
        return UnitUnderCursor.gameObject;
    }

}
