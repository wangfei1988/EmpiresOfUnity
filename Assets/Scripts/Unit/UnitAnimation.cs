using UnityEngine;
using System.Collections;

abstract public class UnitAnimation : MonoBehaviour {
    
    [SerializeField]
    private bool isActive = true;
    public bool IsActive
    {
        get { return isActive; }
        set
        {
            if (value != isActive)
                if (value & GetComponent<Rigidbody>()) GetComponent<Rigidbody>().isKinematic=false;
              
        }
    }

    public UnitAnimation anotherUnitAnimation;

    abstract internal void Animate();

    void Start()
    {
        UpdateManager.OnUpdate += DoUpdate;
    }

    internal void DoUpdate()
    {
        if (IsActive)
            Animate();
        if(anotherUnitAnimation)
            anotherUnitAnimation.DoUpdate();
    }

    internal void HookOnUpdata(UnitAnimation updateter)
    {
        updateter.anotherUnitAnimation = this;
    }
    internal void HookOnUpdata(UnitScript onUnitScript)
    {
        onUnitScript.unitAnimation = this; //.GetComponent<UnitAnimation>();
    }

}
