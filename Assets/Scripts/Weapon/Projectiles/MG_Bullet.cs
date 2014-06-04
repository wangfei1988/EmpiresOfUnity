using UnityEngine;
using System.Collections;

public class MG_Bullet : WeaponObject 
{
    public ParticleSystem Bullet;
    public AudioSource MGSound;
    private Transform Shooter;
    public override WeaponObject.AMUNITONTYPE amunition
    {
        get
        {
            return AMUNITONTYPE.smallMGbullets;
        }
    }
    public override float MAX_RANGE
    {
        get { return 30f; }
    }
    

    public override UnitWeapon.WEAPON WEAPON
    {
        get
        {
            return UnitWeapon.WEAPON.MachineGun;
        }
    }

    internal override void StartUp()
    {
  //      Shooter = UNIT.gameObject.transform;
        Bullet = this.GetComponent<ParticleSystem>();
        MGSound = this.GetComponent<AudioSource>();

    }
    internal override void Engage()
    {
        if (!MGSound.isPlaying)
            MGSound.Play();
        Bullet.Play();
    }

}
