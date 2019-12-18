using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Tury : MonoBehaviour {
    float timeOfTurn = 0;
    public int numberOfTurns = 1;
    public Text turnsText;
    public Action OnTurnEnd;
    [SerializeField]
    private GroundPlacementController placement;
    [SerializeField]
    private ResourcesManager resourcesManager;
    [SerializeField] private Camera camera;
    [SerializeField] BuildingManager buildingManager;
    [SerializeField] Transform cameraVillige;
    [SerializeField] Transform cameraBattlefield;
    [SerializeField] Transform clockArm;
    [SerializeField] GameObject[] thingsToChangeOnBattlefieldUI;
    [SerializeField] GameObject[] thingsToChangeOnVillageUI;
    Vector3 batteFieldPosition;
    Vector3 villigePosition;
    public bool camereOnBattlefield = false;

    private void Start() {
        batteFieldPosition = new Vector3(-21.13697f, 14.80627f, -2.933737f);
    }

    void Update() {
        clockArm.localRotation = Quaternion.Euler(new Vector3(0, 0, -timeOfTurn*6));
        timeOfTurn += Time.deltaTime;
        turnsText.text = "turn: " + numberOfTurns;
        if (Input.GetKeyDown(KeyCode.Space)) {
            MoveCamera();
        }
        
        if (timeOfTurn > 4.0f) {
            foreach (var building in buildingManager.GetBuildings()) 
                resourcesManager.AddResources(building.resourcesProduction);
            
            numberOfTurns++;
            timeOfTurn = 0;
            placement.TryToDestroySelectedObject();
        }
    }

    void MoveCamera() {
        camereOnBattlefield = !camereOnBattlefield;
        
        if (camereOnBattlefield) {
            camera.transform.DOMove(batteFieldPosition, 1.5f);
            camera.transform.DORotate(cameraBattlefield.rotation.eulerAngles, 1.5f);
        } else {
            camera.transform.DOMove(cameraVillige.position, 1.5f);
            camera.transform.DORotate(cameraVillige.rotation.eulerAngles, 1.5f);
        }

        ChangeUiElements();
    }

    void ChangeUiElements() {
        for (int i = 0; i < thingsToChangeOnBattlefieldUI.Length; i++) {
            thingsToChangeOnBattlefieldUI[i].SetActive(camereOnBattlefield);
        }
        
        for (int i = 0; i < thingsToChangeOnVillageUI.Length; i++) {
            thingsToChangeOnVillageUI[i].SetActive(!camereOnBattlefield);
        }
    }
}
