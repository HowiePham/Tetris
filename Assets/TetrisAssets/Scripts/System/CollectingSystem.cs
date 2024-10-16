using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectingSystem : TemporaryMonoSingleton<CollectingSystem>
{
    public UnityEvent OnBlockLineCollecting;
    public UnityEvent<int> OnBlockLineCollected;
    public float explodedTime;

    private GameBoard GameBoard => GameBoard.Instance;
    private ShapeSpawner ShapeSpawner => ShapeSpawner.Instance;
    private Timer _timer;

    private Timer Timer
    {
        get
        {
            if (_timer == null)
                _timer = new Timer();
            return _timer;
        }
    }

    private bool HasPastInterval => Timer.HasPastInterval();
    private BlockMatrix BlockMatrix => GameBoard.BlockMatrix;

    private void Update()
    {
        if (!HasPastInterval) return;

        StartCoroutine(CheckLineCollecting());
    }

    IEnumerator CheckLineCollecting()
    {
        if (!CanCheck()) yield break;

        for (int i = GameBoard.Row - 1; i >= 0; i--)
        {
            if (!CanCollectLine(i, out var blocksInRow)) continue;

            CollectBlockLine(blocksInRow);
            Debug.Log($"(COLLECTING) Collecting at row {i}");

            OnBlockLineCollecting?.Invoke();
            yield return new WaitForSeconds(explodedTime);
            OnBlockLineCollected?.Invoke(i);
        }
    }

    private bool CanCollectLine(int rowNumber, out List<BlockController> blocksInRow)
    {
        blocksInRow = BlockMatrix.AtRow(rowNumber);
        var numberOfBlocks = blocksInRow.Count;

        if (numberOfBlocks < GameBoard.Column) return false;
        return true;
    }

    private bool CanCheck()
    {
        var shapeList = ShapeSpawner.OnMovingShapeList;

        foreach (var shape in shapeList)
        {
            if (shape.IsMoving()) return false;
        }

        return true;
    }

    private static void CollectBlockLine(List<BlockController> blocksInRow)
    {
        foreach (var block in blocksInRow)
        {
            block.DestroyBlock();
        }
    }
}