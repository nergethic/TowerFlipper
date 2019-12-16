using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class MathUtility {
    public static float Clamp0(float value) {
        return value < 0f ? 0f : value;
    }
    
    public static float Map(float x, float fromMin, float fromMax, float toMin, float toMax) {
        return toMin + (x-fromMin) * (toMax-toMin) / (fromMax-fromMin);
    }
    
    public static float3 Map(float3 x, float fromMin, float fromMax, float toMin, float toMax) {
        return new float3(toMin) + ((x-new float3(fromMin))* new float3((toMax-toMin)/(fromMax-fromMin)));
    }
}
