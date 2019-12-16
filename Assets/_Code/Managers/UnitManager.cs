using DG.Tweening;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    [SerializeField] BattlefieldGrid battlefield;
    [SerializeField] TimeManager timeManager;
    public BattlefieldUnit selectedUnit;

    public void SelectUnit(BattlefieldUnit unit) { // TODO make sure this is happening from prefab
        selectedUnit = unit;
    }
    
    public bool SpawnUnit(BattlefieldUnit unitToSpawn, Vector3 position) {
        if (selectedUnit == null)
            return false;

        var cell = battlefield.GetCellAt(ref position);
        if (cell.success == false || cell.gridCell.IsEmpty() == false)
            return false;
        
        var unit = Instantiate(unitToSpawn, position, Quaternion.identity);
        unit.transform.localScale = Vector3.zero;
        unit.transform.DOScale(Vector3.one, 0.4f);
        unit.Initialise(battlefield, timeManager);

        return true;
    }
}