using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Warrior : BattlefieldUnit {
    public Rigidbody rb;
    public int attack = 1;
    [SerializeField] ParticleSystem bloodFX;

    public override void Initialise(BattlefieldGrid battlefield, TurnManager timeManager) {
        base.Initialise(battlefield, timeManager);

        rb.isKinematic = true;
    }

    public override void Tick() {
        base.Tick();
        
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
    }

    public override void OnDeathTick() {
        base.OnDeathTick();
        
        Tick();
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
            case EntityType.Warrior:
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
}
