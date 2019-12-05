using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions.Comparers;

public class MouseManager : MonoBehaviour {
    [SerializeField] Camera camera;
    [SerializeField] GameObject unit;
    //LayerMask hitMask;

    private void Start() {
        //hitMask = LayerMask.GetMask("");
        //hitMask = ~hitMask;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0))
            Action();
    }

    void Action() {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity)) {
            Transform hitObject = hit.transform;

            switch (hitObject.tag) {
                case "PlayerSpawn": {
                    Instantiate(unit, hit.point, Quaternion.identity);
                    break;
                }
            }
            Debug.LogError($"HIT {hitObject.name}");
        }
    }
}