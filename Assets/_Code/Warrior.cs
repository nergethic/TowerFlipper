using System;
using DG.Tweening;
using UnityEngine;

public class Warrior : BattlefieldUnit {
    [SerializeField] ParticleSystem bloodFX;
    
    public override void Initialise(BattlefieldGrid battlefield, TimeManager timeManager) {
        base.Initialise(battlefield, timeManager);
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

    bool HandleEncounter(ref BattlefieldGrid.GridCell gridCell) {
        switch (gridCell.type) {
            case EntityType.Warrior:
                var warrior = gridCell.entity as Warrior;
                bool sameFaction = isEnemy == warrior.isEnemy;
                if (!sameFaction) {
                    bloodFX.Play();
                    Debug.Log("Encountered enemy warrior");
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
            return battlefield.UpdatePosition(this, ref position);
        }
        
        return HandleEncounter(ref nextCellInfo.gridCell);
    }
}
