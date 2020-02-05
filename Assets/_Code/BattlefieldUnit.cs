using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BattlefieldUnit : GameEntity, ICanBePlacedOnBattlefield {
    public float turnSpeed = 3.0f;
    public Renderer renderer;
    public MovementDirection movementDirection = MovementDirection.Forward;
    public int visionLevel;
    public bool isEnemy = false;
    public bool isDead = false;
    public int life = 3;
    public int GridIndex { get; set; }
    public BattlefieldGrid battlefield;
    protected TurnManager timeManager;

    public virtual void Initialise(BattlefieldGrid battlefield, TurnManager timeManager) {
        this.battlefield = battlefield;
        this.timeManager = timeManager;
        
        battlefield.AddEntity(this);
        timeManager.Add(this);

        if (movementDirection == MovementDirection.Forward) {
            thisTransform.LookAt(thisTransform.position + Vector3.right);
        } else if (movementDirection == MovementDirection.Backward) {
            thisTransform.LookAt(thisTransform.position + Vector3.left);
        }
    }
    
    public virtual void OnDeathTick() {
        isDead = true;
    }

    public virtual void ExecuteAction(PlannedAction otherUnit) {
    }

    public virtual PlannedAction PlanActionOn(BattlefieldGrid.GridCell cell) { // TODO
        return new PlannedAction();
    }

    public int GetForwardCellIndex() {
        var pos = thisTransform.position;
        pos += thisTransform.forward * 2f;
        var cellInfo = battlefield.GetCellAt(ref pos);
        return cellInfo.gridCell.index;
    }

    public (bool, BattlefieldGrid.GridCell) PerformFrontalScan(bool ignoreAllies) {
        var pos = thisTransform.position;

        for (var i = 0; i < visionLevel; i++) {
            pos += thisTransform.forward * 2f;
            
            var cellInfo = battlefield.GetCellAt(ref pos);
            if (cellInfo.success == false)
                continue;

            var gridCell = cellInfo.gridCell;
            if (gridCell.IsEmpty())
                continue;

            Assert.IsTrue(gridCell.entity is BattlefieldUnit);
            var entity = gridCell.entity as BattlefieldUnit;
            if (ignoreAllies && (isEnemy && entity.isEnemy) || (!isEnemy && !entity.isEnemy))
                continue;
            
            return (true, gridCell);
        }
        
        BattlefieldGrid.GridCell emptyCell = new BattlefieldGrid.GridCell();
        return (false, emptyCell);
    }
    
    public (bool, BattlefieldGrid.GridCell) PerformSidesScan(bool ignoreAllies) {
        var pos = thisTransform.position;
        var (x, y) = battlefield.GetPositionFromIndex(GridIndex);

        for (int yy = y - visionLevel; yy <= y + visionLevel; yy++) { // TODO test this
            for (int xx = x - visionLevel; xx <= x + visionLevel; xx++) {
                if (xx == x && yy == y)
                    continue;
                
                var (index, indexIsValid) = battlefield.GetIndex(xx, yy);
                if (!indexIsValid)
                    continue;

                var (gridCell, ok) = battlefield.GetCellAt(index);
                if (!ok)
                    continue;
                
                if (gridCell.IsEmpty())
                    continue;
                
                Assert.IsTrue(gridCell.entity is BattlefieldUnit);
                var entity = gridCell.entity as BattlefieldUnit;
                if (ignoreAllies && (isEnemy && entity.isEnemy) || (!isEnemy && !entity.isEnemy))
                    continue;

                return (true, gridCell);
            }
        }

        BattlefieldGrid.GridCell emptyCell = new BattlefieldGrid.GridCell();
        return (false, emptyCell);
    } 

    public void EndLife() {
        timeManager.Remove(this);
        battlefield.RemoveEntity(this);
        Destroy(gameObject, 1.5f);
    }

    public enum MovementDirection {
        Forward = 0,
        Backward
    }
}
