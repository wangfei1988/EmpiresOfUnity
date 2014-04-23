using UnityEngine;
using System.Collections;

abstract public class AnimaQuion : MonoBehaviour {

    public bool IsActive = true;
    public AnimaQuion anotherAnimaquion;

    abstract internal void Animate();

    internal void DoUpdate()
    {
        if (IsActive) Animate();
        if(anotherAnimaquion) anotherAnimaquion.DoUpdate();
    }
	

}
