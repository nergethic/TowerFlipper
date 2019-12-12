using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] BuildingManager buildingManager;
    
    void Update() {
        transform.eulerAngles = new Vector3(0, 0, -timeOfTurn*6);
        timeOfTurn += Time.deltaTime;
        turnsText.text = "turn: " + numberOfTurns;
        if (timeOfTurn > 5.0f) {
            foreach (var building in buildingManager.GetBuildings()) 
                resourcesManager.AddResources(building.resourcesProduction);
            
            numberOfTurns++;
            timeOfTurn = 0;
            placement.TryToDestroySelectedObject();
        }
    }
}
