using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class Shape : ICloneable
{
    private const int RENDER_MATRIX_ROW = 6;
    private const int RENDER_MATRIX_COLUMN = 6;
    [SerializeField] private string name;
    [SerializeField] private int _column;
    [SerializeField] private int _row;
    [SerializeField] private ShapeSide currentSide;
    [SerializeField] private List<BlockController> blockList;

    private BlockSpawner _blockSpawner;
    private int _objectValue;
    private int[,,,] _renderMatrix;
    private ShapePhysic _shapePhysic;
    private ShapeRotatingHandler _shapeRotatingHandler;
    [SerializeField] private int[,] matrix;

    public Shape(List<List<string>> shapeData, string name)
    {
        this.name = name;

        Row = shapeData.Count;
        Column = shapeData[0].Count;
        Matrix = new int[Row, Column];

        InitializeRenderMatrix(shapeData);

        // RenderMatrixLog();
        InitializeShapeHandler();
        InitializeSandSpawner();
    }

    public Shape(int[,] matrix, string name, int[,,,] renderMatrix, ShapeSide shapeSide = default)
    {
        this.name = name;

        Row = matrix.GetLength(0);
        Column = matrix.GetLength(1);

        Matrix = new int[Row, Column];
        Matrix = matrix;
        // currentSide = shapeSide;
        _renderMatrix = renderMatrix;

        InitializeShapeHandler();
        InitializeSandSpawner();
    }

    private GameState GameState => GameState.Instance;

    public int Column
    {
        get => _column;
        set => _column = value;
    }

    public int Row
    {
        get => _row;
        set => _row = value;
    }

    public int[,] Matrix
    {
        get => matrix;
        set => matrix = value;
    }

    public ShapeSide CurrentSide
    {
        get => currentSide;
        set => currentSide = _shapeRotatingHandler.CurrentSide;
    }

    public List<BlockController> BlockList => blockList;

    public MatrixIndex ShapePosition => _shapePhysic.ShapePosition;

    public object Clone()
    {
        return new Shape(Matrix, name, _renderMatrix, currentSide);
    }

    private void InitializeShapePhysic(MatrixIndex pos, List<BlockController> bottomBlocks, List<BlockController> leftSideBlocks, List<BlockController> rightSideBlockcs)
    {
        if (_shapePhysic == null) _shapePhysic = new ShapePhysic();

        _shapePhysic.SetShapePosition(pos);
        _shapePhysic.SetAllSideBlockList(BlockList);
        _shapePhysic.SetShapeSideBlocks(bottomBlocks, leftSideBlocks, rightSideBlockcs);
    }

    private void InitializeShapeHandler()
    {
        _shapeRotatingHandler = new ShapeRotatingHandler(Row, Column, Matrix, this);
    }

    private void InitializeSandSpawner()
    {
        _blockSpawner = new BlockSpawner(this);
    }

    private void InitializeRenderMatrix(List<List<string>> shapeData)
    {
        var renderMatrixRow = Mathf.CeilToInt(1f * Row / RENDER_MATRIX_ROW);
        var renderMatrixColumn = Mathf.CeilToInt(1f * Column / RENDER_MATRIX_COLUMN);

        // Debug.Log($"(READER) matrixRow: {Row}/{RENDER_MATRIX_ROW} = {renderMatrixRow} --- matrixColumn: {Column}/{RENDER_MATRIX_COLUMN} = {renderMatrixColumn}");

        _renderMatrix = new int[renderMatrixRow, renderMatrixColumn, RENDER_MATRIX_ROW, RENDER_MATRIX_COLUMN];

        for (var row = 0; row < Row; row++)
        {
            var rowIndex = row / RENDER_MATRIX_ROW;

            for (var column = 0; column < Column; column++)
            {
                var columnIndex = column / RENDER_MATRIX_COLUMN;

                Matrix[row, column] = int.Parse(shapeData[row][column]);
                _renderMatrix[rowIndex, columnIndex, row % RENDER_MATRIX_ROW, column % RENDER_MATRIX_COLUMN] = Matrix[row, column];
            }
        }
    }

    public void SetShapeState()
    {
        Matrix = _shapeRotatingHandler.Matrix;
        Row = _shapeRotatingHandler.Row;
        Column = _shapeRotatingHandler.Column;

        currentSide = _shapeRotatingHandler.CurrentSide;
    }

    public void RotateShape()
    {
        // if (!IsMoving()) return;

        _shapeRotatingHandler.RotateShape();
    }

    public int GetHalfShapeLength()
    {
        return Row - Row / 2;
    }

    public int GetRightSideWidth()
    {
        return Column - Column / 2;
    }

    public int GetLeftSideWidth()
    {
        return Column / 2;
    }

    public MatrixIndex GetMiddlePointOfShape()
    {
        var middleRow = Row / 2;
        var middleColumn = Column / 2;
        return new MatrixIndex(middleRow, middleColumn);
    }

    public int ValueAt(int i, int j)
    {
        return Matrix[i, j];
    }

    public int ValueAt(MatrixIndex matrixIndex)
    {
        return ValueAt(matrixIndex.Row, matrixIndex.Column);
    }

    public override string ToString()
    {
        var row = new StringBuilder();
        for (var i = 0; i < Matrix.GetLength(0); i++)
        {
            row.Append("| ");
            for (var j = 0; j < Matrix.GetLength(1); j++)
                row.Append($"{Matrix[i, j]}, ");
            row.Remove(row.Length - 2, 2);
            row.Append(" |");
            row.AppendLine();
        }

        return name + "\n" + row;
    }

    private void RenderMatrixLog()
    {
        var row = new StringBuilder();
        for (var i = 0; i < _renderMatrix.GetLength(0); i++)
        {
            row.AppendLine();
            for (var j = 0; j < _renderMatrix.GetLength(1); j++)
            {
                row.AppendLine("&&&&&&&&&&&&& ");

                for (var k = 0; k < _renderMatrix.GetLength(2); k++)
                {
                    row.Append("| ");

                    for (var l = 0; l < _renderMatrix.GetLength(3); l++) row.Append($"{_renderMatrix[i, j, k, l]}, ");

                    row.AppendLine(" |");
                }
            }

            // row.Remove(row.Length - 2, 2);
            row.AppendLine(" --------------------------------------------- ");

            Debug.Log($"RENDER MATRIX: {row}");
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="index">
    ///     The Index of general Matrix
    /// </param>
    /// <param name="value"></param>
    /// <returns></returns>
    // public Sprite GetRenderValueAt(MatrixIndex index, int value = -1)
    // {
    //     var sandColorType = 0;
    //     if (value == -1)
    //     {
    //         sandColorType = ValueAt(index) - 1;
    //     }
    //     else
    //     {
    //         sandColorType = value - 1;
    //     }
    //
    //     SandBlock sandBlock = SandBlockList[sandColorType];
    //     List<Sprite> spriteList = sandBlock.spriteList;
    //
    //     var spriteMatrixIndex = GetChildRenderMatrixIndex(index);
    //     int spriteIndex = GetIndex(spriteMatrixIndex);
    //
    //     Sprite spriteResult = spriteList[spriteIndex];
    //     return spriteResult;
    // }
    private int GetIndex(MatrixIndex childRenderMatrixIndex)
    {
        return childRenderMatrixIndex.Row * RENDER_MATRIX_COLUMN + childRenderMatrixIndex.Column;
    }

    /// <summary>
    /// </summary>
    /// <param name="index">
    ///     The Index of general Matrix
    /// </param>
    /// <returns></returns>
    private MatrixIndex GetChildRenderMatrixIndex(MatrixIndex index)
    {
        var spriteMatrixIndex = new MatrixIndex(index.Row % RENDER_MATRIX_ROW, index.Column % RENDER_MATRIX_COLUMN);
        return spriteMatrixIndex;
    }

    /// <summary>
    /// </summary>
    /// <param name="renderMatrixIndex">
    ///     The Index of render Matrix
    /// </param>
    /// <param name="childMatrixIndex">
    ///     The Index of render Matrix's element Matrix
    /// </param>
    /// <returns></returns>
    public int GetRenderValueAt(MatrixIndex renderMatrixIndex, MatrixIndex childMatrixIndex)
    {
        return _renderMatrix[renderMatrixIndex.Row, renderMatrixIndex.Column, childMatrixIndex.Row, childMatrixIndex.Column];
    }

    public bool CanOccupy(int column)
    {
        return _shapeRotatingHandler.CanOccupy(column);
    }

    public bool CanOccupyByThisRotation(int column, ShapeSide sideType)
    {
        return _shapeRotatingHandler.CanOccupyByThisRotate(column, sideType);
    }

    public void CreateVirtualShape(int startRow, int startColumn, Transform[,] virtualPositionGrid, int objectValue = 0, Transform sandRoot = null, string sandPoolName = "", bool isQueueSand = false)
    {
        if (objectValue != 0) _objectValue = objectValue;

        _blockSpawner.SetMatrix(Matrix);
        _blockSpawner.CreateVirtualShape(startRow, startColumn, _objectValue, virtualPositionGrid, sandRoot, sandPoolName, isQueueSand);
        blockList = _blockSpawner.BlockList;

        if (isQueueSand) return;

        InitializeShapePhysic(new MatrixIndex(startRow, startColumn), _blockSpawner.BottomBlocks, _blockSpawner.LeftSideBlocks, _blockSpawner.RightSideBlocks);
    }

    public void ActiveQueueBlock(bool set)
    {
        _blockSpawner.ActiveQueueBlock(set);
    }

    public void DestroyAllBlock()
    {
        _blockSpawner.DestroyAllBlock();
    }

    public void MoveDown()
    {
        _shapePhysic.MoveDown();
    }

    public void MoveLeft()
    {
        _shapePhysic.MoveLeft();
    }

    public void MoveRight()
    {
        _shapePhysic.MoveRight();
    }

    public void DropDownImmediately()
    {
        _shapePhysic.DropDownImmediately();
    }

    public bool IsMoving()
    {
        return _shapePhysic.IsMoving();
    }

    public List<BlockController> GetSandAtBottomShape()
    {
        return _blockSpawner.BottomBlocks;
    }
}