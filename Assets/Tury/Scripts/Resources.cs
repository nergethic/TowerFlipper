using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    [SerializeField]
    private Building1 building1;
    [SerializeField]
    private Building2 building2;
    public static int Gold { get; set; } = 100;
    public static int Wood { get; set; } = 100;
    [SerializeField] private Text resourcesText;
    public static int Stone { get; set; } = 100;
    public  int numberOfBuilding1 = 0;
    

    private void Update() {
        resourcesText.text = $"Gold: {Gold} Wood: {Wood} Stone: {Stone}";
        
    }
    
    public void EndOfTurn()
    {
        Gold += building1.income.NumberOfBuilding * building1.income.GoldIncome + building2.income.NumberOfBuilding * building2.income.GoldIncome;
        Wood += building1.income.NumberOfBuilding * building1.income.WoodIncome + building2.income.NumberOfBuilding * building2.income.WoodIncome;
        Stone += building1.income.NumberOfBuilding * building1.income.StoneIncome + building2.income.NumberOfBuilding * building2.income.StoneIncome;

    }
}


