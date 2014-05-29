using UnityEngine;
using System.Collections;

public class Attackability : UnitExtension
{
    public enum OPTIONS : int
    {
        Attack = EnumProvider.ORDERSLIST.Attack,
        Conquer = EnumProvider.ORDERSLIST.Conquer,
        Defend = EnumProvider.ORDERSLIST.Defend,
        Seek = EnumProvider.ORDERSLIST.Seek
    }
    public OPTIONS attackState = OPTIONS.Defend;

    public override string IDstring
    {
        get
        {
            return "Attackability";
        }
    }
    bool[] States = new bool[4];
    private enum STATES : byte
    {
        IsAttacking,
        IsDefending,
        HasAmunition,
        IsConquering,
    }
    public Vector3 AttackPoint = Vector3.zero;

    protected override EnumProvider.ORDERSLIST on_UnitStateChange(EnumProvider.ORDERSLIST stateorder)
    {
        if (System.Enum.IsDefined(typeof(OPTIONS), stateorder))
        {
            attackState = (OPTIONS)stateorder;
            switch (attackState)
            {
                case OPTIONS.Attack:
                    if (!UNIT.IsABuilding)
                    {
                        this.GetComponent<Movability>().SetKinematic();
                        this.GetComponent<Movability>().WayPoints.Clear();
                        UNIT.Options.MoveToPoint = this.gameObject.transform.position;
                        this.GetComponent<Movability>().IsMoving = false;
                    }

                    UNIT.Options.LockOnFocus();
                    break;
                case OPTIONS.Conquer:
                    if (!GetComponent<Gunner>())
                        this.gameObject.AddComponent<Gunner>();
                    UNIT.Options.UnitState = EnumProvider.ORDERSLIST.Stay;
                    if(!UNIT.IsABuilding)
                        UNIT.Options.LockOnFocus();
                    GetComponent<Gunner>().FireAtWill = true;
                    break;
                case OPTIONS.Seek:
                    if (!GetComponent<Gunner>())
                        this.gameObject.AddComponent<Gunner>();

                    break;
            }
        }
        return stateorder;
    }
    void Awake()
    {

    }
    void Start()
    {
        IsDefending = IsAttacking = IsConquering = false;
        PflongeOnUnit(typeof(OPTIONS));
    }

    public bool IsAttacking
    {
        get
        {
            if (States[(byte)STATES.IsAttacking])
            {
                if (UNIT.Options.Target == null)
                    States[(byte)STATES.IsAttacking] = false;
                else
                {
                    if (!UNIT.IsABuilding)
                    {
                        Movability Movement = GetComponent<Movability>();
                        Movement.MovingDirection = UNIT.Options.MoveToPoint;

                        if (Movement.Distance < UNIT.AttackRange)
                        {
                            UNIT.weapon.Reload();
                            UNIT.weapon.Engage(UNIT.Options.Target);
                        }

                        return Movement.IsMoving = true;
                    }
                    else
                    {
                        if (Vector3.Distance(this.gameObject.transform.position, AttackPoint) <= UNIT.AttackRange)
                        {
                            UNIT.weapon.Reload();
                            UNIT.weapon.Engage(UNIT.Options.Target);
                        }
                        return true;
                    }
                }
            }
            return false;
        }
        protected set
        {
            if ((UNIT.Options.Target != null) && ((OPTIONS)UNIT.Options.UnitState == OPTIONS.Attack)) 
                States[(byte)STATES.IsAttacking] = value;
            else 
                States[(byte)STATES.IsAttacking] = false;
        }
    }

    public bool IsDefending
    {
        get { return States[(byte)STATES.IsDefending] = UNIT.Options.UnitState == (System.Enum)EnumProvider.ORDERSLIST.Guard; }
        set
        {
            if(value)
                IsAttacking = !value;
            States[(byte)STATES.IsDefending] = value;
        }
    }
    public bool HasAmunition
    {
        get { return States[(byte)STATES.HasAmunition] = !UNIT.weapon.IsOutOfAmmo; }
    }
    public bool IsConquering
    {
        get
        {
            return States[(byte)STATES.IsConquering] = (( UNIT.Options.UnitState == (System.Enum)EnumProvider.ORDERSLIST.Seek)
                                                       || UNIT.Options.UnitState == (System.Enum)EnumProvider.ORDERSLIST.Conquer); 
        }
        set { States[(byte)STATES.IsConquering] = value; }
    }

    internal override void OptionExtensions_OnLEFTCLICK(bool hold)
    {
        if (!hold)
        {
            if (attackState == OPTIONS.Attack)
            {
                UnitScript otherUnit = MouseEvents.State.Position.AsUnitUnderCursor;
                if (otherUnit)
                {
                    if (UNIT.IsEnemy(otherUnit))
                    {
                        UNIT.Options.Target = otherUnit.gameObject;
                        UNIT.Options.MoveToPoint = UNIT.Options.Target.transform.position;
                        if (!UNIT.IsABuilding)
                            GetComponent<Movability>().IsMoving = true;
                        IsAttacking = true;
                    }
                    else if (UNIT.IsAllied(otherUnit))
                    {
                        this.UNIT.Options.Target = otherUnit.SetInteracting(this.gameObject);
                        if (otherUnit.Options.IsAttacking)
                        {
                            this.UNIT.Options.Target = otherUnit.Options.Target;
                            IsAttacking = true;
                            UNIT.Options.MoveAsGroup(otherUnit.gameObject);
                        }
                    }

                    UNIT.Options.UnlockFocus();    
                }
            }
            else if (attackState == OPTIONS.Conquer)
            {
                UNIT.Options.MoveToPoint = MouseEvents.State.Position;

                if (!GetComponent<Gunner>())
                    this.gameObject.AddComponent<Gunner>();

                if (!UNIT.IsABuilding)
                    GetComponent<Movability>().IsMoving = true;

                IsDefending = false;
                IsAttacking = true;
                IsConquering = true;
                GetComponent<Gunner>().FireAtWill = true;

                UNIT.Options.UnlockFocus();
            }
        }
    }
    internal override void OptionExtensions_OnRIGHTCLICK(bool hold)
    {
        if (!hold)
        {
            UNIT.Options.UnlockAndDestroyFocus();
        }
    }

    public override void DoUpdate()
    {
        if (IsAttacking) IsAttacking = Attack();
        if (IsConquering) IsDefending = Conquere();
    }

    private bool Attack()
    {
        if (GetComponent<Gunner>())
            GetComponent<Gunner>().DoUpdate();

        if (UNIT.Options.Target)
        { 
            AttackPoint = UNIT.Options.Target.transform.position;
            if (!UNIT.IsABuilding)
                GetComponent<Movability>().IsMoving = true;
            return true;
        }
        else
            return false;
    }

    private bool Conquere()
    {

        if (UNIT.IsABuilding)
            return true;
        else
        {
            return !(GetComponent<Movability>().IsMoving = (UNIT.Options.MoveToPoint != this.transform.position));
        }

    }
}
