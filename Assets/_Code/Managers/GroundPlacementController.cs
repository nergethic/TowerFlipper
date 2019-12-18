using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


public class GroundPlacementController : MonoBehaviour {
    // TODO: (make check for this) building prefab types must be in the same order as in BuildingType enum!
    [SerializeField] GameObject[] placeableObjectPrefabs;
    [SerializeField] BuildingManager buildingManager;
    [SerializeField] ResourcesManager resourcesManager;
    [SerializeField] KeyCode deleteObjectHotKey;
    public Text errorText;
    public Text errorResources;
    private GameObject currentPlaceableObject;
    private BuildingType selectedBuildingType = BuildingType.None;
    private Dictionary<Vector3, BuildingType> occupiedGrids = new Dictionary<Vector3, BuildingType>();
    [SerializeField] BoxCollider skrrr;
    Grid grid = new Grid(2);

    private void Start()
    {
        errorText.text = "";
        errorResources.text = "";
    }

    public void Update() {
        HandleDestroyObjectHotKey();
        
        if (selectedBuildingType == BuildingType.None)
            return;
        
        MoveCurrentPlacableObjectToMouse();
        ReleaseIfClicked();
    }

    IEnumerator DisplayPopupBuildingSpace()
    {    
       // errorText.transform.localPosition = Input.mousePosition ;
        errorText.text = "You can't build here";
        yield return new WaitForSeconds(1f);
        errorText.text = "";
    }
    IEnumerator DisplayPopupSpendings()
    {    
        errorResources.text = "You don't have enough resources to build it";
        yield return new WaitForSeconds(1f);
        errorResources.text = "";
    }


    private bool IsGridEmpty()
    {
        if (occupiedGrids.ContainsKey(currentPlaceableObject.transform.position))
            return false;
        else
            return true;

    }
    
    private void MoveCurrentPlacableObjectToMouse() {
        Assert.IsTrue(currentPlaceableObject != null, "currentPlaceableObject != null");
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (skrrr.Raycast(ray, out hitInfo, float.PositiveInfinity)) {
            currentPlaceableObject.transform.position = grid.SnapToGrid(hitInfo.point);
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
        
    }

    private void ReleaseIfClicked() {
        if (Input.GetMouseButtonDown(0)) {
            var requiredResources = buildingManager.GetResourcesRequiredToBuild(selectedBuildingType);
            if (resourcesManager.CanSpend(requiredResources) && IsGridEmpty()) {
                resourcesManager.SpendResources(requiredResources);
                var building = currentPlaceableObject.GetComponentInChildren<Building>();
                buildingManager.InitAndRegisterBuilding(building, selectedBuildingType);
                occupiedGrids.Add(currentPlaceableObject.transform.position, selectedBuildingType);
                selectedBuildingType = BuildingType.None;
                currentPlaceableObject = null;
            } else {
                // TODO: red blink
                if (!resourcesManager.CanSpend(requiredResources))
                    StartCoroutine(DisplayPopupSpendings());
                else
                    StartCoroutine(DisplayPopupBuildingSpace());
                
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

    public void TryToDestroySelectedObject()
    {
        if (currentPlaceableObject != null){
            Destroy(currentPlaceableObject);
            selectedBuildingType = BuildingType.None;
        }
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
        HandleButtonClick(BuildingType.Mine);
    }
}
