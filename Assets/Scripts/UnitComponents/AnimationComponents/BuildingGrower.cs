/*UnitAnimation: Building Grower
 * 
 * if put on a Unit which is a Building, it's causing it to "grow" out of the ground.
 * when finished growing, it will add a Rigidbody to the object and then it will
 * destruct itself... 
 */
using UnityEngine;
using System.Collections;

[AddComponentMenu("Program-X/Buildings/Building Grower (Grow from Ground)")]
public class BuildingGrower : UnitAnimation 
{

    public float targetYps;
    private float startYps;
    private float growingFactor;
    [SerializeField]
    private float GrowingTime=0;
    private float timer;
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
        timer = 0f;
        startYps = gameObject.transform.position.y;
        growingFactor = -1f;
        //GrowingTime = gameObject.GetComponent<UnitScript>().Settings.ProductionTime;
	}

    private bool Grow()
    {
        if (startGrowing)
        {
            timer += Time.deltaTime;

            growingFactor = Mathf.Clamp((timer / GrowingTime), 0f, 1f);
            growState = Mathf.SmoothStep(startYps, targetYps, growingFactor);
            transform.position = new Vector3(gameObject.transform.position.x,  growState, gameObject.transform.position.z);
            if (growingFactor == 1)
                return false;
            return true;
        }
        return false;
    }

    internal override void Animate()
    {
        if (StartGrowing)
        {
            StartGrowing = Grow();
        }
        else if (growingFactor == 1)
        {
            growingFactor = 2;
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
                Component.Destroy(gameObject.GetComponent<BuildingGrower>());
            }

            // Build Finished
            if (this.gameObject.GetComponent<AbstractBuilding>())
            {
                this.gameObject.GetComponent<AbstractBuilding>().BuildFinished();
            }
        }

    }

}
