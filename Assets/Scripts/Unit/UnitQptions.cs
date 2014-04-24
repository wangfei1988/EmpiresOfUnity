using UnityEngine;
using System.Collections;
using System.Collections.Generic;



abstract public class UnitOptions : MonoBehaviour
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
        get { return gameObject.GetComponent<UnitScript>().weapon.GetMaximumRange(); }

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
    virtual internal void FocussedLeftOnGround(Vector3 worldPoint)
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
    virtual internal void FocussedRightOnGround(Vector3 worldPoint)
    {
        Component.Destroy(gameObject.GetComponent<Focus>());
    }
    virtual internal void FocussedLeftOnEnemy(GameObject enemy)
    { enemy.AddComponent<Focus>(); }
    virtual internal void FocussedRightOnEnemy(GameObject enemy)
    { }
    virtual internal void FocussedLeftOnAllied(GameObject friend)
    { friend.AddComponent<Focus>(); }
    virtual internal void FocussedRightOnAllied(GameObject friend)
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
    
    protected void LockOnFocus()
    {
        if (!gameObject.GetComponent<Focus>()) gameObject.AddComponent<Focus>();
        gameObject.GetComponent<Focus>().Lock();
        _isMouseEventsRecieving = true;
    }
    protected void UnlockFocus()
    {
        _isMouseEventsRecieving = false;
        MouseEvents.LEFTRELEASE += MouseEvents_LEFTRELEASE;
    }
    private bool _isMouseEventsRecieving;
    private void MouseEvents_LEFTRELEASE()
    {
        gameObject.GetComponent<Focus>().Unlock(gameObject);
        MouseEvents.LEFTRELEASE -= MouseEvents_LEFTRELEASE;
    }

    virtual protected void MouseEvents_LEFTMouseEvents(Ray qamRay, bool hold) { }
    virtual protected void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold) { }

    protected bool TargetIsEnemy(GameObject target)
    {
        return target.GetComponent<UnitScript>().GoodOrEvil != gameObject.GetComponent<UnitScript>().GoodOrEvil;
    }
    protected bool TargetIsAllied(GameObject target)
    {
        if (target.GetInstanceID() != gameObject.GetInstanceID())
            return target.GetComponent<UnitScript>().GoodOrEvil == gameObject.GetComponent<UnitScript>().GoodOrEvil;
        else return false;
    }
    abstract internal void Hit(int power);
}
