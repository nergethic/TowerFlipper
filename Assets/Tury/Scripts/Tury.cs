using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tury : MonoBehaviour
{
    float timeOfTurn = 0;
    public int numberOfTurns = 1;
    public Text turnsText;
    public Action OnTurnEnd;
    [SerializeField]
    private GroundPlacementController placement;
    [SerializeField]
    private Resources resources;

    [SerializeField] private Buildings buildings;

    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, -timeOfTurn*6);
        timeOfTurn += Time.deltaTime;
        turnsText.text = "turn: " + numberOfTurns;
        if (timeOfTurn > 2.0f)
        {
            foreach (var building in buildings.Where(b => b.NumberOfBuildings > 0)) {
                resources.AddResources(building.TotalIncome);
            }
            switchMode();
            numberOfTurns++;
            timeOfTurn = 0;
            placement.DestroySelectedObject();
        }
    }
    
    void switchMode()
    {

    }
}
