using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Warrior : BattlefieldUnit {
    public override void Initialise(TimeManager timeManager) {
        base.Initialise(timeManager);
    }

    public override void Tick() {
        base.Tick();
        
        var pos = thisTransform.position;
        pos += thisTransform.forward * 2f;

        thisTransform.DOMove(pos, 0.8f);
    }
}
