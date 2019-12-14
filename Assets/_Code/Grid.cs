using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Grid {
    public const float gridResolution = 2f;
    public const float halfGridRes = gridResolution / 2f;
    
    public static Vector3 SnapToGrid(Vector3 position) {
        var result = position;
        result.x = Mathf.Floor(result.x / gridResolution) * gridResolution;
        result.z = Mathf.Floor(result.z / gridResolution) * gridResolution;

        return result;
    }
}