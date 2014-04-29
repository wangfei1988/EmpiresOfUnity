using UnityEngine;
using System.Collections;
using System.Timers;

public class Quarry : MonoBehaviour 
{
    //Stats
    public float quarryLife = 1;
    public int quarryLevel = 1;
    


    //Timer
    private Timer workTimer;

    //Methods
    void Start()
    {
        QuarryWork();

    }

    void Update()
    {
        QuarryWork();
    }

    private void QuarryWork()
    {
        while (false)
        {
            if (quarryLevel == 1)
            {
                ResourceManager.AddResouce(ResourceManager.Resource.STONE, 10);
            }
            if (quarryLevel == 2)
            {
            }
            if (quarryLevel == 3)
            {
            }
            if (quarryLevel == 4)
            {
            }
            if (quarryLevel == 5)
            {

            }
        }

    }
}
