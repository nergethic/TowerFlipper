using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {
    [SerializeField] UnitManager unitManager;
    [SerializeField] BattlefieldGrid battlefield;
    [SerializeField] Warrior unit;
    Grid grid = new Grid(2); // TODO make this global in HQ

    void Start() {
        InvokeRepeating("SpawnWarrior", 1.0f, 5.0f);
        InvokeRepeating("SpawnWarrior", 1.5f, 5.5f);
    }
    
    void SpawnWarrior() {
        var spawnPoint = battlefield.enemySpawn.position;
        spawnPoint.z += UnityEngine.Random.Range(-4f, 4f);
        var snappedPos = grid.SnapToGrid(spawnPoint);
        snappedPos.x += grid.halfGridRes;
        snappedPos.z += grid.halfGridRes;
        
        var newUnit = Instantiate(unit, snappedPos, Quaternion.identity);
        newUnit.isEnemy = true;
        newUnit.movementDirection = BattlefieldUnit.MovementDirection.Backward;
        unitManager.SpawnUnit(newUnit, snappedPos);
    }
}
