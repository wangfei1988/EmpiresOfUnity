using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//--   A.I. to improve shooting-behavior of AttackUnits
[AddComponentMenu("Program-X/UNIT/AI - Gunner")]
public class Gunner : UnitComponent
{
    public override string IDstring
    {
        get
        {
            return "Rambo";
        }
    }
    public bool FireAtWill;
    public SortedDictionary<Vector3, float> Targets = new SortedDictionary<Vector3, float>();
    [SerializeField]
    private bool _battle = false;
    public bool Battle
    {
        get
        {
            return _battle;
        }
        set
        {
            _battle = this.GetComponent<Pilot>().PerceptionIsGunnerControlled = value;
        }
    }
    public float MAXIMUM_ATTACK_RANGE
    {
        get { return weapon.GetMaximumRange(); }
    }
    public UnitWeapon weapon;
    public SphereCollider MySpace;

    public EnumProvider.ORDERSLIST OrderState
    {
        get { return (EnumProvider.ORDERSLIST)UNIT.Options.UnitState; }
        set { UNIT.Options.UnitState = value; }
    }
    public EnumProvider.ORDERSLIST BattleState = EnumProvider.ORDERSLIST.Patrol;
    void Awake()
    {
        weapon = GetComponent<UnitWeapon>();
        MySpace = GetComponent<SphereCollider>();
    }

    void Start()
    {
        PflongeOnUnit();
    }

    public override void DoUpdate()
    {
        if (Battle) Battle = Fight();
    }

    private bool Fight()
    {
        if (FireAtWill)
        {
            float nearest = MAXIMUM_ATTACK_RANGE;
            Vector3 index = Vector3.zero;
            foreach (Vector3 position in Targets.Keys)
            {
                if (nearest > Targets[position])
                {
                    nearest = Targets[position];
                    index = position;
                }
            }
            weapon.Engage(index);
        }
        return UNIT.IsUnderAttack || UNIT.Options.IsAttacking || Targets.Count > 0;
    }

    private float MaximizePerseptionRadius()
    {
        if (((EnumProvider.ORDERSLIST)UNIT.Options.UnitState) != EnumProvider.ORDERSLIST.Hide)
        {
            this.GetComponent<Pilot>().PerceptionIsGunnerControlled = true;
            return MySpace.radius = weapon.GetMaximumRange();

        }
        else
            return MySpace.radius;

    }

    void OnTriggerEnter(Collider other)
    {
        if (UNIT.IsEnemy(other.gameObject))
        {
            MaximizePerseptionRadius();
            if (UNIT.ALARM < UnitScript.ALLERT_LEVEL.A)
                UNIT.ALARM++;

            if (FireAtWill)
            {
                if (weapon.IsOutOfAmmo)
                {
                    UNIT.Options.UnitState = EnumProvider.ORDERSLIST.Hide;
                    if (!UNIT.IsABuilding)
                        UNIT.Options.FocussedLeftOnGround(-(other.gameObject.transform.position - this.transform.position));
                }
                else if (UNIT.ALARM >= UnitScript.ALLERT_LEVEL.RED)
                {
                    weapon.Engage(other.gameObject);
                }
            }

            switch ((EnumProvider.ORDERSLIST)OrderState)
            {
                case EnumProvider.ORDERSLIST.Attack:

                    break;
                case EnumProvider.ORDERSLIST.Guard:

                    break;
                case EnumProvider.ORDERSLIST.Patrol:
                    UNIT.ALARM = UnitScript.ALLERT_LEVEL.A;
                    break;
                case EnumProvider.ORDERSLIST.Seek:

                    break;
                case EnumProvider.ORDERSLIST.Hide:
                    if (!UNIT.IsABuilding)
                        UNIT.Options.FocussedLeftOnGround(-(other.gameObject.transform.position - this.transform.position));
                    break;

            }

        }
    }
    void OnTriggerStay(Collider other)
    {
        if (UNIT.IsEnemy(other.gameObject))
        {
            if (FireAtWill)
            {
                float distance;
                Vector3 targetPosition = other.transform.position;
                if (Targets.TryGetValue(targetPosition, out distance))
                    Targets[targetPosition] = Vector3.Distance(other.transform.position, this.transform.position);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (UNIT.IsEnemy(other.gameObject))
        {
            UNIT.ALARM--;
            Targets.Remove(other.transform.position);
        }
    }

    protected override EnumProvider.ORDERSLIST on_UnitStateChange(EnumProvider.ORDERSLIST stateorder)
    {
        return stateorder;
    }
}
