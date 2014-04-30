using UnityEngine;
using System.Collections;

public class NoWeapon : Weapon {


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
        public override void Reloade()
        {

        }

}
