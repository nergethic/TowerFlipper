using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class BuildingManager : MonoBehaviour {
    public int numberOfBuildings;
    private List<Building> buildings;

    private void Start() {
        buildings = new List<Building>();
    }

    public List<Building> GetBuildings() {
        return buildings;
    }

    public void InitAndRegisterBuilding(Building building, BuildingType buildingType) {
        building.type = buildingType;
        var resourcesProduction = new Resources();
        
        switch (building.type) {
            case BuildingType.SmolBuildng:
                    resourcesProduction.SetValue(ResourceType.Gold, 1f);
                break;
            
            case BuildingType.BigBuilding:
                    resourcesProduction.SetValue(ResourceType.Wood, 2f);
                break;
            
            default:
                throw new Exception("unhandled building type");
        }

        building.resourcesProduction = resourcesProduction;
        buildings.Add(building);
        numberOfBuildings = buildings.Count;
    }

    public Resources GetResourcesRequiredToBuild(BuildingType buildingType) {
        Resources requiredResources = new Resources();
        
        switch (buildingType) {
            case BuildingType.SmolBuildng:
                requiredResources.SetValue(ResourceType.Gold, 10.0f);
                requiredResources.SetValue(ResourceType.Stone, 10.0f);
                requiredResources.SetValue(ResourceType.Wood, 10.0f);
                break;
            
            case BuildingType.BigBuilding:
                requiredResources.SetValue(ResourceType.Gold, 50.0f);
                requiredResources.SetValue(ResourceType.Stone, 50.0f);
                requiredResources.SetValue(ResourceType.Wood, 50.0f);
                break;
            
            default:
                throw new Exception("unhandled building type");
        }

        return requiredResources;
    }
}

public enum BuildingType {
    BigBuilding = 0,
    SmolBuildng,
    None
}