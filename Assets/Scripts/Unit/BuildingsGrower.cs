using UnityEngine;
using System.Collections;

public class BuildingsGrower : UnitAnimation 
{
    public float startYps;
    public float targetYps;
    public float growingFactor;
    public float GrowingTime;
    public float timer;
    [SerializeField]
    private bool startGrowing = false;
    public bool StartGrowing
    {
        get 
        {
            if ((startGrowing)&&(growingFactor== -1f))
            {
                growingFactor = 0f;
                Grow();
            }
            return startGrowing;
            
        }
        set 
        {
            if (!value) 
            {
                startGrowing = value;
            }
        }
    }
    private float growState;
    void Start()
    {
        targetYps = 0f;
        startYps = gameObject.transform.position.y;
        GrowingTime = gameObject.GetComponent<UnitScript>().Speed;
        
        timer=0f;
        growingFactor = -1f;
     //   gameObject.transform.position = new Vector3(gameObject.transform.position.x,Height,gameObject.transform.position.z);
	}

    private bool Grow()
    {
        if (startGrowing)
        {
            timer += Time.deltaTime;
            growingFactor = Mathf.Clamp( (timer / GrowingTime),0f,1f);
            growState = Mathf.SmoothStep(startYps, targetYps, growingFactor);
            transform.position = new Vector3(gameObject.transform.position.x,  growState, gameObject.transform.position.z);
            if (growingFactor == 1) return false;
            return true;
        }
        else
        {

            return false;
        }
    }

    internal override void Animate()
    {
        if (StartGrowing) StartGrowing = Grow();
        else if (growingFactor > -1)
        {
            if (!gameObject.GetComponent<Rigidbody>())
            {
                gameObject.AddComponent<Rigidbody>().isKinematic = true;
                gameObject.AddComponent<Shaker>().IsActive = false;
            }
            else
            {
                gameObject.GetComponent<Rigidbody>().useGravity = false;
                gameObject.GetComponent<Shaker>().amount.z = 0.0001f;
                gameObject.GetComponent<Shaker>().speed.z = 0.0001f;
                gameObject.GetComponent<Shaker>().sineRooting = Shaker.ROOTING.mainPosition;
                gameObject.GetComponent<Shaker>().mainTargetTransform = this.gameObject.transform;
                gameObject.GetComponent<Shaker>().HookOnUpdata(this.gameObject.GetComponent<UnitScript>());
                gameObject.GetComponent<Shaker>().IsActive = true;
                Component.Destroy(gameObject.GetComponent<BuildingsGrower>());
            }
        }
    }

}
