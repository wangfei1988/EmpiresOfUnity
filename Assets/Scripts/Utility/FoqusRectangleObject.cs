using UnityEngine;
using System.Collections;


public class FoqusRectangleObject : MonoBehaviour {


    public Quaternion rotary;

    private bool MasterHasFoqus
    {
        get 
        {
            if (Focus.masterGameObject) return Focus.masterGameObject.GetComponent<Focus>();
            else return false;        
        }   
    }
    private Transform MasterTransform
    {
        get 
        {
            if (Focus.masterGameObject) return Focus.masterGameObject.transform;
            else return null;
        }
    }

    public bool Visible
    {
        get { return gameObject.renderer.enabled; }
        private set
        {
            if (gameObject.renderer.enabled != value)
            {
                foreach (MarkerScript marker in Focus.Marker) marker.Visible = value;
                gameObject.renderer.enabled = value;
            }
        }
    }

    void Start()
    {


    }



    public void Equalize()
    {
        if (MasterHasFoqus)
        {
            Visible = true;
            gameObject.transform.localScale = new Vector3(MasterTransform.lossyScale.x * 2f, MasterTransform.lossyScale.z * 2f, 1f);
            gameObject.transform.position = new Vector3(MasterTransform.position.x - MasterTransform.lossyScale.x, MasterTransform.position.y + MasterTransform.localScale.y, MasterTransform.position.z + MasterTransform.lossyScale.z);
            faceDirections();
        }
        else Visible = false;
    }

    private void faceDirections()
    {
        gameObject.GetComponent<FaceDirection>().DoUpdate();
        foreach (MarkerScript marker in Focus.Marker) marker.DoUpdate();
    }

    //public void SetTo(Transform to)
    //{
    //    MasterTransform = to;
    //    transform.position = new Vector3(to.position.x, to.localScale.y/2f, to.position.z);
    //    transform.localScale = new Vector3((to.localScale.x / 4f) * 3f, (to.localScale.z / 4f) * 3f, 1f);
    //}

    void Update()
    {
        Equalize();   
    }

}

