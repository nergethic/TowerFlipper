using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Buildings : MonoBehaviour, IEnumerable<Building> {
    private Dictionary<BuildingType, Building> _buildings;

    public void Start() {
        _buildings = new Dictionary<BuildingType, Building>();
        RegisterSmolBuilding();
        RegisterBigBuilding();
    }

    public Building GetBuilding(BuildingType type) {
        if (_buildings.TryGetValue(type, out var building))
            return building;
        throw new ArgumentException($"Building of type {type} is not registered!");
    }

    private void RegisterSmolBuilding() {
        var income = new[] {(Resource.Gold, 100), (Resource.Stone, 100), (Resource.Wood, 100)};
        var cost = new[] {(Resource.Gold, 20), (Resource.Stone, 30), (Resource.Wood, 100)};
        var smolBuilding = new Building(BuildingType.SmolBuildng, income, cost);
        _buildings[smolBuilding.Type] = smolBuilding;
    }
    private void RegisterBigBuilding() {
        var income = new[] {(Resource.Gold, 40), (Resource.Stone, 30), (Resource.Wood, 10)};
        var cost = new[] {(Resource.Gold, 10), (Resource.Stone, 30), (Resource.Wood, 100)};
        var bigBuilding = new Building(BuildingType.BigBuilding, income, cost);
        _buildings[bigBuilding.Type] = bigBuilding;
    }

    public IEnumerator<Building> GetEnumerator() {
        return _buildings.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}




