using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildings : MonoBehaviour

{
    private Resources resources;
    public struct Building
    {
        public  int GoldIncome { get; set; }
        public  int WoodIncome { get; set; }
        public  int StoneIncome { get; set; }
        public  int GoldCost { get; set; }
        public  int WoodCost { get; set; }
        public  int StoneCost { get; set; }
        public int NumberOfBuilding { get; set; }


        public Building(int goldIncome, int woodIncome, int stoneIncome, int goldCost, int woodCost, int stoneCost)
        {
            GoldIncome = goldIncome;
            WoodIncome = woodIncome;
            StoneIncome = stoneIncome;
            GoldCost = goldCost;
            WoodCost = woodCost;
            StoneCost = stoneCost;
            NumberOfBuilding = 0;
        }

        public bool CostCheck()
        {
            if (Resources.Gold >= GoldCost && Resources.Wood >= WoodCost &&
                Resources.Stone >= StoneCost)
            {
                NumberOfBuilding++;
                Resources.Gold -= GoldIncome;
                Resources.Wood -= WoodCost;
                Resources.Stone -= StoneCost;
                return true;
            }
            else return false;
        }
    }
}
