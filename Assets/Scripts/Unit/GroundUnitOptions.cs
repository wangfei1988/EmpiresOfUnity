using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Character/Unit Options (Standard GroundUnit)")]
class GroundUnitOptions : UnitOptions
{
    public UnitScript UNIT;
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
                                LockOnFocus();
                                MouseEvents.LEFTCLICK += MouseEvents_LEFTMouseEvents;
                                break; 
                            }
                        case OPTIONS.Patrol:
                            {
                                SetKinematic();
                                LockOnFocus();
                                MouseEvents.LEFTCLICK += MouseEvents_LEFTMouseEvents;
                                MouseEvents.RIGHTCLICK += MouseEvents_RIGHTCLICK;
                                WayPoints.Add(gameObject.transform.position);
                                break; 
                            }
                        case OPTIONS.Atack:
                            {
                                SetKinematic();
                                WayPoints.Clear();
                                LockOnFocus();
                                MouseEvents.LEFTCLICK += MouseEvents_LEFTMouseEvents; 
                                break; 
                            }
                        case OPTIONS.Stay:
                            {
                                SetKinematic();
                                UnlockFocus();
                                WayPoints.Clear();
                                IsAttacking = false;
                                SetMoveToPoint(gameObject.transform.position);
                                break;
                            }
                        case OPTIONS.Guard:
                            {
                                SetKinematic();
                                LockOnFocus();
                                WayPoints.Clear();
                                MouseEvents.LEFTCLICK += MouseEvents_LEFTMouseEvents; 
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
   protected override void MouseEvents_LEFTMouseEvents(Ray qamRay, bool hold)
   {
       if (!hold)
       {
           if (gameObject.GetComponent<Focus>())
           {
               if (unitState == OPTIONS.MoveTo)
               {
                   SetMoveToPoint(MouseEvents.State.Position.AsWorldPointOnMap);
                   movingDirection = GetMovingDirection();
                   IsMoving = true;
                   MouseEvents.LEFTCLICK -= MouseEvents_LEFTMouseEvents;
                   UnlockFocus();
               }
               else if (unitState == OPTIONS.Guard)
               {
                   RaycastHit Hit;
                   if (Physics.Raycast(qamRay, out Hit, Camera.main.transform.position.y))
                   {
                       if (TargetIsAllied(Hit.collider.gameObject))
                       {
                           Target = Hit.collider.gameObject.GetComponent<UnitScript>().SetInteracting(this.gameObject);
                           SetMoveToPoint(Target.transform.position);
                           IsMoving = true;
                       }
                   }
                   MouseEvents.LEFTCLICK -= MouseEvents_LEFTMouseEvents;
                   UnlockFocus();
               }
               else if (unitState == OPTIONS.Patrol)
               {
                   WayPoints.Add(MouseEvents.State.Position.AsWorldPointOnMap);
                   IsMoving = true;
                   MouseEvents.LEFTCLICK -= MouseEvents_LEFTMouseEvents;
                   MouseEvents.RIGHTCLICK -= MouseEvents_RIGHTCLICK;
                   UnlockFocus();
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
                   MouseEvents.LEFTCLICK -= MouseEvents_LEFTMouseEvents;
                   UnlockFocus();
               }
           }
       }
   }
   protected override void MouseEvents_RIGHTCLICK(Ray qamRay, bool hold)
   {
       if (!hold)
       {
           if (gameObject.GetComponent<Focus>())
           {
               if (unitState == OPTIONS.Patrol)
               {
                   if (WayPoints.Count >= 2) WayPoints.Insert(WayPoints.Count - 1, MouseEvents.State.Position.AsWorldPointOnMap);
                   else WayPoints.Add(MouseEvents.State.Position.AsWorldPointOnMap);
                   movingDirection = GetMovingDirection();
                   IsMoving = true;
               }
           }
       }
   }


   internal override void FocussedLeftOnEnemy(GameObject enemy)
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
            if (gameObject.GetComponent<Pilot>()) UnitComponent.Destroy(gameObject.GetComponent<Pilot>());
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

            if (IsGroupLeader) GUIScript.mainGUI.GetComponent<GUIScript>().SelectedGroup.GroupState = UnitGroup.GROUPSTATE.Waiting;
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
        UNIT = gameObject.GetComponent<UnitScript>();
    }

    internal override void DoUpdate()
    {

        if (IsAttacking) SetMoveToPoint(Target.transform.position);
        if (IsMoving) IsMoving = MoveTo();

    }

}
