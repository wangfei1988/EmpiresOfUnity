/*UnitAnimation: Building Grower
 * 
 * if put on a Unit which is a Building, it's causing it to "grow" out of the ground.
 * when finished growing, it will add a Rigidbody to the object and then it will
 * destruct itself... 
 */
using UnityEngine;
using System.Collections;

[AddComponentMenu("Program-X/Buildings/Building Grower")]
public class BuildingGrower : UnitAnimation 
{

    public float targetYps;
    private float startYps;
    private float growingFactor;
    private float GrowingTime=0;
    private float timer;
    [SerializeField]
    private bool startGrowing = false;
    [SerializeField]
    private AudioClip AudioGrow;
    public bool StartGrowing
    {
        

        get 
        {
            if ((startGrowing)&&(growingFactor== -1f))
            {
                growingFactor = 0f;
                //Grow();
            }
            return startGrowing;
        }
        set 
        {
            startGrowing = value;
            if (startGrowing)
                PlaySound();

            //if (startGrowing)
            //    startYps = gameObject.transform.position.y;
        }
    }
    private float growState;
    void Start()
    {
        timer = 0f;
        startYps = gameObject.transform.position.y;
        growingFactor = -1f;
        this.GrowingTime = this.GetComponent<UnitOptions>().SettingFile.GrowingTime;

        // let MainBuilding Play sound at init
        if (startGrowing)
            PlaySound();
    }

    private void PlaySound()
    {

        AudioSource mySource = this.gameObject.AddComponent<AudioSource>();
        mySource.clip = AudioGrow;
        mySource.rolloffMode = AudioRolloffMode.Linear;
        mySource.Play();
    }

    private void Grow()
    {
        timer += Time.deltaTime;
        growingFactor = Mathf.Clamp((timer / GrowingTime), 0f, 1f);
        growState = Mathf.SmoothStep(startYps, targetYps, growingFactor);

        transform.position = new Vector3(gameObject.transform.position.x,  growState, gameObject.transform.position.z);
        if (growingFactor == 1)
            StartGrowing = false;
    }

    internal override void Animate()
    {
        if (StartGrowing)
        {
            Grow();
        }
        else if (growingFactor == 1)
        {
            growingFactor = 2;
            if(!gameObject.GetComponent<Rigidbody>())
                gameObject.AddComponent<Rigidbody>();
            if(!gameObject.GetComponent<Shaker>())
                gameObject.AddComponent<Shaker>();

            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Shaker>().IsActive = false;
            gameObject.GetComponent<Shaker>().amount.z = 0.0001f;
            gameObject.GetComponent<Shaker>().speed.z = 0.0001f;
            gameObject.GetComponent<Shaker>().sineRooting = Shaker.ROOTING.mainPosition;
            gameObject.GetComponent<Shaker>().mainTargetTransform = this.gameObject.transform;
            gameObject.GetComponent<Shaker>().IsActive = true;
            gameObject.GetComponent<Shaker>().HookOnUpdata(this.gameObject.GetComponent<UnitScript>());
            if (NextUnitAnimation)
                this.gameObject.GetComponent<BuildingGrower>().NextUnitAnimation.HookOnUpdata(gameObject.GetComponent<Shaker>());

            gameObject.GetComponent<Rigidbody>().isKinematic = false;

            // Build Finished
            if (this.gameObject.GetComponent<AbstractBuilding>())
            {
                this.gameObject.GetComponent<AbstractBuilding>().BuildFinished();
            }

            Component.Destroy(gameObject.GetComponent<BuildingGrower>());
        }

    }

}
