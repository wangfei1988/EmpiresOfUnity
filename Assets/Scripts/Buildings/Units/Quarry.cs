using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Timers;

public class Quarry : MonoBehaviour 
{
    //Stats
    public float quarryLife = 1;
    public int quarryLevel = 1;
    public float resourceEachSeconds = 3;

    private Dictionary<int, uint> stoneWork = new Dictionary<int, uint>();

    //Timer
    private float workTimer;

    // Reflection
    void Start()
    {
        UpdateHandler.OnUpdate += DoUpdate;
        stoneWork.Add(1, 10);
        stoneWork.Add(2, 15);
        stoneWork.Add(3, 18);
    }

    void OnDestroy()
    {
       UpdateHandler.OnUpdate -= DoUpdate;
    }

    void DoUpdate()
    {
        workTimer += Time.deltaTime;
        // nur ausfühen alle 5 sek
        if (workTimer >= resourceEachSeconds)
        {
            workTimer = 0;
            QuarryWork();
        }
    }

    //Methods
    private void QuarryWork()
    {
        uint resValue = 0;
        stoneWork.TryGetValue(quarryLevel, out resValue);
        ResourceManager.AddResouce(ResourceManager.Resource.STONE, resValue);
            Debug.Log(resValue);
    }
}
