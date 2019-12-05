using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource {
    public static Resource Gold = new Resource("Gold", 100);
    public static Resource Wood = new Resource("Wood", 100);
    public static Resource Stone = new Resource("Stone", 100);
    public static IEnumerable<Resource> AllResources = new[] {Gold, Wood, Stone};
    
    public readonly string Name;
    public readonly int StartingQuantity;

    private Resource(string name, int startingQuantity) {
        Name = name;
        StartingQuantity = startingQuantity;
    }
}
