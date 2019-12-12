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
    const float gridResolution = 2f;
    const float halfGridRes = gridResolution / 2f;
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
                    Debug.LogError("Hit");
                    Vector3 centerMouse = hit.point;
                    var snappedPos = SnapToGrid(centerMouse);
                    snappedPos.x += halfGridRes;
                    snappedPos.z += halfGridRes;
                    highlightedCell.position = snappedPos;
                    gizmosGridCenterPos = snappedPos;
                    snapUnit.transform.position = snappedPos;
                    break;
                }
            }
        }
    }

    Vector3 SnapToGrid(Vector3 position) {
        var result = position;
        result.x = Mathf.Floor(result.x / gridResolution) * gridResolution;
        result.z = Mathf.Floor(result.z / gridResolution) * gridResolution;

        return result;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;

        for (float y = -5f; y < 8f; y += gridResolution) {
            for (float x = -5f; x < 8f; x += gridResolution) {
                var snappedPos = SnapToGrid(new Vector3(x, 0, y));
                Gizmos.DrawSphere(gizmosGridCenterPos+snappedPos, 0.04f);
            }
        }
    }
}