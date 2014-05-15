#define TERRAIN 
using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour
{
    public delegate void LightSwitch(bool OnOff,int lightID);
    public static event LightSwitch SWITCH;

    private const int MINIMUM_NUMBER_OF_GROUNGS = 3;

    public static GameObject masterGround;
    public static GroundLayer Current;
    private GameObject[] grounds = new GameObject[3];
    private bool NeedsUpdate = true;
    private static int GroundIndex = 0;
    [SerializeField]
    private GameObject GroundPreFab;
    [SerializeField]
    private int numberOfGrounds;



    public static int NumberOfGrounds
    {
        get;
        private set;
    }
    private static float GroundOffset()
    {
        return (Current.groundSize.x * 1.5f) * GroundIndex;
    }


    void Awake()
    {
        masterGround = grounds[0] = this.gameObject.transform.FindChild("SubGround0").gameObject;
        grounds[1] = this.gameObject.transform.FindChild("SubGround1").gameObject;
        grounds[2] = this.gameObject.transform.FindChild("SubGround2").gameObject;
        GroundIndex = 0;
    }


    void Start()
    {
        Current = masterGround.GetComponent<GroundLayer>();
        int count = 0;
        foreach (Switch lightswitch in this.gameObject.GetComponentsInChildren<Switch>())
        {
            lightswitch.SetID(count++);
        }
        this.gameObject.transform.DetachChildren();
        NumberOfGrounds = MINIMUM_NUMBER_OF_GROUNGS;
        if ((numberOfGrounds -= MINIMUM_NUMBER_OF_GROUNGS) > 0)
            AddGrounds(numberOfGrounds);
        numberOfGrounds = NumberOfGrounds;
        Current.IsActiveGround = true;
        
    }

    private void AddGround()
    {
        int indexbuffer = GroundIndex;
        GroundIndex = NumberOfGrounds - 1;
        GameObject temp = GameObject.Instantiate(GroundPreFab, new Vector3(grounds[0].transform.position.x + GroundOffset(), grounds[0].transform.position.y, grounds[0].transform.position.z), grounds[0].transform.rotation) as GameObject;
        temp.name = "SubGround" + NumberOfGrounds.ToString();
        temp.tag = "Ground";
        temp.layer = 2;
        temp.GetComponentInChildren<Switch>().SetID(GroundIndex);
        NumberOfGrounds++;
        NeedsUpdate = true;
        GroundIndex = indexbuffer;
    }

    private void AddGrounds(int groundsToAdd)
    {
        for (int i = 0; i < groundsToAdd; i++)
            AddGround();
        ResetGroundArray();
    }

    private void ResetGroundArray()
    {
        if (NeedsUpdate)
        {
            grounds = new GameObject[NumberOfGrounds];
            for (int i = 0; i < NumberOfGrounds; i++)
            {
                grounds[i] = GameObject.Find("SubGround" + i.ToString());
            }
            NeedsUpdate = false;

        }
    }


    public static GameObject Switch(int groundnumber)
    {
        Vector3 camPosition = Camera.main.transform.position;
        camPosition.x -= GroundOffset();
        GroundIndex = groundnumber;
        camPosition.x += GroundOffset();
        Camera.main.transform.position = camPosition;
        Current.IsActiveGround = false;
        masterGround = Current.Control.SwitchTo(groundnumber);
        Current.IsActiveGround = true;
        if (SWITCH != null)
            SWITCH(true, groundnumber);
        return Current.gameObject;
    }
    public GameObject GetCurrentGround()
    {
        return grounds[GroundIndex];
    }
    public GameObject SwitchTo(int newindex)
    {
        return grounds[newindex];
    }
}
