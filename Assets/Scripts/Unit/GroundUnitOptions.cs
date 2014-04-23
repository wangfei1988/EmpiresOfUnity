using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Character/Unit Options (Standard GroundUnit)")]
class GroundUnitOptions : UnitQptions
{
    public UnitSqript UNIT;
    new public enum OPTIONS : int
    {

        MoveTo,
        Atack,
        Guard,
        Patrol,
        Hide,
        Seek,
        Stay
    }

    public OPTIONS unitState;
   public override System.Enum UnitState
    {
        get
        {
            return unitState;
        }
        set
        {
            OPTIONS order = (OPTIONS)value;
            if (unitState != order)
            {
                if (!standardOrder)
                {
                    switch (order)
                    {
                        case OPTIONS.MoveTo:
                            {
                                SetKinematic();
                                WayPoints.Clear();
                                LockOnFoqus();
                                Qlick.LEFTQLICK += Qlick_LEFTQLICK;
                                break; 
                            }
                        case OPTIONS.Patrol:
                            {
                                SetKinematic();
                                LockOnFoqus();
                                Qlick.LEFTQLICK += Qlick_LEFTQLICK;
                                Qlick.RIGHTQLICK += Qlick_RIGHTQLICK;
                                WayPoints.Add(gameObject.transform.position);
                                break; 
                            }
                        case OPTIONS.Atack:
                            {
                                SetKinematic();
                                WayPoints.Clear();
                                LockOnFoqus();
                                Qlick.LEFTQLICK += Qlick_LEFTQLICK; 
                                break; 
                            }
                        case OPTIONS.Stay:
                            {
                                SetKinematic();
                                UnlockFoqus();
                                WayPoints.Clear();
                                IsAttacking = false;
                                SetMoveToPoint(gameObject.transform.position);
                                break;
                            }
                        case OPTIONS.Guard:
                            {
                                SetKinematic();
                                LockOnFoqus();
                                WayPoints.Clear();
                                Qlick.LEFTQLICK += Qlick_LEFTQLICK; 
                                break; 
                            }
                             

                    }
                }
                unitState = order;
            }
        }
    }

   public float standardYPosition;

   internal override void Hit(int power)
   {
       Life -= power;
   }

   public override float AttackRange
   {
       get
       {
           return UNIT.weapon.GetMaximumRange();
       }
   }
   protected override void Qlick_LEFTQLICK(Ray qamRay, bool hold)
   {
       if (!hold)
       {
           if (gameObject.GetComponent<FoQus>())
           {
               if (unitState == OPTIONS.MoveTo)
               {
                   SetMoveToPoint(Qlick.State.Position.AsWorldPointOnMap);
                   movingDirection = GetMovingDirection();
                   IsMoving = true;
                   Qlick.LEFTQLICK -= Qlick_LEFTQLICK;
                   UnlockFoqus();
               }
               else if (unitState == OPTIONS.Guard)
               {
                   RaycastHit Hit;
                   if (Physics.Raycast(qamRay, out Hit, Camera.main.transform.position.y))
                   {
                       if (TargetIsAllied(Hit.collider.gameObject))
                       {
                           Target = Hit.collider.gameObject.GetComponent<UnitSqript>().SetInteracting(this.gameObject);
                           SetMoveToPoint(Target.transform.position);
                           IsMoving = true;
                       }
                   }
                   Qlick.LEFTQLICK -= Qlick_LEFTQLICK;
                   UnlockFoqus();
               }
               else if (unitState == OPTIONS.Patrol)
               {
                   WayPoints.Add(Qlick.State.Position.AsWorldPointOnMap);
                   IsMoving = true;
                   Qlick.LEFTQLICK -= Qlick_LEFTQLICK;
                   Qlick.RIGHTQLICK -= Qlick_RIGHTQLICK;
                   UnlockFoqus();
               }
               else if (unitState == OPTIONS.Atack)
               {
                   RaycastHit Hit;
                   if (Physics.Raycast(qamRay, out Hit, Camera.main.transform.position.y))
                   {
                       if (TargetIsEnemy(Hit.collider.gameObject))
                       {
                           Target = Hit.collider.gameObject;
                           SetMoveToPoint(Target.transform.position);
                           IsMoving = true;
                           IsAttacking = true;
                       }
                   }
                   Qlick.LEFTQLICK -= Qlick_LEFTQLICK;
                   UnlockFoqus();
               }
           }
       }
   }
   protected override void Qlick_RIGHTQLICK(Ray qamRay, bool hold)
   {
       if (!hold)
       {
           if (gameObject.GetComponent<FoQus>())
           {
               if (unitState == OPTIONS.Patrol)
               {
                   if (WayPoints.Count >= 2) WayPoints.Insert(WayPoints.Count - 1, Qlick.State.Position.AsWorldPointOnMap);
                   else WayPoints.Add(Qlick.State.Position.AsWorldPointOnMap);
                   movingDirection = GetMovingDirection();
                   IsMoving = true;
               }
           }
       }
   }


