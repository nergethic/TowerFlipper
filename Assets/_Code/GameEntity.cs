using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour {
    public EntityType type;
    public Transform thisTransform;

    private void Awake() {
        thisTransform = transform;
    }
    
    public virtual void Tick() {}
}

public enum EntityType {
    Warrior = 0,
    Archer,
    None,
    Count
}
