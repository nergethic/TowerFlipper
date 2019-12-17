using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Warrior : BattlefieldUnit {
    public Rigidbody rb;
    [SerializeField] ParticleSystem bloodFX;

    public override void Initialise(BattlefieldGrid battlefield, TimeManager timeManager) {
        base.Initialise(battlefield, timeManager);

        rb.isKinematic = true;
    }

    public override void Tick() {
        if (isDead)
            return;
        
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

    IEnumerator Die() {
        yield return new WaitForSeconds(0.9f);
        rb.isKinematic = false;
        rb.AddExplosionForce(20f, new Vector3(thisTransform.position.x, thisTransform.position.y-0.2f, thisTransform.position.z), 2f);
        rb.AddForce(Vector3.up*50f, ForceMode.Impulse);
    }

    bool HandleEncounter(ref BattlefieldGrid.GridCell gridCell) {
        switch (gridCell.type) {
            case EntityType.Warrior:
                var warrior = gridCell.entity as Warrior;
                if (warrior.isDead)
                    return true;
                
                bool sameFaction = isEnemy == warrior.isEnemy;
                if (!sameFaction) {
                    warrior.bloodFX.Play();
                    warrior.StartCoroutine(warrior.Die());
                    warrior.isDead = true;
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
