using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {
    [SerializeField] UnitManager unitManager;
    [SerializeField] BattlefieldGrid battlefield;
    [SerializeField] TowerBuilder builder;
    [SerializeField] Warrior unit;
    Grid grid = new Grid(2); // TODO make this global in HQ

    void Start() {
        InvokeRepeating("SpawnWarrior", 2.0f, 3.0f);
    }
    
    void SpawnWarrior() {
        var snappedPos = grid.SnapToGrid(battlefield.enemySpawn.position);
        snappedPos.x += grid.halfGridRes;
        snappedPos.z += grid.halfGridRes;
        
        var newUnit = Instantiate(unit, snappedPos, Quaternion.identity);
        newUnit.isEnemy = true;
        newUnit.movementDirection = BattlefieldUnit.MovementDirection.Backward;
        unitManager.SpawnUnit(newUnit, snappedPos);
        
        builder.BuildLevel();
    }
}
