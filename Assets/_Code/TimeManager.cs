using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {
    [SerializeField] List<BattlefieldUnit> units = new List<BattlefieldUnit>();
    List<float> timeSinceLastUpdate = new List<float>();

    private void Update() {
        for (int i = 0; i < units.Count; i++) {
            var unit = units[i];
            timeSinceLastUpdate[i] += Time.deltaTime;

            if (timeSinceLastUpdate[i] >= unit.turnSpeed) {
                timeSinceLastUpdate[i] = 0f;
                unit.Tick();
            }
        }
    }

    public void Add(BattlefieldUnit unit) {
        units.Add(unit);
        timeSinceLastUpdate.Add(0f);
    }

    public void Remove(BattlefieldUnit unit) {
        int index = units.IndexOf(unit);
        units.RemoveAt(index);
        timeSinceLastUpdate.RemoveAt(index);
    }
}
