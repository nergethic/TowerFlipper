using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] Camera camera;
    public GameObject cube1;
    public GameObject cube2;
    public int rows;
    public int columns;
    
    void Start() {
        GameObject cube;
        for (var y = 0; y < columns; ++y) {
            for (var x = 0; x < rows; ++x) {
                if (((y + 1) % 2) + (x % 2) == 0)
                {
                    cube = cube1;
                } else
                    cube = cube2;
                
                var instance = Instantiate(cube);
                Vector3 pos = new Vector3(x, 100.0f*Mathf.PerlinNoise(x, 100.0f*y), y);
                instance.transform.position = pos;
            }
        }
    }
}
