using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GroundPlacementController : MonoBehaviour
{
    [SerializeField]
    private GameObject placeableObjectPrefab;
    [SerializeField]
    private GameObject placeableObjectPrefab1;
    [SerializeField]
    private KeyCode deleteObjectHotKey;
    private KeyCode deleteObjectHotKey1;
    private GameObject currentPlaceableObject;
    
    [SerializeField]
    private BoxCollider skrrr;

    [SerializeField] Resources resources;
    
    [SerializeField] private Building1 building1;
    
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
        if (Input.GetMouseButtonDown(0))
        {
            if (building1.income.CostCheck())
            {
                currentPlaceableObject = null;
            }
            else DestroySelectedObject();

        }
           
    }
    
    private void HandleNewObjectButtonClick()
    {      
        if(currentPlaceableObject == null)
          currentPlaceableObject = Instantiate(placeableObjectPrefab);   
        
    }

    private void HandleNewObjectButtonClick1()
    {
        if (currentPlaceableObject == null)
            currentPlaceableObject = Instantiate(placeableObjectPrefab1);
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
