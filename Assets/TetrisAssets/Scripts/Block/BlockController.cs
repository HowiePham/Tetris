using MarchingBytes;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField] private bool _shouldLog;
    [SerializeField] private bool _isMoving;
    [SerializeField] private bool _isQueueSand;

    [SerializeField] private MatrixIndex _position;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Sprite blockSprite;
    [SerializeField] private BlockType blockType;
    [SerializeField] private Shape _currentShape;

    private int _emptyValue; // The value representing an empty position in the logical matrix

    private int _objectValue = 1; // The value representing the object in the logical matrix
    public Transform[,] QueueVirtualPosititonGrid;

    public int[,] LogicalMatrix => GameBoard.LogicalMatrix;
    private GameBoard GameBoard => GameBoard.Instance;
    private EasyObjectPool EasyObjectPool => EasyObjectPool.instance;
    public Transform[,] OnBoardVirtualPositionGrid => GameBoard.OnBoardVirtualPositionGrid;

    public MatrixIndex Position
    {
        get => _position;
        private set => _position = value;
    }

    public Sprite BlockSprite => blockSprite;

    public void SetRenderer(Sprite sprite)
    {
        blockSprite = sprite;
        SetBlockSprite(BlockSprite);
    }

    public void SetBlockSprite(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }

    public void SetCurrentShape(Shape shape)
    {
        _currentShape = shape;
    }

    public void SetData(int currentRow, int currentColumn, int objectValue, bool isQueueSand, Transform[,] queueGrid)
    {
        _position.SetPosition(currentRow, currentColumn);
        _objectValue = objectValue;
        blockType = (BlockType)objectValue;
        _isQueueSand = isQueueSand;
        QueueVirtualPosititonGrid = queueGrid;

        Init();
    }

    private void Init()
    {
        UpdatePosition();

        if (_isQueueSand) return;
        UpdateBoardMatrix(_position);
    }

    private void LogPosition()
    {
        Log($"position: ({Position.Row} - {Position.Column})");
    }

    private void LogPositionState(int row, int column)
    {
        Log($"position: ({row} - {column}) - {LogicalMatrix[row, column]}");
    }

    public void CheckMoveDown()
    {
        if (IsAtBottomRow()) return;

        if (CanMoveDown())
        {
            _isMoving = true;

            MoveDown();
        }
        else
        {
            _isMoving = false;
        }
    }

    public void CheckMoveLeft()
    {
        if (IsAtLeftBoardSide()) return;

        if (!CanMoveLeft()) return;
        MoveLeft();
    }

    public void CheckMoveRight()
    {
        if (IsAtRightBoardSide()) return;

        if (!CanMoveRight()) return;
        MoveRight();
    }

    public bool IsAtBottomRow()
    {
        if (Position.Row < GameBoard.Row - 1) return false;

        _isMoving = false;
        return true;
    }

    public bool IsAtRightBoardSide()
    {
        return Position.Column >= GameBoard.Column - 1;
    }

    public bool IsAtLeftBoardSide()
    {
        return Position.Column <= 0;
    }

    public void MoveDown()
    {
        var exPosition = new MatrixIndex(Position.Row, Position.Column);

        LogPositionState(Position.Row + 1, Position.Column);
        _position.Row++;
        Log("move down");
        UpdatePosition();
        UpdateBoardMatrix(exPosition);
    }

    private void MoveLeft()
    {
        var exPosition = new MatrixIndex(Position.Row, Position.Column);

        LogPositionState(Position.Row, Position.Column - 1);
        _position.Column--;
        Log("move left");
        UpdatePosition();
        UpdateBoardMatrix(exPosition);
    }

    private void MoveRight()
    {
        var exPosition = new MatrixIndex(Position.Row, Position.Column);

        LogPositionState(Position.Row, Position.Column + 1);
        _position.Column++;
        Log("move right");
        UpdatePosition();
        UpdateBoardMatrix(exPosition);
    }

    public bool CanMoveDown()
    {
        return Position.Row < GameBoard.Row - 1 && LogicalMatrix[Position.Row + 1, Position.Column] == _emptyValue;
    }

    public bool CanMoveLeft()
    {
        return Position.Column > 0 && LogicalMatrix[Position.Row, Position.Column - 1] == _emptyValue;
    }

    public bool CanMoveRight()
    {
        return Position.Column < GameBoard.Column - 1 && LogicalMatrix[Position.Row, Position.Column + 1] == _emptyValue;
    }

    private void Log(string message)
    {
        if (!_shouldLog)
            return;
        // #if DEBUG_LOG
        print(message);
        // #endif
    }

    private void UpdateBoardMatrix(MatrixIndex exPosition)
    {
        GameBoard.UpdateLogicalMatrix(exPosition, Position,
            _objectValue);
        GameBoard.UpdateBlockMatrix(this, exPosition);
    }

    private Vector3 GetPosition(int row, int column)
    {
        if (_isQueueSand) return QueueVirtualPosititonGrid[row, column].transform.position;

        return OnBoardVirtualPositionGrid[row, column].transform.position;
    }

    private void UpdatePosition()
    {
        var newPosition = GetPosition(Position.Row, Position.Column);

        UpdatePosition(newPosition);
    }

    private void UpdatePosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void UpdatePosition(MatrixIndex newPosition)
    {
        Position = newPosition;
        UpdatePosition();
    }

    public void UpdateAllPosition(MatrixIndex newPosition)
    {
        var exPosition = new MatrixIndex(Position.Row, Position.Column);
        Position = newPosition;
        UpdateBoardMatrix(exPosition);
        UpdatePosition();
    }

    public void DestroyBlock()
    {
        if (!_isQueueSand)
        {
            ClearBlock();
            return;
        }

        ReturnObjectToPool();
    }

    public void ClearBlock()
    {
        GameBoard.UpdateLogicalMatrix(Position, Position, 0);
        GameBoard.UpdateBlockMatrix(null, Position);

        ReturnObjectToPool();
    }

    private void ReturnObjectToPool()
    {
        EasyObjectPool.ReturnObjectToPool(gameObject);
    }

    public bool IsMoving()
    {
        return _isMoving;
    }

    public int GetLabel()
    {
        return _objectValue;
    }

    public void ActiveRenderer(bool set)
    {
        _renderer.enabled = set;
    }
}