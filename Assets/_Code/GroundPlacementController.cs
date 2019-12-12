using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;

public class GroundPlacementController : MonoBehaviour {
    // TODO: (make check for this) building prefab types must be in the same order as in BuildingType enum!
    [SerializeField] GameObject[] placeableObjectPrefabs;
    [SerializeField] BuildingManager buildingManager;
    [SerializeField] ResourcesManager resourcesManager;
    [SerializeField] KeyCode deleteObjectHotKey;
    private GameObject currentPlaceableObject;
    private BuildingType selectedBuildingType = BuildingType.None;

    [SerializeField] BoxCollider skrrr;

    public void Update() {
        HandleDestroyObjectHotKey();
        
        if (selectedBuildingType == BuildingType.None)
            return;
        
        MoveCurrentPlacableObjectToMouse();
        ReleaseIfClicked();
    }

    private void MoveCurrentPlacableObjectToMouse() {
        Assert.IsTrue(currentPlaceableObject != null, "currentPlaceableObject != null");
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        
        if (skrrr.Raycast(ray, out hitInfo, float.PositiveInfinity)) {
            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);           
        }
    }

    private void ReleaseIfClicked() {
        if (Input.GetMouseButtonDown(0)) {
            var requiredResources = buildingManager.GetResourcesRequiredToBuild(selectedBuildingType);
            if (resourcesManager.CanSpend(requiredResources)) {
                resourcesManager.SpendResources(requiredResources);
                var building = currentPlaceableObject.GetComponentInChildren<Building>();
                buildingManager.InitAndRegisterBuilding(building, selectedBuildingType);
                selectedBuildingType = BuildingType.None;
                currentPlaceableObject = null;
            } else {
                // TODO: red blink
                TryToDestroySelectedObject();   
            }
        }
    }
    
    private void HandleButtonClick(BuildingType buildingType) {
        if (selectedBuildingType == BuildingType.None && buildingType != BuildingType.None) {
            selectedBuildingType = buildingType;
            currentPlaceableObject = Instantiate(placeableObjectPrefabs[(int)buildingType]);
        }
    }

    private void HandleDestroyObjectHotKey() {
        if (Input.GetKeyDown(deleteObjectHotKey))
            TryToDestroySelectedObject();
    }

    public void TryToDestroySelectedObject() {
        if (currentPlaceableObject != null)
            Destroy(currentPlaceableObject);
    }
    
    public void button() {
        HandleNewObjectButtonClick();
    }
    
    public void button1() {
        HandleNewObjectButtonClick1();
    }
    
    private void HandleNewObjectButtonClick() {
        HandleButtonClick(BuildingType.SmolBuildng);
    }

    private void HandleNewObjectButtonClick1() {
        HandleButtonClick(BuildingType.BigBuilding);
    }
}
