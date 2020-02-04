using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    private static List<ResourceNode> resourceNodeList;


    void Awake()
    {
        if (resourceNodeList == null)
            resourceNodeList = new List<ResourceNode>();
        resourceNodeList.Add(this);
    }

    public Vector3 GetPosition() => transform.position;

    public static ResourceNode GetResourceNode() => resourceNodeList[Random.Range(0, resourceNodeList.Count)];
}
