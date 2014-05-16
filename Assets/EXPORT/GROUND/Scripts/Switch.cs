using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour
{
    private int lightSwitchID;
    
    void Start()
    {
        Ground.SWITCH += Ground_SWITCH;
    }
    public void SetID(int id)
    {
        lightSwitchID = id;
    }

    void Ground_SWITCH(bool OnOff, int id)
    {
        if (lightSwitchID == id)
            this.light.enabled = OnOff;
        else
            this.light.enabled = false;
    }


	

}
