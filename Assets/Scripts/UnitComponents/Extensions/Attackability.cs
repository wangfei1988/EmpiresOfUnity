using UnityEngine;
using System.Collections;

public class Attackability : UnitExtension
{
    new public enum OPTIONS : int
    {
        Attack = EnumProvider.ORDERSLIST.Attack,
        Conquer = EnumProvider.ORDERSLIST.Conquer,
        Seek = EnumProvider.ORDERSLIST.Seek
    }
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

    protected override EnumProvider.ORDERSLIST on_UnitStateChange(EnumProvider.ORDERSLIST stateorder)
    {
        switch ((OPTIONS)stateorder)
        {
            case OPTIONS.Attack:

                break;
            case OPTIONS.Conquer:

                break;
            case OPTIONS.Seek:

                break;
        }
        return stateorder;
    }

    void Start()
    {
        IsDefending = IsAttacking = IsConquering = false;
        PflongeOnUnit(System.Enum.GetValues(typeof(OPTIONS)));
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
                    if (!UNIT.IsABuilding)
                    {
                        (UNIT.Options as MovingUnitOptions).MovingDirection = UNIT.Options.MoveToPoint;

                        if ((UNIT.Options as MovingUnitOptions).Distance < UNIT.AttackRange)
                        {
                            UNIT.weapon.Reloade();
                            UNIT.weapon.Engage(UNIT.Options.Target);
                        }

                        return (UNIT.Options as MovingUnitOptions).IsMoving = true;
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
        get { return States[(byte)STATES.HasAmunition] = !UNIT.weapon.IsOutOfAmmu; }
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
        
    }
    internal override void OptionExtensions_OnRIGHTCLICK(bool hold)
    {
        
    }

    public override void DoUpdate()
    {
        if (IsAttacking) IsAttacking = Attack();
    }

    private bool Attack()
    {
        if (UNIT.Options.Target)
        { 
            UNIT.Options.MoveToPoint = UNIT.Options.Target.transform.position;
            if (!UNIT.IsABuilding)
                (UNIT.Options as MovingUnitOptions).IsMoving = true;
            return true;
        }
        else
            return false;
    }
}
