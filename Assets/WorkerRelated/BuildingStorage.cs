using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingStorage : MonoBehaviour
{

    private static List<BuildingStorage> instanceList;
    [SerializeField] private Worker worker;
    

    public static BuildingStorage GetClosest(Vector3 position)
    {
        BuildingStorage closest = null;
        for (int i = 0; i < instanceList.Count; i++)
        {
            if (closest == null)
                closest = instanceList[i];
            else
            {
                if (Vector3.Distance(position, instanceList[i].GetPosition()) < Vector3.Distance(position, closest.GetPosition()))
                    closest = instanceList[i];
            }
        }
        return closest;
    }
    
    public void Register()
    {
        if (instanceList == null) instanceList = new List<BuildingStorage>();
        instanceList.Add(this);
        worker = Instantiate(worker);
        worker.transform.position = this.transform.position;
    }
    public Vector3 GetPosition() => transform.position;
}
