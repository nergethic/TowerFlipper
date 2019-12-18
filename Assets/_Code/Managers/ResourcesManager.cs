using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour {
    [SerializeField] Text goldText;
    [SerializeField] Text stoneText;
    [SerializeField] Text woodText;
    Resources playerResources;

    private void Awake() {
        InitResources();
    }

    void InitResources() {
        playerResources = new Resources();
        playerResources.SetValue(ResourceType.Gold, 1000f);
        playerResources.SetValue(ResourceType.Stone, 1000f);
        playerResources.SetValue(ResourceType.Wood, 1000f);
    }

    private void Update() {
        goldText.text = $"Gold: {playerResources.GetValue(ResourceType.Gold)}";
        stoneText.text = $"Stone: {playerResources.GetValue(ResourceType.Stone)}";
        woodText.text = $"Wood: {playerResources.GetValue(ResourceType.Wood)}";
    }

    public bool CanSpend(Resources resources) {
        for (var i = 0; i < resources.values.Length; i++)
            if (playerResources.values[i] - resources.values[i] < 0f)
                return false;

        return true;
    }

    public void SpendResources(Resources resources) {
        for (var i = 0; i < resources.values.Length; i++) {
            playerResources.values[i] = playerResources.values[i] - resources.values[i];
            // playerResources.values[i] = MathUtility.Clamp0(newValue); // WANT CLAMPING?
        }
    }

    public void AddResources(Resources resources) {
        for (var i = 0; i < resources.values.Length; i++) 
            playerResources.values[i] += resources.values[i];
    }
}