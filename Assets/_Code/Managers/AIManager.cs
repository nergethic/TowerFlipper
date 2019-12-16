using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {
    [SerializeField] Battlefield battlefieldLane;
    [SerializeField] Battlefield unit;

    void Start() {
        //InvokeRepeating("SpawnWarrior", 1.0f, 3.0f);
    }
    
    void SpawnWarrior() {
        //var newUnit = Instantiate(unit, battlefieldLane.enemySpawnArea.transform.position, Quaternion.identity);
        //newUnit.isEnemy = true;
        //newUnit.movementDirection = BattlefieldUnit.MovementDirection.Backward;
    }
}
