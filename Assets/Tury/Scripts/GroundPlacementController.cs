using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GroundPlacementController : MonoBehaviour {
    [SerializeField] private Building2[] placeableObjectPrefabs;
    [SerializeField]
    private KeyCode deleteObjectHotKey;
    private GameObject currentPlaceableObject;
    private int currentBuildingIdx;
    [SerializeField] private Buildings buildings;
    
    [SerializeField]
    private BoxCollider skrrr;
    

    [SerializeField] Resources resources;
    
    
    void Update()
    {
        HandleDestroyObjectHotKey();
        if (currentPlaceableObject != null)
        {
            MoveCurrentPlacableObjectToMouse();
            ReleaseIfClicked();
        }
    }
    public void button()
    {
        HandleNewObjectButtonClick();
    }
    public void button1()
    {
        HandleNewObjectButtonClick1();
    }
    
    private void MoveCurrentPlacableObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (skrrr.Raycast(ray, out hitInfo, 10000000.0f))
        {
            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);           
        }
    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0)) {
            var buildingInstance = buildings.GetBuilding(placeableObjectPrefabs[currentBuildingIdx].buildingType);
            
            if (buildingInstance.Build(resources))
            {
                currentPlaceableObject = null;
                currentBuildingIdx = -1;    
            }
            else DestroySelectedObject();

        }
           
    }
    
    private void HandleNewObjectButtonClick()
    {
        if (currentPlaceableObject == null) {
            currentPlaceableObject = Instantiate(placeableObjectPrefabs[0].gameObject);
            currentBuildingIdx = 0;
        }
        
    }

    private void HandleNewObjectButtonClick1()
    {
        if (currentPlaceableObject == null) {
            currentPlaceableObject = Instantiate(placeableObjectPrefabs[1].gameObject);
            currentBuildingIdx = 1;
        }
    }

    private void HandleDestroyObjectHotKey()
    {
        if(Input.GetKeyDown(deleteObjectHotKey))
            Destroy(currentPlaceableObject);
    }

    public void DestroySelectedObject() {
        Destroy(currentPlaceableObject);
    }
    
}
