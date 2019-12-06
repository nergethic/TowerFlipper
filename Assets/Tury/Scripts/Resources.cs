using UnityEngine.Assertions;

public class Resources {
    public readonly float[] values;

    public Resources() {
        values = new float[(int)ResourceType.Count];
    }

    public void SetValue(ResourceType resourceType, float value) {
        int index = (int)resourceType;
        PerformSafetyCheck(index);
        values[index] = value;
    }

    public float GetValue(ResourceType resourceType) {
        int index = (int)resourceType;
        PerformSafetyCheck(index);
        return values[index];
    }

    void PerformSafetyCheck(int index = (int)ResourceType.Count-1) {
        Assert.IsTrue(index >= 0, "index >= 0");
        Assert.IsTrue(index < (int)ResourceType.Count, "index < (int)ResourceType.Count");
        Assert.IsTrue(values != null, "values != null");
        Assert.IsTrue(values.Length == (int)ResourceType.Count, "values.Length == (int)ResourceType.Count");
    }
}

public enum ResourceType: int {
    Gold = 0,
    Wood,
    Stone,
    Count
}