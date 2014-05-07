using UnityEngine;
using System.Collections;

[AddComponentMenu("Program-X/UNIT/UnitOptions (Flying Units)")]
public class FlyingUnitOptions : GroundUnitOptions
{

    public override EnumProvider.UNITCLASS UNIT_CLASS
    {
        get { return EnumProvider.UNITCLASS.AIR_UNIT; }
    }
    public WingsAndJets Aviator;

    internal override void DoStart()
    {
        if (!this.gameObject.GetComponent<WingsAndJets>()) this.gameObject.AddComponent<WingsAndJets>();
        Aviator = this.gameObject.GetComponent<WingsAndJets>();
        base.DoStart();

    }

    public float Speed_Height_Relation = 10;

    internal override void DoUpdate()
    {

        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, (Speed > 0) ? (Speed * Speed_Height_Relation) : (-Speed * Speed_Height_Relation), this.gameObject.transform.position.z);
        base.DoUpdate();
        Aviator.DoUpdate();
    }

    
    

}
