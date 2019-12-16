using DG.Tweening;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    [SerializeField] Battlefield battlefield;
    [SerializeField] TimeManager timeManager;
    [SerializeField] BattlefieldUnit selectedUnit;

    public void SelectUnit(BattlefieldUnit unit) { // TODO make sure this is happening from prefab
        selectedUnit = unit;
    }
    
    public bool SpawnSelectedUnit(Vector3 position) {
        if (selectedUnit == null)
            return false;

        var cell = battlefield.GetCellAt(ref position);
        if (cell.success == false || cell.gridCell.IsEmpty() == false)
            return false;
        
        var instance = Instantiate(selectedUnit, position, Quaternion.identity);
        instance.transform.localScale = Vector3.zero;
        instance.transform.DOScale(Vector3.one, 0.4f);

        var battlefieldUnityComponent = instance.GetComponent<BattlefieldUnit>();
        battlefieldUnityComponent.Initialise(battlefield, timeManager);

        return true;
    }
}