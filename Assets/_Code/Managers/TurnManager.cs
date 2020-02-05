using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TurnManager : MonoBehaviour {
    public const float TURN_TIME_SECONDS = 2f;
    [SerializeField] List<BattlefieldUnit> units = new List<BattlefieldUnit>();
    List<BattlefieldUnit> scheduledUnits = new List<BattlefieldUnit>();
    List<PlannedAction> plannedActions = new List<PlannedAction>();
    List<int> waitTime = new List<int>();
    float time = 0f;

    private void Update() {
        time += Time.deltaTime;
        if (time >= TURN_TIME_SECONDS) {
            time -= TURN_TIME_SECONDS;
            Tick();
        }
    }

    public void Add(BattlefieldUnit unit) {
        scheduledUnits.Add(unit);
    }

    public void Remove(BattlefieldUnit unit) {
        if (unit == null)
            return;
        
        int index = units.IndexOf(unit);
        if (index < 0)
            return;
        
        units.RemoveAt(index);
        waitTime.RemoveAt(index);
    }

    bool PlanNextAction(BattlefieldUnit unit, ScanType type) {

        bool foundSomething;
        BattlefieldGrid.GridCell cell;
        
        switch (type) {
            case ScanType.Front:
                (foundSomething, cell) = unit.PerformFrontalScan(false);
                break;
            
            case ScanType.Around:
                (foundSomething, cell) = unit.PerformSidesScan(true);
                break;
            
            default:
                throw new Exception("invalid switch case");
        }

        if (foundSomething) {
            var plannedAction = unit.PlanActionOn(cell);
            plannedActions.Add(plannedAction);
            return true;
        }

        return false;
    }

    void Tick() {
        AddScheduledUnits();

        for (int i = 0; i < units.Count; i++) {
            waitTime[i] += (int)TURN_TIME_SECONDS;
        }

        for (int i = 0; i < units.Count; i++) {
            var unit = units[i];
            if (waitTime[i] >= unit.turnSpeed) {
                bool actionPlanned = PlanNextAction(unit, ScanType.Front);
                if (!actionPlanned) {
                    bool secondActionPlanned = PlanNextAction(unit, ScanType.Around);
                    if (!secondActionPlanned) {
                        var plannedAction = new PlannedAction {
                            thisUnit = unit,
                            type = ActionType.MoveForward,
                            cellIndex = unit.GetForwardCellIndex()
                        };
                        plannedActions.Add(plannedAction);   
                    }
                }
            }
        }
        
        // movement resolve phase
        var plannedActionConflictsIndices = new List<int>();
        for (int i = 0; i < plannedActions.Count; i++) {
            for (int j = 0; j < plannedActions.Count; j++) {
                if (j >= i) // only i > j passes, discard same and redundant pairs: (i: 1; j: 2), (i: 2; j: 1)
                    continue;
                
                var thisAction = plannedActions[i];
                var otherAction = plannedActions[j];
                if (thisAction.cellIndex == otherAction.cellIndex &&
                    thisAction.type == ActionType.MoveForward && otherAction.type == ActionType.MoveForward) {
                    //Assert.IsTrue(thisAction.thisUnit == otherAction.otherUnit);
                    //Assert.IsTrue(thisAction.otherUnit == otherAction.thisUnit);
                    plannedActionConflictsIndices.Add(i);
                    plannedActionConflictsIndices.Add(j);
                    Debug.LogError($"i: {i}, j: {j}");
                    
                    // adjust action from moving to attacking:
                    thisAction.type = ActionType.Attack;
                    thisAction.otherUnit = otherAction.thisUnit;
                    thisAction.cellIndex = otherAction.thisUnit.GridIndex;
                    
                    otherAction.type = ActionType.Attack;
                    otherAction.otherUnit = thisAction.thisUnit;
                    otherAction.cellIndex = thisAction.thisUnit.GridIndex;

                    thisAction.thisUnit.ExecuteAction(thisAction);
                    otherAction.thisUnit.ExecuteAction(otherAction);
                }
            }
        }
        
        plannedActionConflictsIndices.Sort((a, b) => b.CompareTo(a)); // descending order
        
        foreach (int index in plannedActionConflictsIndices)
            plannedActions.RemoveAt(index);

        foreach (var action in plannedActions) 
            action.thisUnit.ExecuteAction(action);

        plannedActions.Clear();

        // death phase and turn timer reset 
        for (int i = 0; i < units.Count; i++) {
            var unit = units[i];
            if (waitTime[i] >= unit.turnSpeed) {
                if (unit.isDead || unit.life <= 0) {
                    unit.OnDeathTick();
                    unit.EndLife();
                } else 
                    waitTime[i] = 0;
            }
        }
    }

    void AddScheduledUnits() {
        for (int i = 0; i < scheduledUnits.Count; i++) {
            var unitToAdd = scheduledUnits[i];
            units.Add(unitToAdd);
            int t = (int)1000;
            waitTime.Add(t);
        }
        
        scheduledUnits.Clear();
    }
}

public struct PlannedAction {
    public ActionType type;
    public int cellIndex;
    public BattlefieldUnit thisUnit;
    public BattlefieldUnit otherUnit;
}

public enum ActionType {
    Attack = 0,
    MoveForward,
    TurnLeft,
    TurnRight,
    DoNothing
}

public enum ScanType {
    Front = 0,
    Around
}
