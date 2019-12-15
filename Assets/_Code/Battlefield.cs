using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Battlefield : MonoBehaviour {
    [SerializeField] Transform offset;
    public BoxCollider playerSpawnArea;
    public BoxCollider enemySpawnArea;
    public const int GRID_CELLS_COUNT = 200;
    GridCell[] entityGrid = new GridCell[GRID_CELLS_COUNT];
    Vector3Int offsetPosition;

    private void Awake() {
        var pos = offset.position;
        offsetPosition.x = (int)Mathf.Floor(pos.x);
        offsetPosition.y = (int)Mathf.Floor(pos.y);
        offsetPosition.z = (int)Mathf.Floor(pos.z);
    }

    public bool AddEntity(GameEntity entity, bool overrideExisting = false) {
        Assert.IsTrue(entity != null);
        if (overrideExisting && GetEntityAt(entity.thisTransform.position).type == EntityType.None)
            return false;

        entityGrid[GetIndex(entity.thisTransform.position)] = new GridCell {
            type = entity.type,
            entity = entity
        };

        return true;
    }

    public GridCell GetEntityAt(Vector3 snappedPosition) {
        return entityGrid[GetIndex(snappedPosition)];
    }
    
    public GridCell GetEntityAt(int index) {
        Assert.IsTrue(index > 0 && index < GRID_CELLS_COUNT, $"Index = {index} is outside of array bounds!");
        return entityGrid[index];
    }
    
    public int GetIndex(Vector3 snappedPosition) {
        var pos = (snappedPosition - offsetPosition) / 2;
        pos.z *= -1f;
        int index = (int)(Mathf.Floor(pos.z)*40 + Mathf.Floor(pos.x)) + 1;
        // Debug.LogError($"originalPos: ({snappedPosition.x}, {snappedPosition.z} offset: ({offsetPosition.x}, {offsetPosition.z}))");
        // Debug.LogError($"final: ({pos.x}, {pos.z}) -> index: {index}");
        Assert.IsTrue(index > 0 && index < GRID_CELLS_COUNT, $"Index = {index} is outside of array bounds!");
        return index;
    }

    public struct GridCell {
        public EntityType type;
        public GameEntity entity;
    }
}
