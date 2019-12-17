using System;
using UnityEngine;
using UnityEngine.Assertions;

public class BattlefieldGrid : MonoBehaviour {
    [SerializeField] Transform offset;
    public BoxCollider playerSpawnArea;
    public Transform enemySpawn;
    public const int GRID_CELLS_COUNT = 200;
    public const float CELL_WORLD_WIDTH = 2f;
    public const int FLOOR_CELLS_STRIDE = 40;
    GridCell[] entityGrid = new GridCell[GRID_CELLS_COUNT];
    Vector3Int offsetPosition;
    GridCell emptyCell;

    private void Awake() {
        emptyCell.type = EntityType.None;
        
        var pos = offset.position;
        offsetPosition.x = (int)Mathf.Floor(pos.x);
        offsetPosition.y = (int)Mathf.Floor(pos.y);
        offsetPosition.z = (int)Mathf.Floor(pos.z);
        
        for (int i = 0; i < GRID_CELLS_COUNT; i++) {
            entityGrid[i] = new GridCell();
            entityGrid[i].index = i;
            entityGrid[i].type = EntityType.None;
        }
    }

    public bool AddEntity(GameEntity entity, bool overrideExisting = false) {
        Assert.IsTrue(entity != null);
        Assert.IsTrue(entity is ICanBePlacedOnBattlefield);

        var e = GetEntityAt(entity.thisTransform.position);
        if (e.success == false)
            return false;
        
        if (overrideExisting == false && e.gridCell.type != EntityType.None) {
            Debug.Log($"{entity.name}: something is placed at the position: {e.gridCell.type}");
            return false;
        }

        var (index, success) = GetIndex(entity.thisTransform.position);
        if (success == false)
            return false;
        
        var placableEntity = (ICanBePlacedOnBattlefield)entity;
        placableEntity.GridIndex = index;

        entityGrid[index].type = entity.type;
        entityGrid[index].entity = entity;

        // Debug.LogError($"newIndex: {placableEntity.GridIndex}");
        
        return true;
    }

    public void RemoveEntity(GameEntity entity) {
        if (entity == null)
            return;

        Assert.IsTrue(entity is ICanBePlacedOnBattlefield);

        var placableEntity = (ICanBePlacedOnBattlefield)entity;
        entityGrid[placableEntity.GridIndex].type = EntityType.None;
        placableEntity.GridIndex = 0;
    }

    public bool UpdateEntityLocation(GameEntity entity, ref Vector3 position, bool overrideExisting = false) {
        Assert.IsTrue(entity != null);
        Assert.IsTrue(entity is ICanBePlacedOnBattlefield);

        var placableEntity = (ICanBePlacedOnBattlefield)entity;
        entityGrid[placableEntity.GridIndex].type = EntityType.None;
        
        var (newIndex, success) = GetIndex(position);
        if (success == false)
            return false;
        
        placableEntity.GridIndex = newIndex;
        entityGrid[newIndex].type = entity.type;
        entityGrid[newIndex].entity = entity;
        
        // Debug.LogError($"newIndex: {placableEntity.GridIndex}");
        return true;
    }
    
    public (GridCell gridCell, bool success) GetEntityAt(Vector3 snappedPosition) {
        var (index, success) = GetIndex(snappedPosition);
        
        if (success) 
            return (entityGrid[index], true);

        return (emptyCell, false);
    }

    public (GridCell gridCell, bool success) GetCellAt(ref Vector3 snappedPosition) {
        var (index, success) = GetIndex(snappedPosition);
        
        if (success) 
            return (entityGrid[index], true);

        return (emptyCell, false);
    }
    
    public GridCell GetEntityAt(int index) {
        Assert.IsTrue(index >= 0 || index < GRID_CELLS_COUNT, $"Index = {index} is outside of array bounds!");
        return entityGrid[index];
    }
    
    public (int index, bool success) GetIndex(Vector3 snappedPosition) {
        var position = (snappedPosition - offsetPosition) / CELL_WORLD_WIDTH;
        position.z *= -1f;
        return GetIndexInternal(ref position);
    }
    
    public (int index, bool success) GetIndex(GameEntity entity) {
        var position = (entity.thisTransform.position - offsetPosition) / CELL_WORLD_WIDTH;
        position.z *= -1f;
        return GetIndexInternal(ref position);
    }

    (int index, bool success) GetIndexInternal(ref Vector3 position) {
        int index = (int)(Mathf.Floor(position.z)*FLOOR_CELLS_STRIDE + Mathf.Floor(position.x));
        
        // Debug.LogError($"originalPos: ({snappedPosition.x}, {snappedPosition.z} offset: ({offsetPosition.x}, {offsetPosition.z}))");
        // Debug.LogError($"final: ({position.x}, {position.z}) -> index: {index}");
        bool isOutsideBounds = index < 0 || index >= GRID_CELLS_COUNT;
        return (index, isOutsideBounds == false);
    }

    public struct GridCell {
        public int index;
        public EntityType type;
        public GameEntity entity;

        public bool IsEmpty() {
            return type == EntityType.None;
        }
    }
}
