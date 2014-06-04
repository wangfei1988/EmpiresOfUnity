using UnityEngine;
using System.Collections;

public class MachineGun : UnitWeapon
{
    public float INTERVAL = 0.1f;
    public float timer=0f;
    public bool IsLoaded=false;
    public MG_Bullet Bullet;
    public int MAX_AMU;
    public override UnitWeapon.WEAPON ID
    {
        get { return WEAPON.MachineGun; }
    }
    private int Amu;
    public Vector3 AttackPoint
    {
        get 
        {
            return this.transform.forward*GetMaximumRange();
        }
        set 
        {
         //   Bullet.transform.position=this.transform.position;
            Bullet.transform.forward = (value - this.transform.position).normalized;
        }
    }

    void Start() 
    {
        Amu = MAX_AMU;
        Bullet = (GameObject.Instantiate(this.prefabSlot, this.transform.position, this.transform.rotation) as MG_Bullet);
        Bullet.SetShooter(this.gameObject);
	}

    public override float GetMaximumRange()
    {
       return Bullet.MAX_RANGE;
    }
    public override void Engage(Vector3 targetPoint)
    {
        AttackPoint=targetPoint;
        if (IsLoaded)
        {
            
            Bullet.Engage();
            IsLoaded=false;
        }
    }
    public override void Engage(GameObject targetUnit)
    {
        Engage(targetUnit.transform.position);
    }
    public override bool IsOutOfAmu
    {
        get { return Amu<=0; }
    }
    
    public override void Reload()
    {
        Bullet.transform.position = this.transform.position;
        if (!IsLoaded)
        {
            if ((timer+=Time.deltaTime)>=INTERVAL)
            {
                IsLoaded=true;
                timer=0;
            }
        }
    }
    void OnDestroy()
    {
        GameObject.Destroy(this.Bullet);
    }
}
