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
    int index = 0;
    
    void Start() {
        towerElements.Add(towerTop.transform);
        InvokeRepeating("ChangeTower", 4.0f, 3.0f);
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

    IEnumerator DestroyFirstFloor() {
        yield return new WaitForSeconds(1.6f);
        int lastIndex = towerElements.Count-1;
        var firstFloor = towerElements[lastIndex];
        towerElements.RemoveAt(lastIndex);
        Destroy(firstFloor.gameObject);
    }
    
    public void DamageTower() {
        for (int i = 0; i < towerElements.Count; i++) {
            var newPos = towerElements[i].position;
            newPos.y -= towerHeight;
            var action = towerElements[i].DOMove(newPos, 1.5f);
        }
        
        StartCoroutine(DestroyFirstFloor());
    }

    void ChangeTower() {
        if (index % 3 == 0) {
            BuildLevel();
        }
        else if (index % 3 == 1) {
            DamageTower();
        } else {
            BuildLevel();
        }
        
        index++;
    }
}
