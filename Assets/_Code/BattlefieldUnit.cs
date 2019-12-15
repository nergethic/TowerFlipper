using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldUnit : GameEntity {
    public float turnSpeed = 3.0f;
    public Renderer renderer;
    public MovementDirection movementDirection = MovementDirection.Forward;
    public bool isEnemy = false;
    TimeManager timeManager;

    public virtual void Initialise(TimeManager timeManager) {
        this.timeManager = timeManager;
        timeManager.Add(this);

        if (movementDirection == MovementDirection.Forward) {
            thisTransform.LookAt(thisTransform.position + Vector3.right);
        } else if (movementDirection == MovementDirection.Backward) {
            thisTransform.LookAt(thisTransform.position + Vector3.left);
        }
    }

    public enum MovementDirection {
        Forward = 0,
        Backward
    }
}
