using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapeSpawner : TemporaryMonoSingleton<ShapeSpawner>
{
    [Header("On Board Shape Block")] [SerializeField]
    private bool _shouldLog;

    [SerializeField] private int _numberOfColorInShape;
    [SerializeField] private Transform _blockRoot;
    [SerializeField] private List<Shape> _onMovingShapeList;
    private Dictionary<BlockController, int> _blockDictionary;

    [Space(20)] [Header("In Queue Shape Block")] [SerializeField]
    private bool _shouldCreateNewShape;

    [SerializeField] private bool _isRandomValue;
    [SerializeField] private int startColumn;

    [SerializeField] private List<QueueSandBlock> _onQueueBlockList = new List<QueueSandBlock>();
    [SerializeField] private List<QueueSandBlock> _queueBlockList = new List<QueueSandBlock>();
    [SerializeField] private QueuePointManager _queuePointManager;

    private GameBoard _gameBoard;
    private float _lastTime;
    private ShapeReader _shapeReader;
    private GameBoard GameBoard => GameBoard.Instance;
    private ShapeReader ShapeReader => ShapeReader.Instance;

    private Dictionary<BlockController, int> BlockDictionary
    {
        get
        {
            if (_blockDictionary == null) _blockDictionary = new Dictionary<BlockController, int>();

            return _blockDictionary;
        }
    }


    public List<Shape> OnMovingShapeList
    {
        get
        {
            if (_onMovingShapeList == null) _onMovingShapeList = new List<Shape>();

            return _onMovingShapeList;
        }
    }

    private void Update()
    {
        CheckCreateNewQueueShape();
        CheckCreateNewOnBoardShape();
    }

    private void CheckCreateNewOnBoardShape()
    {
        if (OnMovingShapeList.Count <= 0)
        {
            var onQueueBlock = _onQueueBlockList[0];

            CreateNewOnBoardSand(startColumn, onQueueBlock, onQueueBlock.GetObjectValue());
        }
    }

    public void TestSpawn()
    {
        var objectValue = Random.Range(1, (int)BlockType.RANDOM);
        CreateNewOnBoardSand(startColumn, _onQueueBlockList[0], objectValue);
    }

    private void CreateQueueBlock()
    {
        Debug.Log($"(SPAWNING) Create On Queue Block at column");

        var pointList = _queuePointManager.GetPointList();
        for (int i = 0; i < pointList.Count; i++)
        {
            var point = pointList[i];

            var objectValue = Random.Range(1, (int)BlockType.RANDOM - 1);

            var pointPosition = point.position;

            var queueBlock = _queueBlockList[i];
            queueBlock.transform.position = pointPosition;
            queueBlock.gameObject.SetActive(true);

            QueueSandBlock queueSandBlock = queueBlock.GetComponent<QueueSandBlock>();
            queueSandBlock.Init(pointPosition, objectValue);
            queueSandBlock.SetShape(ShapeReader.GetShape());

            _onQueueBlockList.Add(queueSandBlock);
        }
    }

    public void CreateOnBoardVirtualShape(Shape shape, int _startColumn, int objectValue)
    {
        int startColumn = _startColumn - shape.GetMiddlePointOfShape().Column;
        int startRow = 0;

        Shape newShape = (Shape)shape.Clone();
        newShape.CreateVirtualShape(startRow, startColumn, GameBoard.OnBoardVirtualPositionGrid, objectValue, _blockRoot, PoolName.BLOCK_POOL);
        OnMovingShapeList.Add(newShape);
    }

    private void AddBlock(BlockController block)
    {
        GameBoard.AddBlockToMatrix(block);
    }

    private void CheckCreateNewQueueShape()
    {
        if (_onQueueBlockList.Count <= 0)
        {
            _shouldCreateNewShape = true;
        }

        if (_shouldCreateNewShape)
        {
            _shouldCreateNewShape = false;

            CreateQueueBlock();
        }
    }

    public void DestroySand(BlockController sand)
    {
        BlockDictionary.Remove(sand);
        sand.DestroyBlock();
    }

    private void Log(string message)
    {
        if (!_shouldLog)
            return;
        print($"{message}");
    }

    public void CreateNewOnBoardSand(int startColumn, QueueSandBlock queueSandBlock, int objectValue)
    {
        Debug.Log($"(SPAWNING) Create On Board Sand at column {startColumn}");

        var shape = queueSandBlock.GetShape();

        CreateOnBoardVirtualShape(shape, startColumn, objectValue);

        queueSandBlock.DestroyQueueBlock();
        _onQueueBlockList.Remove(queueSandBlock);
    }

    public void ClearAllBlock()
    {
        foreach (var value in BlockDictionary)
        {
            var sand = value.Key;
            sand.ClearBlock();
        }

        BlockDictionary.Clear();
    }

    public void RerollQueueBlock()
    {
        for (int i = _onQueueBlockList.Count - 1; i >= 0; i--)
        {
            var block = _onQueueBlockList[i];
            _onQueueBlockList.Remove(block);
        }

        CreateQueueBlock();
    }

    public void RemoveFromShapeList(Shape shape)
    {
        OnMovingShapeList.Remove(shape);
    }

    public void RemoveFromShapeList(int index)
    {
        OnMovingShapeList.RemoveAt(index);
    }

    public List<QueueSandBlock> GetQueueSandBlockList()
    {
        return _onQueueBlockList;
    }
}