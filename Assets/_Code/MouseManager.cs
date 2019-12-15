using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions.Comparers;

public class MouseManager : MonoBehaviour {
    [SerializeField] Camera camera;
    [SerializeField] UnitManager unitManager;
    [SerializeField] Transform snapUnit;
    [SerializeField] Transform highlightedCell;
    RaycastHit hit;
    Ray ray;
    Vector3 gizmosGridCenterPos = Vector3.zero;
    Grid grid = new Grid(2);
    bool mouseIsDown;
    //LayerMask hitMask;

    private void Start() {
        ray = new Ray();
        //hitMask = LayerMask.GetMask("");
        //hitMask = ~hitMask;
    }

    private void Update() {
        mouseIsDown = Input.GetMouseButtonDown(0);
        Action();
    }

    void Action() {
        ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, float.PositiveInfinity)) {
            Transform hitObject = hit.transform;
            
            Vector3 centerMouse = hit.point;
            var snappedPos = grid.SnapToGrid(centerMouse);
            snappedPos.x += grid.halfGridRes;
            snappedPos.z += grid.halfGridRes;

            switch (hitObject.tag) {
                case "PlayerSpawn": {
                    //unitManager.SpawnSelectedUnit(snappedPos);
                    break;
                }

                case "Battlefield": {
                    if (mouseIsDown) {
                        unitManager.SpawnSelectedUnit(snappedPos);
                    }
                    
                    highlightedCell.position = new Vector3(snappedPos.x, highlightedCell.position.y, snappedPos.z);
                    gizmosGridCenterPos = snappedPos;
                    //snapUnit.transform.position = snappedPos;
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.gray;

        for (float y = -5f; y < 8f; y += grid.gridResolution) {
            for (float x = -5f; x < 8f; x += grid.gridResolution) {
                var snappedPos = grid.SnapToGrid(new Vector3(x, 0, y));
                Gizmos.DrawSphere(gizmosGridCenterPos+snappedPos, 0.06f);
            }
        }
    }
}