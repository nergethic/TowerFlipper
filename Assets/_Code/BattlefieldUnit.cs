using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldUnit : GameEntity, ICanBePlacedOnBattlefield {
    public float turnSpeed = 3.0f;
    public Renderer renderer;
    public MovementDirection movementDirection = MovementDirection.Forward;
    public bool isEnemy = false;
    public int GridIndex { get; set; }
    protected Battlefield battlefield;
    protected TimeManager timeManager;

    public virtual void Initialise(Battlefield battlefield, TimeManager timeManager) {
        this.battlefield = battlefield;
        this.timeManager = timeManager;
        
        battlefield.AddEntity(this);
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
