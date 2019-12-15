using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {
    public float gridResolution;
    public float halfGridRes => gridResolution / 2;

    public Grid(float res) {
        gridResolution = res;
    }
    
    public  Vector3 SnapToGrid(Vector3 position) {
        var result = position;
        result.x = Mathf.Floor(result.x / gridResolution) * gridResolution;
        result.z = Mathf.Floor(result.z / gridResolution) * gridResolution;

        return result;
    }
}