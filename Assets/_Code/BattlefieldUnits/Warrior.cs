using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

public class Warrior : BattlefieldUnit {
    public Rigidbody rb;
    public int attack = 1;
    [SerializeField] ParticleSystem bloodFX;

    public override void Initialise(BattlefieldGrid battlefield, TurnManager timeManager) {
        base.Initialise(battlefield, timeManager);

        rb.isKinematic = true;
    }

    //public override void ResolveAction(PlannedAction action) {
        //base.ResolveAction(action);
        
        /*
        var pos = thisTransform.position;
        pos += thisTransform.forward * 2f;

        if (!TryToMove(pos)) {
            // Debug.LogError($"{name} sees at next cell: {nextCell.type}");
            pos += Vector3.forward * 2f;
            if (!TryToMove(pos)) {
                pos -= Vector3.forward * 4f;
                TryToMove(pos);
            }
        }
        */
    //}
    
    public override void ExecuteAction(PlannedAction action) {
        switch (action.type) {
            case ActionType.Attack:
                var (cell, success) = battlefield.GetCellAt(action.cellIndex);
                if (success)
                    HandleEncounter(ref cell);
                break;
            
            case ActionType.MoveForward:
                    MoveForwards();
                break;

            case ActionType.TurnLeft: {
                var rot = thisTransform.eulerAngles + Quaternion.AngleAxis(90f, Vector3.up).eulerAngles;
                thisTransform.DORotate(rot, 0.8f);
            } break;

            case ActionType.TurnRight: {
                var rot = thisTransform.eulerAngles + Quaternion.AngleAxis(90f, Vector3.up).eulerAngles;
                thisTransform.DORotate(-rot, 0.8f);
            } break;
            
            case ActionType.DoNothing:
                break;
            
            default:
                throw new Exception("invalid switch case!");
        }
    }

    public override PlannedAction PlanActionOn(BattlefieldGrid.GridCell cell) { // TODO
        var entity = cell.entity as BattlefieldUnit;
        PlannedAction plannedAction = new PlannedAction{ thisUnit = this, otherUnit = entity, cellIndex = cell.index };

        if ((isEnemy && entity.isEnemy) || (!isEnemy && !entity.isEnemy))
            plannedAction.type = ActionType.DoNothing;
        else
            plannedAction.type = ActionType.Attack;
        

        switch (cell.type) { // TODO
            case EntityType.Warrior:
                break;
            
            case EntityType.Archer:
                break;
        }

        return plannedAction;
    }

    public override void OnDeathTick() {
        base.OnDeathTick();
        
        //Tick();
    }

    IEnumerator Die() {
        yield return new WaitForSeconds(0.9f);
        rb.isKinematic = false;
        rb.AddExplosionForce(20f, new Vector3(thisTransform.position.x, thisTransform.position.y-0.2f, thisTransform.position.z), 2f);
        rb.AddForce(Vector3.up*50f, ForceMode.Impulse);
    }
    
    IEnumerator Wound() {
        bloodFX.Play();
        yield return new WaitForSeconds(0.05f);
        bloodFX.Stop();
    }

    bool HandleEncounter(ref BattlefieldGrid.GridCell gridCell) {
        switch (gridCell.type) {
            case EntityType.Archer: {
                var archer = gridCell.entity as Archer;
                if (archer.isDead)
                    return true;

                bool sameFaction = isEnemy == archer.isEnemy;
                if (!sameFaction) {
                    var currentPos = thisTransform.position;
                    if (movementDirection == MovementDirection.Forward) {
                        var action =
                            thisTransform.DOMove(currentPos + Vector3.right * (BattlefieldGrid.CELL_WORLD_WIDTH - 0.9f),
                                0.3f);
                        action.onComplete = () => { thisTransform.DOMove(currentPos, 0.7f); };
                    }
                    else {
                        var action =
                            thisTransform.DOMove(currentPos - Vector3.right * (BattlefieldGrid.CELL_WORLD_WIDTH - 0.9f),
                                0.3f);
                        action.onComplete = () => { thisTransform.DOMove(currentPos, 0.7f); };
                    }

                    archer.life -= attack;
                    if (archer.life < 0)
                        archer.isDead = true;
                    // Debug.Log("Encountered enemy warrior");
                }
                else {
                    return false;
                }

                break;
            }

            case EntityType.Warrior: {
                var warrior = gridCell.entity as Warrior;
                if (warrior.isDead)
                    return true;
                
                bool sameFaction = isEnemy == warrior.isEnemy;
                if (!sameFaction) {
                    var currentPos = thisTransform.position;
                    if (movementDirection == MovementDirection.Forward) {
                        var action = thisTransform.DOMove(currentPos + Vector3.right * (BattlefieldGrid.CELL_WORLD_WIDTH-0.9f), 0.3f);
                        action.onComplete = () => {
                            thisTransform.DOMove(currentPos, 0.7f);
                        };
                    } else {
                        var action = thisTransform.DOMove(currentPos - Vector3.right * (BattlefieldGrid.CELL_WORLD_WIDTH-0.9f), 0.3f);
                        action.onComplete = () => {
                            thisTransform.DOMove(currentPos, 0.7f);
                        };
                    }
                     
                    warrior.life -= attack;
                    if (warrior.life > 0) 
                        warrior.StartCoroutine(warrior.Wound());
                    else {
                        warrior.isDead = true;
                        warrior.bloodFX.Play();
                        warrior.StartCoroutine(warrior.Die());
                    }
                    // Debug.Log("Encountered enemy warrior");
                } else {
                    return false;
                }
                break;
            }

            default:
                throw new Exception("entity type not handled");
        }

        return true;
    }

    bool TryToMove(Vector3 position) {
        var nextCellInfo = battlefield.GetCellAt(ref position);
        if (nextCellInfo.success == false)
            return false;

        if (nextCellInfo.gridCell.IsEmpty()) {
            var action = thisTransform.DOMove(position, 0.8f);
            thisTransform.DOMove(position, 0.8f);
            return battlefield.UpdateEntityLocation(this, ref position);
        }
        
        return HandleEncounter(ref nextCellInfo.gridCell);
    }

    void MoveForwards() {
        var pos = thisTransform.position;
        pos += thisTransform.forward * 2f;
        
        var nextCellInfo = battlefield.GetCellAt(ref pos);

        if (nextCellInfo.gridCell.IsEmpty()) {
            var action = thisTransform.DOMove(pos, 0.8f);
            thisTransform.DOMove(pos, 0.8f);
            battlefield.UpdateEntityLocation(this, ref pos);
        }
    }
}
