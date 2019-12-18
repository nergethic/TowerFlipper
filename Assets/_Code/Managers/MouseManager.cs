using UnityEngine;
using DG.Tweening;

public class MouseManager : MonoBehaviour {
    [SerializeField] Camera camera;
    [SerializeField] UnitManager unitManager;
    [SerializeField] Transform snapUnit;
    [SerializeField] Transform highlightedCell;
    [SerializeField] Material selectedUnitMaterial;
    [SerializeField] Color green;
    [SerializeField] Color red;
    RaycastHit hit;
    Ray ray;
    Vector3 gizmosGridCenterPos = Vector3.zero;
    Grid grid = new Grid(2);
    bool mouseIsDown;
    Vector3 lastMousePosition;
    int lastCellIndex;
    int selectedUnitShaderColorID;

    private void Start() {
        ray = new Ray();
        selectedUnitShaderColorID = Shader.PropertyToID("Color_F8E0E738");
    }

    private void Update() {
        var screenMouseX = Input.mousePosition.x / Screen.width;
        screenMouseX = Mathf.Clamp01(screenMouseX);

        if ((Input.mousePosition.y / Screen.height) > 0.25f) {
            if (screenMouseX < 0.3f) {
                var cameraPos = camera.transform.position;
                float scrollStrength = MathUtility.Map(0.3f - screenMouseX, 0.0f, 0.3f, 0.0f, 1f);
                cameraPos.x -= 30f * scrollStrength * Time.deltaTime;
                camera.transform.position = cameraPos;
            } else if (screenMouseX > 0.7f) {
                var cameraPos = camera.transform.position;
                float scrollStrength = MathUtility.Map(screenMouseX, 0.7f, 1f, 0.0f, 1f);
                cameraPos.x += 30f * scrollStrength * Time.deltaTime;
                camera.transform.position = cameraPos;
            }
        }

        mouseIsDown = Input.GetMouseButtonDown(0);

        var mousePosition = Input.mousePosition;
        float diff = (mousePosition - lastMousePosition).sqrMagnitude;
        if (mouseIsDown || true) { // TODO optimization // diff > 2f
            lastMousePosition = mousePosition;
            Action(ref mousePosition);
        }
    }

    void Action(ref Vector3 mousePosition) {
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
                    var battlefield = unitManager.GetBattlefield();
                    var cellInfo = battlefield.GetCellAt(ref snappedPos);
                    if (!cellInfo.success)
                        return;

                    unitManager.unitOnMouse.thisTransform.position = centerMouse;

                    bool cellIsEmpty = cellInfo.gridCell.IsEmpty();
                    
                    if (cellIsEmpty) {
                        selectedUnitMaterial.SetColor(selectedUnitShaderColorID, green);
                    } else {
                        selectedUnitMaterial.SetColor(selectedUnitShaderColorID, red);
                    }
                    
                    if (mouseIsDown) {
                        if (cellIsEmpty) {
                            unitManager.SpawnUnit(unitManager.selectedUnit, snappedPos);
                            var action = unitManager.unitOnMouse.thisTransform.DOScale(Vector3.zero, 0.4f);
                            action.OnComplete(() => {
                                unitManager.unitOnMouse.thisTransform.DOScale(new Vector3(1, 1, 1), 0.6f);
                            });
                        } else {
                            highlightedCell.localScale = new Vector3(BattlefieldGrid.CELL_WORLD_WIDTH, 0.1f, BattlefieldGrid.CELL_WORLD_WIDTH);
                            highlightedCell.transform.DOPunchScale(new Vector3(-0.5f, 0f, -0.5f), 0.5f, 2, 0.3f);
                        }
                    } else if (cellInfo.gridCell.index == lastCellIndex) 
                        return;

                    lastCellIndex = cellInfo.gridCell.index;
                    
                    highlightedCell.DOMove(new Vector3(snappedPos.x, highlightedCell.position.y, snappedPos.z), 0.12f);
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