using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TowerBuilder : MonoBehaviour {
    [SerializeField] float towerHeight;
    [SerializeField] GameObject towerTop; // scene object
    [SerializeField] GameObject towerMiddle; // prefab object
    [SerializeField] Transform towerCenter;
    List<Transform> towerElements = new List<Transform>();
    
    void Start() {
        towerElements.Add(towerTop.transform);
    }

    public void BuildLevel() {
        var obj = Instantiate(towerMiddle, towerCenter);
        towerElements.Add(obj.transform);

        for (int i = 0; i < towerElements.Count; i++) {
            var newPos = towerElements[i].position;
            newPos.y += towerHeight;
            towerElements[i].DOMove(newPos, 1.5f);
        }
    }
}
