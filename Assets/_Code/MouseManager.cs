using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions.Comparers;

public class MouseManager : MonoBehaviour {
    [SerializeField] Camera camera;
    [SerializeField] GameObject unit;
    [SerializeField] Transform snapUnit;
    [SerializeField] Transform highlightedCell;
    RaycastHit hit;
    Ray ray;
    Vector3 gizmosGridCenterPos = Vector3.zero;
    //LayerMask hitMask;

    private void Start() {
        ray = new Ray();
        //hitMask = LayerMask.GetMask("");
        //hitMask = ~hitMask;
    }

    private void Update() {
        //if (Input.GetMouseButtonDown(0))
            Action();
    }

    void Action() {
        ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, float.PositiveInfinity)) {
            Transform hitObject = hit.transform;

            switch (hitObject.tag) {
                case "PlayerSpawn": {
                    //Instantiate(unit, hit.point, Quaternion.identity);
                    break;
                }

                case "Battlefield": {
                    Vector3 centerMouse = hit.point;
                    var snappedPos = Grid.SnapToGrid(centerMouse);
                    snappedPos.x += Grid.halfGridRes;
                    snappedPos.z += Grid.halfGridRes;
                    highlightedCell.position = new Vector3(snappedPos.x, highlightedCell.position.y, snappedPos.z);
                    gizmosGridCenterPos = snappedPos;
                    snapUnit.transform.position = snappedPos;
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.gray;

        for (float y = -5f; y < 8f; y += Grid.gridResolution) {
            for (float x = -5f; x < 8f; x += Grid.gridResolution) {
                var snappedPos = Grid.SnapToGrid(new Vector3(x, 0, y));
                Gizmos.DrawSphere(gizmosGridCenterPos+snappedPos, 0.06f);
            }
        }
    }
}