using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ShapeRotatingHandler
{
    private List<MatrixIndex> _occupiedPoints;
    public int Column { get; set; }
    public int Row { get; set; }
    public int[,] Matrix { get; set; }
    private Dictionary<ShapeSide, int[,]> _shapeSideDict;
    private Shape _shape;

    public ShapeRotatingHandler(int row, int column, int[,] matrix, Shape shape)
    {
        _shape = shape;
        Row = row;
        Column = column;
        Matrix = matrix;
        // CurrentSide = shapeSide;
        InitializeShapeSide();
    }

    public ShapeSide CurrentSide { get; set; }
    private GameBoard GameBoard => GameBoard.Instance;
    private BlockMatrix BlockMatrix => GameBoard.BlockMatrix;
    private List<BlockController> BlockList => _shape.BlockList;

    private void InitializeShapeSide()
    {
        _shapeSideDict = new Dictionary<ShapeSide, int[,]>();

        int maxSidePerShape = (int)ShapeSide.MAX_COUNT;
        for (int i = 0; i < maxSidePerShape; i++)
        {
            var sideType = (ShapeSide)i;
            _shapeSideDict.Add(sideType, Matrix);
            RotateMatrix();
        }
    }

    private void RotateMatrix()
    {
        int[,] newMartix = new int[Column, Row];

        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Column; j++)
            {
                newMartix[j, Row - 1 - i] = Matrix[i, j];
            }
        }

        Matrix = newMartix;
        Row = Matrix.GetLength(0);
        Column = Matrix.GetLength(1);
    }

    public void RotateShape()
    {
        var nextSide = GetNextSide();
        var shapePosition = _shape.ShapePosition;

        // Debug.Log($"(OCCUPY) NEXT SHAPE MATRIX --- [{shapePosition.Row},{shapePosition.Column}]");
        // LogMatrix(GetShapeMatrix(nextSide));

        if (!CanOccupyByThisRotate(shapePosition.Row, shapePosition.Column, nextSide))
        {
            Debug.Log($"(OCCUPY) Can not rotate...");
            return;
        }

        Debug.Log($"(OCCUPY rotating...");

        RotateMatrixBySide(nextSide);

        _shape.SetShapeState();
        _shape.CreateVirtualShape(shapePosition.Row, shapePosition.Column, GameBoard.OnBoardVirtualPositionGrid);
    }

    private ShapeSide GetNextSide()
    {
        int sideIndex = (int)CurrentSide;
        int maxSidePerShape = (int)ShapeSide.MAX_COUNT;
        ShapeSide nextSide;

        if (sideIndex >= maxSidePerShape - 1)
        {
            nextSide = ShapeSide.SIDE_1;
        }
        else
        {
            nextSide = (ShapeSide)(sideIndex + 1);
        }

        return nextSide;
    }

    private void RotateMatrixBySide(ShapeSide shapeSide)
    {
        CurrentSide = shapeSide;
        Matrix = GetShapeMatrix(CurrentSide);
        Row = Matrix.GetLength(0);
        Column = Matrix.GetLength(1);
    }

    public int[,] GetShapeMatrix(ShapeSide shapeSide)
    {
        return _shapeSideDict[shapeSide];
    }

    public bool CanOccupy(int column)
    {
        int canOccupyCount = 0;

        foreach (var shapeSide in _shapeSideDict)
        {
            if (!CanOccupyByThisRotate(column, shapeSide.Key)) continue;

            canOccupyCount++;
        }

        if (canOccupyCount != 0)
        {
            return true;
        }

        return false;
    }

    public bool CanOccupyByThisRotate(int column, ShapeSide sideType)
    {
        _occupiedPoints = new List<MatrixIndex>();

        var maxRow = GameBoard.LogicalMatrix.GetLength(0);
        var maxcolum = GameBoard.LogicalMatrix.GetLength(1);
        int emptyValue = 0;

        int occupiedCount = 0;

        var shape = _shapeSideDict[sideType];
        var shapeWidth = shape.GetLength(1);
        var shapeLength = shape.GetLength(0);

        // LogMatrix(shape);

        var startRow = maxRow - shapeLength;
        var startColumn = column - shapeWidth / 2;
        var endColumn = startColumn + shapeWidth;

        Debug.Log($"(OCCUPIY) Check from Row: {startRow} - {maxRow - 1} --- Column: {startColumn} - {endColumn - 1}");

        if (endColumn > maxcolum || startColumn < 0) return false;

        int shapeRow = 0;
        for (int i = startRow; i < maxRow; i++)
        {
            int shapeColumn = 0;
            for (int j = startColumn; j < endColumn; j++)
            {
                if (GameBoard.LogicalMatrix[i, j] == emptyValue || shape[shapeRow, shapeColumn] == emptyValue)
                {
                    shapeColumn++;
                    continue;
                }

                // Debug.Log($"(OCCUPIY) Occupied by [{i},{j}] --- {GameBoard.LogicalMatrix[i, j]}");
                occupiedCount++;
                _occupiedPoints.Add(new MatrixIndex(i, j));
                break;
            }

            shapeRow++;

            if (occupiedCount != 0)
            {
                return false;
            }
        }

        return true;
    }

    public bool CanOccupyByThisRotate(int row, int column, ShapeSide sideType)
    {
        _occupiedPoints = new List<MatrixIndex>();

        var maxRow = GameBoard.LogicalMatrix.GetLength(0);
        var maxcolum = GameBoard.LogicalMatrix.GetLength(1);
        int emptyValue = 0;

        int occupiedCount = 0;

        var shape = _shapeSideDict[sideType];
        var shapeWidth = shape.GetLength(1);
        var shapeLength = shape.GetLength(0);

        // LogMatrix(shape);

        var startRow = row;
        var endRow = row + shapeLength;
        var startColumn = column;
        var endColumn = startColumn + shapeWidth;

        // Debug.Log($"(OCCUPY) Check from Row: {startRow} - {endRow - 1} --- Column: {startColumn} - {endColumn - 1}");

        if (startRow < 0 || endRow > maxRow || endColumn > maxcolum || startColumn < 0) return false;

        int shapeRow = 0;
        for (int i = startRow; i < endRow; i++)
        {
            int shapeColumn = 0;
            for (int j = startColumn; j < endColumn; j++)
            {
                // Debug.Log($"(OCCUPY) check at [{i},{j}] --- [{shapeRow},{shapeColumn}]/[{shapeLength},{shapeWidth}]");

                if (shapeRow < shapeLength)
                {
                    if (shape[shapeRow, shapeColumn] == emptyValue)
                    {
                        shapeColumn++;
                        continue;
                    }
                }

                if (GameBoard.LogicalMatrix[i, j] == emptyValue)
                {
                    shapeColumn++;
                    continue;
                }


                var currentBlockCheck = BlockMatrix.At(new MatrixIndex(i, j));
                if (BlockList.Contains(currentBlockCheck))
                {
                    shapeColumn++;
                    continue;
                }

                // Debug.Log($"(OCCUPY) Occupied by [{i},{j}] --- {GameBoard.LogicalMatrix[i, j]}");
                occupiedCount++;
                _occupiedPoints.Add(new MatrixIndex(i, j));
                break;
            }

            shapeRow++;

            if (occupiedCount != 0)
            {
                return false;
            }
        }

        return true;
    }

    public void LogMatrix(int[,] matrix)
    {
        var row = new StringBuilder();
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            row.Append("| ");
            for (var j = 0; j < matrix.GetLength(1); j++)
                row.Append($"{matrix[i, j]}, ");
            row.Remove(row.Length - 2, 2);
            row.Append(" |");
            row.AppendLine();
        }

        Debug.Log($"Current checking: \n {row.ToString()}");
    }
}