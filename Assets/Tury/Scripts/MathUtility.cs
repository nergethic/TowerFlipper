using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtility
{
    public static float Clamp0(float value) {
        return value < 0f ? 0f : value;
    }
}
