using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Building : MonoBehaviour {
    public BuildingType type;
    // TODO: implement upgrade level
    public Resources resourcesProduction;

    private void Awake() {
        resourcesProduction = new Resources();
    }
}