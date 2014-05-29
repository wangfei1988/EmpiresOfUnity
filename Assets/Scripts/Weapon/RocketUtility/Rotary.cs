using UnityEngine;
using System.Collections;

public class Rotary : MonoBehaviour {

    public float rotation;
    void Start()
    { }
    void Update()
    { }

    void OnTriggerEnter(Collider hit)
    {
        transform.parent.gameObject.GetComponent<SmallRocketObject>().SpriteColliderEnter(hit.collider.gameObject);
    }

    public void Rotation(Vector3 R)
    {
        this.transform.Rotate(R.x, R.y, R.z);
    }
}
