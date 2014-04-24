using UnityEngine;
using System.Collections;


public class FocusRectangleObject : MonoBehaviour {


    public Quaternion rotary;
    public float focusScaling;

    private bool MasterHasFocus
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
                foreach (GameObject marker in Focus.Marker) marker.GetComponent<MarkerScript>().Visible = value;
                gameObject.renderer.enabled = value;
            }
        }
    }

    void Start()
    {


    }

    public void Equalize()
    {
        if (MasterHasFocus)
        {
            Visible = true;
            gameObject.transform.localScale = new Vector3(MasterTransform.lossyScale.x * focusScaling, MasterTransform.lossyScale.z * focusScaling, 1f);
            gameObject.transform.position = new Vector3(MasterTransform.position.x - MasterTransform.lossyScale.x, MasterTransform.position.y + MasterTransform.localScale.y, MasterTransform.position.z + MasterTransform.lossyScale.z);
            faceDirections();
        }
        else Visible = false;
    }

    private void faceDirections()
    {
        gameObject.GetComponent<FaceDirection>().DoUpdate();
        foreach (GameObject marker in Focus.Marker) marker.GetComponent<MarkerScript>().DoUpdate();
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

