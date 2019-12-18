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
    [SerializeField] private Transform cameraVillige;
    [SerializeField] private Transform cameraBattlefield;
    [SerializeField] private Transform clockArm;
    [SerializeField] private GameObject[] thingsToChangeOnBattlefieldUI;
    [SerializeField] private GameObject[] thingsToChangeOnVillageUI;
    private Vector3 batteFieldPosition;
    private Vector3 villigePosition;

    private void Start()
    {
        batteFieldPosition = new Vector3(-21.13697f, 14.80627f, -2.933737f);
    }

    void Update() {
        clockArm.eulerAngles = new Vector3(0, 0, -timeOfTurn*6);
        timeOfTurn += Time.deltaTime;
        turnsText.text = "turn: " + numberOfTurns;
        if (Input.GetKeyDown(KeyCode.Space)) {
        //if (timeOfTurn > 5.0f) {
            foreach (var building in buildingManager.GetBuildings()) 
                resourcesManager.AddResources(building.resourcesProduction);
            
            numberOfTurns++;
            MoveCamera();
            timeOfTurn = 0;
            placement.TryToDestroySelectedObject();
            ChangeUiElements(thingsToChangeOnBattlefieldUI, true);
            ChangeUiElements(thingsToChangeOnVillageUI, false);
        }
    }

    private void MoveCamera() {
        if (numberOfTurns % 2 == 0) {
            camera.transform.DOMove(cameraVillige.position, 1.5f);
            camera.transform.DORotate(cameraVillige.rotation.eulerAngles, 1.5f);
        } else {
            camera.transform.DOMove(batteFieldPosition, 1.5f);
            camera.transform.DORotate(cameraBattlefield.rotation.eulerAngles, 1.5f);
        }

    }

    private void ChangeUiElements(GameObject[] thingsToChange, bool isChanged)
    {
        if (numberOfTurns % 2 == 0)
        {
            for (int i = 0; i < thingsToChange.Length; i++)
            {
                thingsToChange[i].SetActive(isChanged);
            }
        }
        else
        {
            for (int i = 0; i < thingsToChange.Length; i++)
            {
                thingsToChange[i].SetActive(!isChanged);
            }
        }
    }
    


}
