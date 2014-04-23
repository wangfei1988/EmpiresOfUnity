using UnityEngine;
using System.Collections;
using System.Collections.Generic;



abstract public class UnitQptions : MonoBehaviour
{
    

    public enum OPTIONS : int
    {
        Sellect,
        Cancel
    }

    //--------------------------------------- Values...

    virtual public int Life
    {
        get;
        protected set;
    }
    public float Speed
    {
        get;
        set;
    }
    virtual public float AttackRange
    {
        get { return gameObject.GetComponent<UnitSqript>().weapon.GetMaximumRange(); }

    }


    //---------------------------------------- State Flags...
    public virtual bool IsMoving
    {
        get
        {
            return _mooving;
        }
        protected set
        {
            if (!value) _groupmove = false;
            _mooving = value;
        }
    }
    protected bool _mooving;
    public bool IsMovingAsGroup
    {
        get { return _groupmove; }
        protected set
        {
            if (value) _mooving = true; 
            _groupmove = value;
        }
    }
    public bool IsGroupLeader;
    protected bool _groupmove;
    public virtual bool IsAttacking
    { get; protected set; }
    public bool IsUnderAttack
    { get; set; }
    
    //-----------------------------------------------------Orders and Interaction...
    virtual internal string[] GetUnitsMenuOptions()
    {
        return System.Enum.GetNames(UnitState.GetType());
    }
    virtual public void GiveOrder(int orderNumber)
    {
        UnitState = (OPTIONS)orderNumber;
    }
    protected bool standardOrder = false;
    abstract public System.Enum UnitState
    { get; set; }
    virtual internal void FoqussedLeftOnGround(Vector3 worldPoint)
    {
        standardOrder = true;
        IsMovingAsGroup = true;
        gameObject.rigidbody.isKinematic = true;
        UnitState = (OPTIONS)0;
        SetMoveToPoint(worldPoint);
        movingDirection = (MoveToPoint - gameObject.transform.position).normalized;
        gameObject.transform.position += movingDirection*Speed;
        IsAttacking = false;
        Target = null;
        standardOrder = false;
    }
    virtual internal void FoqussedRightOnGround(Vector3 worldPoint)
    {
        Component.Destroy(gameObject.GetComponent<FoQus>());
    }
    virtual internal void FoqussedLeftOnEnemy(GameObject enemy)
    { enemy.AddComponent<FoQus>(); }
    virtual internal void FoqussedRightOnEnemy(GameObject enemy)
    { }
    virtual internal void FoqussedLeftOnAllied(GameObject friend)
    { friend.AddComponent<FoQus>(); }
    virtual internal void FoqussedRightOnAllied(GameObject friend)
    { }
    abstract internal void MoveAsGroup(GameObject leader);
    

    //------------------------------------------------- Navigation..
    virtual public Vector3 MoveToPoint
    { get; set; }
    virtual public float Distance
    {
        get;
        protected set;
    }
    virtual public void SetMoveToPoint(Vector3 point)
    {
        MoveToPoint = point;
    }
    public Vector3 movingDirection;
    public GameObject Target;

    public GameObject MoveToPointMarker;
    public GameObject AttackPointMarker;
    public GameObject WayPointMarker;

    //---------------------------------------- Enginal stuff and functions...
    abstract internal void DoUpdate();
    internal void OptionsUpdate()
    {
        checkKinematic();
        DoUpdate();
    }
    virtual internal void SetUp(int life,float speed)
    {
        Life = life;
        Speed = speed;
    //    gameObject.transform.DetachChildren();
    }
    protected void SetKinematic()
    {
        gameObject.rigidbody.isKinematic = true;
        kinematicFrames=2;
    }
    private void checkKinematic()
    {
        if (gameObject.rigidbody.isKinematic)
            if (kinematicFrames <= 0) gameObject.rigidbody.isKinematic = false;
            else --kinematicFrames;
    }
    private int kinematicFrames;
    
    protected void LockOnFoqus()
    {
        if (!gameObject.GetComponent<FoQus>()) gameObject.AddComponent<FoQus>();
        gameObject.GetComponent<FoQus>().Lock();
        _isQlickRecieving = true;
    }
    protected void UnlockFoqus()
    {
        _isQlickRecieving = false;
        Qlick.LEFTRELEASE += Qlick_LEFTRELEASE;
    }
    private bool _isQlickRecieving;
    private void Qlick_LEFTRELEASE()
    {
        gameObject.GetComponent<FoQus>().Unlock(gameObject);
        Qlick.LEFTRELEASE -= Qlick_LEFTRELEASE;
    }

    virtual protected void Qlick_LEFTQLICK(Ray qamRay, bool hold) { }
    virtual protected void Qlick_RIGHTQLICK(Ray qamRay, bool hold) { }

    protected bool TargetIsEnemy(GameObject target)
    {
        return target.GetComponent<UnitSqript>().GoodOrEvil != gameObject.GetComponent<UnitSqript>().GoodOrEvil;
    }
    protected bool TargetIsAllied(GameObject target)
    {
        if (target.GetInstanceID() != gameObject.GetInstanceID())
            return target.GetComponent<UnitSqript>().GoodOrEvil == gameObject.GetComponent<UnitSqript>().GoodOrEvil;
        else return false;
    }
    abstract internal void Hit(int power);
}
