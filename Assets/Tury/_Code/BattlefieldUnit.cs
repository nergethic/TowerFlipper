using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlefieldUnit : MonoBehaviour {
    [SerializeField] float movementSpeed = 1.0f;
    public Renderer renderer;
    public MovementDirection movementDirection = MovementDirection.Forward;
    public bool isEnemy = false;

    void Update() {
        Move();
    }

    void Move() {
        var pos = transform.position;

        if (movementDirection == MovementDirection.Forward)
            pos.x += movementSpeed * Time.deltaTime;
        else
            pos.x -= movementSpeed * Time.deltaTime;
        
        transform.position = pos;
    }

    public enum MovementDirection {
        Forward = 0,
        Backward
    }
}
