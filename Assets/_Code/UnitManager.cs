using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    [SerializeField] TimeManager timeManager;
    [SerializeField] BattlefieldUnit selectedUnit;

    public void SelectUnit(BattlefieldUnit unit) { // TODO make sure this is happening from prefab
        selectedUnit = unit;
    }
    
    public void SpawnSelectedUnit(Vector3 position) {
        if (selectedUnit == null)
            return;
        
        var gameObject = Instantiate(selectedUnit, position, Quaternion.identity);
        gameObject.GetComponent<BattlefieldUnit>().Initialise(timeManager);
    }
}