using UnityEngine;
using System.Collections;

public class SwitchLight : MonoBehaviour
{
    [SerializeField]
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
        {
            this.light.enabled = OnOff;
            this.transform.parent.gameObject.SetActive(OnOff);          
        }
        else
        {
            this.light.enabled = false;
            this.transform.parent.gameObject.SetActive(false);
        }
    }


	

}