   internal override void FoqussedLeftOnEnemy(GameObject enemy)
   {
       standardOrder = true;
       unitState = OPTIONS.Atack;
       SetMoveToPoint(enemy.transform.position);
       Target = enemy;
       IsAttacking = true;
       IsMoving = true;
       standardOrder = false;
   }

   public override bool IsMoving
   {
       get
       {
           return base.IsMoving;
       }
       protected set
       {
           if (value) { if (!gameObject.GetComponent<Pilot>()) gameObject.AddComponent<Pilot>(); }
           else { if (gameObject.GetComponent<Pilot>()) Component.Destroy(gameObject.GetComponent<Pilot>()); }
           base.IsMoving = value;
          
       }
   }

    
    
    private float distance;
    public override float Distance
    {
        get 
        {
                    return Vector3.Distance(gameObject.transform.position, MoveToPoint);
        }
        protected set 
        {
            if (value != distance)
            {
                if (IsMovingAsGroup)
                {
                    if (Target)
                    {
                        SetMoveToPoint(Target.transform.position);
                        gameObject.transform.position += GetMovingDirection()*Speed;
                    }
                    else IsGroupLeader = true;
                }
            }
        }
    }

    

    public List<Vector3> WayPoints;

    private bool attacking=false;
    public override bool IsAttacking
    {
        get
        {
            if (attacking) { movingDirection = GetMovingDirection(); return IsMoving = true; }

            else { return false; }

        }
        protected set
        {
            if (unitState == OPTIONS.Atack) attacking = value;
            else attacking = false;

        }
    }


    public override void SetMoveToPoint(Vector3 point)
    {
        MoveToPoint = new Vector3(point.x, standardYPosition, point.z);
    }

    virtual protected bool MoveTo()
    {
        if (gameObject.GetComponent<Pilot>()) gameObject.GetComponent<Pilot>().DoUpdate();

        if(unitState==OPTIONS.Guard)
        {
            if (gameObject.GetComponent<Pilot>()) Quomponent.Destroy(gameObject.GetComponent<Pilot>());
            SetMoveToPoint(Target.transform.position);  
            
            GetMovingDirection();
            if (Distance >= 150) gameObject.transform.position += movingDirection;
            else if (Distance <= 100) gameObject.transform.position -= movingDirection * Speed*1.5f;
        }
        else if (IsMovingAsGroup)
        {
            if (IsGroupLeader) IsMovingAsGroup = false;
            else Distance = Vector3.Distance(gameObject.transform.position, MoveToPoint);

        }
        else if (IsAttacking)
        {
            if(Distance < AttackRange)
            {
                UNIT.weapon.Engage(Target);
            }

            if (Distance >= AttackRange / 2)
            {
                gameObject.transform.position += movingDirection * Speed;
                MoveToPoint = gameObject.transform.position;
                UNIT.weapon.Engage(Target);
            }
        }
        else if (Distance >= 3)
        {
            gameObject.transform.position += movingDirection * Speed;

        }
        else
        {
            SetKinematic();
            gameObject.transform.position = MoveToPoint;
            //        gameObject.rigidbody.isKinematic = false;

            if (IsGroupLeader) GUISqript.mainGUI.GetComponent<GUISqript>().SellectedGroup.GroupState = UnitGroup.GROUPSTATE.Waiting;
            if (unitState == OPTIONS.Patrol)
            {
                WayPoints.RemoveAt(0);
                WayPoints.Add(gameObject.transform.position);
                SetMoveToPoint(WayPoints[0]);
                GetMovingDirection();

            }
            else unitState = OPTIONS.Stay;

        }

        return (gameObject.transform.position != MoveToPoint);
    }

    private Vector3 GetMovingDirection()
    {
        return movingDirection = (MoveToPoint - gameObject.transform.position).normalized;
    }


    void OnCollisionExit(Collision collision)
    {
        SetKinematic();
        GetMovingDirection();
    }

    internal override void MoveAsGroup(GameObject leader)
    {
        Target = leader;
        SetMoveToPoint(leader.transform.position);
        IsMovingAsGroup = true;
        IsAttacking = false;
    }
    
    
    void Start()
    {
        standardYPosition = gameObject.transform.position.y;
        MoveToPoint = gameObject.transform.position;
        unitState = OPTIONS.Stay;
        UnitState = unitState;
        UNIT = gameObject.GetComponent<UnitSqript>();
    }

    internal override void DoUpdate()
    {

        if (IsAttacking) SetMoveToPoint(Target.transform.position);
        if (IsMoving) IsMoving = MoveTo();

    }

}
