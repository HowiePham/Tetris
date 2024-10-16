using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class QueueBlockSpawner : MonoBehaviour
{
    [SerializeField] private int objectValue;
    [SerializeField] private QueueSandBlock _queueSandBlock;
    [SerializeField] private Transform _sandRoot;
    [SerializeField] private PositionGridView _positionGridView;
    private Transform[,] queueVirtualPositionGrid;
    private int _queueRow;
    private int _queueColumn;
    private int _startColumn;

    private void Awake()
    {
        LoadComponents();
        InitQueueVirtualMatrix();
    }

    private void LoadComponents()
    {
        _queueSandBlock = GetComponentInParent<QueueSandBlock>();
    }

    private void InitQueueVirtualMatrix()
    {
        var positionGrid = _positionGridView.positionGrid;
        _queueRow = positionGrid.Count;
        _queueColumn = positionGrid[0].Positions.Length;

        queueVirtualPositionGrid = new Transform[_queueRow, _queueColumn];

        for (var row = 0; row < _queueRow; row++)
        for (var column = 0; column < _queueColumn; column++)
            queueVirtualPositionGrid[row, column] = positionGrid[row].Positions[column];
    }

    public void CreateQueueVirtualShape()
    {
        var shape = _queueSandBlock.GetShape();
        FindStartSpawningPosition(shape, out var startRow, out var startColumn);

        shape.CreateVirtualShape(startRow, startColumn, queueVirtualPositionGrid, objectValue, _sandRoot, PoolName.QUEUE_BLOCK_POOL, true);
    }

    private void FindStartSpawningPosition(Shape shape, out int startRow, out int startColumn)
    {
        var shape_middlePoint = shape.GetMiddlePointOfShape();

        var grid_middlePoint = new MatrixIndex(_queueRow / 2, _queueColumn / 2);

        startRow = grid_middlePoint.Row - shape_middlePoint.Row;
        startColumn = grid_middlePoint.Column - shape_middlePoint.Column;
    }


    public void ActiveQueueBlock(bool set)
    {
        var shape = _queueSandBlock.GetShape();
        shape.ActiveQueueBlock(set);
    }

    public void SetObjectValue(int objectValue)
    {
        this.objectValue = objectValue;
    }

    public int GetObjectValue()
    {
        return objectValue;
    }
}