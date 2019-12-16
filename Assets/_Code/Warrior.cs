using System;
using DG.Tweening;
using UnityEngine;

public class Warrior : BattlefieldUnit {
    public override void Initialise(Battlefield battlefield, TimeManager timeManager) {
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

    bool TryToMove(Vector3 pos) {
        var nextCell = battlefield.GetCellAt(ref pos);
        if (nextCell.success && nextCell.gridCell.IsEmpty()) {
            var action = thisTransform.DOMove(pos, 0.8f);
            return battlefield.UpdatePosition(this, ref pos);
        }

        return false;
    }
}
