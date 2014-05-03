using UnityEngine;
using System.Collections;

public class UnitUnderCursor
{
    public static GameObject gameObject;
    public static Transform transform;
    public static UnitScript UNIT;




    public void Set(GameObject unit)
    {


        if (unit == null)
        {
            transform = null;
            UNIT = null;
            gameObject = null;
        }
        else
        {
            gameObject = unit;
            transform = unit.transform;
            UNIT = unit.GetComponent<UnitScript>();
        }
        
    }



    public bool Changed(int instanceID)
    {

        if (gameObject)
            return instanceID != gameObject.GetInstanceID();
        else return true;
            //else
            //    return instanceID != -2;
      
    }

    public UnitUnderCursor()
    {

    }






}
