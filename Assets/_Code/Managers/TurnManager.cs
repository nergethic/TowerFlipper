using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
    public const float TURN_TIME_SECONDS = 2f;
    [SerializeField] List<BattlefieldUnit> units = new List<BattlefieldUnit>();
    List<BattlefieldUnit> scheduledUnits = new List<BattlefieldUnit>();
    List<PlannedMove> plannedMoves = new List<PlannedMove>();
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

    void Tick() {
        AddScheduledUnits();

        for (int i = 0; i < units.Count; i++) {
            waitTime[i] += (int)TURN_TIME_SECONDS;
        }

        // cell is empty: movement planning phase
        // enemy detected: attack phase
        for (int i = 0; i < units.Count; i++) {
            var unit = units[i];
            if (waitTime[i] >= unit.turnSpeed) {
                unit.Tick();
            }
        }
        
        // movement resolve phase
        for (int i = 0; i < plannedMoves.Count; i++) {
            for (int j = 0; j < plannedMoves.Count; j++) {
                if (i == j)
                    continue;

                if (plannedMoves[i].cellIndex == plannedMoves[j].cellIndex) {
                    
                }
            }
        }
        plannedMoves.Clear();

        // death phase
        for (int i = 0; i < units.Count; i++) {
            var unit = units[i];
            if (waitTime[i] >= unit.turnSpeed) {
                if (unit.isDead || unit.life <= 0) {
                    unit.OnDeathTick();
                    unit.EndLife();
                }
                
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

public struct PlannedMove {
    public int cellIndex;
    public BattlefieldUnit unit;
}
