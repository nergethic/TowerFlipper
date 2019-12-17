using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {
    [SerializeField] List<BattlefieldUnit> units = new List<BattlefieldUnit>();
    List<BattlefieldUnit> scheduledUnits = new List<BattlefieldUnit>();
    List<int> waitTime = new List<int>();
    float time = 0f;

    private void Update() {
        time += Time.deltaTime;
        if (time >= 1f) {
            time -= 1f;
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
            var unit = units[i];
            waitTime[i] += 1;
            if (waitTime[i] >= unit.turnSpeed) {
                waitTime[i] = 0;
                if (unit.isDead) {
                    unit.battlefield.RemoveEntity(unit);
                    Remove(unit);
                    Destroy(unit, 1f);
                } else
                    unit.Tick();
            }
        }   
    }

    void AddScheduledUnits() {
        for (int i = 0; i < scheduledUnits.Count; i++) {
            var unitToAdd = scheduledUnits[i];
            units.Add(unitToAdd);
            waitTime.Add(0);
            
            scheduledUnits.RemoveAt(i);
        }
    }
}
