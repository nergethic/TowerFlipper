using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Building {
    private int _numberOfBuildings;
    public int NumberOfBuildings => _numberOfBuildings;
    public readonly BuildingType Type;
    public IEnumerable<(Resource resource, int quantity)> IncomePerBuilding;
    public IEnumerable<(Resource resource, int quantity)> TotalIncome => IncomePerBuilding.Select(tuple => (tuple.resource, tuple.quantity * _numberOfBuildings));
    public IEnumerable<(Resource resource, int quantity)> CostForBuilding;
        

    public Building(BuildingType type, IEnumerable<(Resource resource, int quantity)> incomePerBuilding, IEnumerable<(Resource resource, int quantity)> cost) {
        Type = type;
        IncomePerBuilding = incomePerBuilding;
        CostForBuilding = cost;
    }
    
    public bool Build(Resources resources) {
        if (!resources.Spend(CostForBuilding)) return false;
        _numberOfBuildings++;
        return true;
    }
    
}

public enum BuildingType {
    BigBuilding, SmolBuildng
} 