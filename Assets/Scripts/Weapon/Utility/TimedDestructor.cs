using UnityEngine;
using System.Collections;

public class TimedDestructor : MonoBehaviour
{
    public void DestroyByTimer(float timer)
    {
        Invoke("DestroyNow", timer);
    }

    void DestroyNow()
    {
        DestroyObject(gameObject);
    }
}