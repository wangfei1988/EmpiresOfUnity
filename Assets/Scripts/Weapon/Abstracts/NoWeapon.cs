using UnityEngine;
using System.Collections;

public class NoWeapon : UnitWeapon {

    public override bool IsOutOfAmmo
    {
        get { return true; }
    }
        public override void Engage(GameObject targetUnit)
        {

        }
        public override void Engage(Vector3 targetPoint)
        {

        }
        public override float GetMaximumRange()
        {
            return 0f;
        }
        public override void Reload()
        {

        }

}
