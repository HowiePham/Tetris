using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

public class GameBoard : TemporaryMonoSingleton<GameBoard>
{
    public PositionGridView onBoardPositionGridView;
    public int Left;
    public int Right;

    public List<LogicalRow> matrix;

    [SerializeField] private bool _isUpdateMatrixFromHierarchy = true;

    [FormerlySerializedAs("_BlockMatrix")] [SerializeField]
    private BlockMatrix blockMatrix;

    [SerializeField] private int[,] _logicalMatrix; // Logical matrix representing different kinds of objects
    private readonly int _emptyValue = 0; // The value representing an empty position in the logical matrix

    private int emptyValue = 0; // The value representing an empty position in the logical matrix

    public int[,] LogicalMatrix
    {
        get => _logicalMatrix;
        private set => _logicalMatrix = value;
    }

    public Transform[,] OnBoardVirtualPositionGrid { get; set; }
    public int Row { get; private set; }
    public int Column { get; private set; }

    public BlockMatrix BlockMatrix
    {
        get => blockMatrix;
        private set => blockMatrix = value;
    }

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Update()
    {
        if (_isUpdateMatrixFromHierarchy)
        {
            _isUpdateMatrixFromHierarchy = false;
            InitLogicalMatrix();
        }
    }

    private void Initialize()
    {
        InitOnBoardVirtualMatrix();
        InitMatrix();
        InitBlockMatrix();
        InitLogicalMatrix();

        if (Right <= 0) Right = Column - 1;
        if (Left < 0)
            Left = 0;
    }

    private void InitMatrix()
    {
        matrix.Clear();
        for (int i = 0; i < Row; i++)
        {
            LogicalRow logicalRow = new LogicalRow();
            logicalRow.points = new int[Column];
            matrix.Add(logicalRow);
        }
    }

    private void InitOnBoardVirtualMatrix()
    {
        var positionGrid = onBoardPositionGridView.positionGrid;
        Row = positionGrid.Count;
        Column = positionGrid[0].Positions.Length;

        OnBoardVirtualPositionGrid = new Transform[Row, Column];

        for (var row = 0; row < Row; row++)
        for (var column = 0; column < Column; column++)
            OnBoardVirtualPositionGrid[row, column] = positionGrid[row].Positions[column];
    }

    private void InitBlockMatrix()
    {
        BlockMatrix = new BlockMatrix(Row, Column);
    }

    private void InitLogicalMatrix()
    {
        // Initialize the logical matrix with empty positions
        LogicalMatrix = new int[Row, Column];
        for (var row = 0; row < Row; row++)
        for (var column = 0; column < Column; column++)
            LogicalMatrix[row, column] = matrix[row].points[column];

        // LogLogicalMatrix();
    }

    public void AddBlockToMatrix(BlockController block)
    {
        BlockMatrix.AddBlock(block);
    }

    public void UpdateBlockMatrix(BlockController sand, MatrixIndex exPosition)
    {
        BlockMatrix.UpdateBlock(sand, exPosition);
    }

    public void UpdateLogicalMatrix(MatrixIndex exPosition, MatrixIndex currentPosition, int value)
    {
        LogicalMatrix[exPosition.Row, exPosition.Column] = _emptyValue;
        LogicalMatrix[currentPosition.Row, currentPosition.Column] = value;
    }

    public BlockController At(MatrixIndex position)
    {
        return BlockMatrix.At(position);
    }

    public List<BlockController> AtRow(int row)
    {
        return BlockMatrix.AtRow(row);
    }

    public List<BlockController> AtColumn(int column)
    {
        return BlockMatrix.AtColumn(column);
    }

    private void LogLogicalMatrix()
    {
        var log = new StringBuilder();
        for (var row = 0; row < Row; row++)
        {
            log.Append("(");
            for (var column = 0; column < Column; column++) log.Append($"{LogicalMatrix[row, column]}, ");

            log.Remove(log.Length - 2, 2);
            log.Append(")\n");
        }

        print(log.ToString());
    }

    public int GetOnBoardColumnByPosition(Vector3 position)
    {
        for (int i = 0; i < OnBoardVirtualPositionGrid.GetLength(1); i++)
        {
            float distance = position.x - OnBoardVirtualPositionGrid[0, i].position.x;
            if (distance > 0.05f)
                continue;
            return i;
        }

        return 0;
    }
}