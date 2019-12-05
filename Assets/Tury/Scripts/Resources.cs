using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    [SerializeField]
    private Building2 building2;
    [SerializeField] private Text resourcesText;
    public Dictionary<string, int> playerResources;
    private void Start() {
        playerResources = Resource.AllResources.ToDictionary(r => r.Name, r => r.StartingQuantity);
    }

    private void Update() {
        resourcesText.text = string.Join(",", playerResources.Select(r => $"{r.Key}: {r.Value}"));
    }

    private bool CanSpend(IEnumerable<(Resource resource, int quantity)> resources) =>
        resources.All(r => playerResources[r.resource.Name] >= r.quantity);

    public bool Spend(IEnumerable<(Resource resource, int quantity)> resources) {
        var valueTuples = resources as (Resource resource, int quantity)[] ?? resources.ToArray();
        if (!CanSpend(valueTuples)) return false;
        foreach (var (resource, quantity) in valueTuples) {
            playerResources[resource.Name] -= quantity;
        }
        return true;
    }

    public void AddResources(IEnumerable<(Resource resource, int quantity)> resources) {
        foreach (var (resource, quantity) in resources) {
            playerResources[resource.Name] += quantity;
        }
    }

}



