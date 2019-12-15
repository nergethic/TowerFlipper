using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldUnit : MonoBehaviour {
    public float turnSpeed = 3.0f;
    public Renderer renderer;
    public MovementDirection movementDirection = MovementDirection.Forward;
    public bool isEnemy = false;
    TimeManager timeManager;
    Transform thisTransform;

    public void Initialise(TimeManager timeManager) {
        thisTransform = transform;
        this.timeManager = timeManager;
        turnSpeed = UnityEngine.Random.RandomRange(0.1f, 4f);
        timeManager.Add(this);
    }

    public void Tick() {
        var pos = thisTransform.position;
        pos += thisTransform.forward * 2f;

        thisTransform.position = pos;
    }

    public enum MovementDirection {
        Forward = 0,
        Backward
    }
}
