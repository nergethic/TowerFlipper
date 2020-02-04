using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Worker : MonoBehaviour
{
    private Transform goldNodeTransform;
    private Transform storageTransform;
    private float time;
    private bool IsAtDestination;
    private Transform currentDestination;
    
    

    void Start()
    {
        currentDestination = ResourceNode.GetResourceNode().transform;
    }

    public void MoveTo(Transform destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination.position, Time.deltaTime);
        Vector3 direction = destination.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    public void MakeMeInvisible()
    {
        Vector3 invisible = new Vector3(0,0,0);
        transform.localScale = invisible;
    }

    public void MakeMeVisible()
    {
        Vector3 visible = new Vector3(1,1,1);
        transform.localScale = visible;
    }

    private Vector3 GetPosition() => transform.position;

    void Update()
    {
        IsAtDestination = currentDestination.position != transform.position;
        
        if (IsAtDestination)
            MoveTo(currentDestination);
        else
        {
            MakeMeInvisible();
            time += Time.deltaTime;
            if (time > 1)
            {
                MakeMeVisible();
                if (currentDestination == storageTransform)
                    currentDestination = ResourceNode.GetResourceNode().transform;
                else
                {
                    currentDestination = BuildingStorage.GetClosest(GetPosition()).transform;
                    storageTransform = currentDestination;
                }
                    
                time = 0;
            }
        }
    }      
}
