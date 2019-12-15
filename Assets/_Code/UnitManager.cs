using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    [SerializeField] Battlefield battlefield;
    [SerializeField] TimeManager timeManager;
    [SerializeField] BattlefieldUnit selectedUnit;

    public void SelectUnit(BattlefieldUnit unit) { // TODO make sure this is happening from prefab
        selectedUnit = unit;
    }
    
    public void SpawnSelectedUnit(Vector3 position) {
        if (selectedUnit == null)
            return;
        
        var instance = Instantiate(selectedUnit, position, Quaternion.identity);
        instance.transform.localScale = Vector3.zero;
        instance.transform.DOScale(Vector3.one, 0.4f);

        var battlefieldUnityComponent = instance.GetComponent<BattlefieldUnit>();
        battlefieldUnityComponent.Initialise(timeManager);
        battlefield.AddEntity(battlefieldUnityComponent);
    }
}