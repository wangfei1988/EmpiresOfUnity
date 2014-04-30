using UnityEngine;
using System.Collections;


public class FocusRectangleObject : MonoBehaviour {


    public Quaternion rotary;
    public float focusScaling;

    private FaceDirection directionFacer;
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
            if (MasterHasFocus) return Focus.masterGameObject.transform;
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
        directionFacer = this.gameObject.GetComponent<FaceDirection>();

        UpdateManager.OnUpdate += DoUpdate;
    }
    void DoUpdate()
    {
        Equalize();
    }

    public void Equalize()
    {
        if (MasterHasFocus)
        {
            Visible = true;
            gameObject.transform.localScale = new Vector3(MasterTransform.localScale.x * focusScaling, MasterTransform.localScale.z * focusScaling, 1f);
            gameObject.transform.position = new Vector3(MasterTransform.position.x, MasterTransform.position.y+MasterTransform.localScale.y, MasterTransform.position.z);
            faceDirections();
        }
        else Visible = false;
    }

    private void faceDirections()
    {
        directionFacer.DoUpdate();
        foreach (MarkerScript marker in Focus.Marker) marker.DoUpdate();
    }



}

