using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour
{
    private const int MINIMUM_NUMBER_OF_GROUNGS = 3;
    public static GameObject Current;
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
        return (Current.gameObject.transform.localScale.x * 1.5f) * GroundIndex;
    }


    void Awake()
    {

        Current = this.gameObject.transform.FindChild("SubGround0").gameObject;
        grounds[0] = this.gameObject.transform.FindChild("SubGround0").gameObject;
        grounds[1] = this.gameObject.transform.FindChild("SubGround1").gameObject;
        grounds[2] = this.gameObject.transform.FindChild("SubGround1").gameObject;
        GroundIndex = 0;
    }


    void Start()
    {
        this.gameObject.transform.DetachChildren();
        NumberOfGrounds = MINIMUM_NUMBER_OF_GROUNGS;
        if ((numberOfGrounds -= MINIMUM_NUMBER_OF_GROUNGS) > 0)
            AddGrounds(numberOfGrounds);
        numberOfGrounds = NumberOfGrounds;
    }

    private void AddGround()
    {
        int indexbuffer = GroundIndex;
        GroundIndex = NumberOfGrounds - 1;
        GameObject temp = GameObject.Instantiate(GroundPreFab, new Vector3(grounds[0].transform.position.x + GroundOffset(), grounds[0].transform.position.y, grounds[0].transform.position.z), grounds[0].transform.rotation) as GameObject;
        temp.name = "SubGround" + NumberOfGrounds.ToString();
        temp.tag = "Ground";
        temp.layer = 2;
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
                    grounds[i] = GameObject.Find("SubGround" + i.ToString());

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
        Current = Current.GetComponent<Ground>().GetGround(GroundIndex);
        return Current.gameObject;
    }
    public GameObject GetCurrent()
    {
        return grounds[GroundIndex];
    }
    public GameObject GetGround(int index)
    {
        return grounds[index];
    }
}
