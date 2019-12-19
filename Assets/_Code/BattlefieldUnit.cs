using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldUnit : GameEntity, ICanBePlacedOnBattlefield {
    public float turnSpeed = 3.0f;
    public Renderer renderer;
    public MovementDirection movementDirection = MovementDirection.Forward;
    public bool isEnemy = false;
    public bool isDead = false;
    public int life = 3;
    public int GridIndex { get; set; }
    public BattlefieldGrid battlefield;
    protected TurnManager timeManager;

    public virtual void Initialise(BattlefieldGrid battlefield, TurnManager timeManager) {
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
    
    public virtual void OnDeathTick() {
        isDead = true;
    }

    public void EndLife() {
        timeManager.Remove(this);
        battlefield.RemoveEntity(this);
        Destroy(gameObject, 1.5f);
    }

    public enum MovementDirection {
        Forward = 0,
        Backward
    }
}
