using System;
using System.Collections.Generic;

[Serializable]
public class BlockMatrix
{
    public int _column;

    public int _row;

    public BlockController[,] Matrix;

    public BlockMatrix(int row, int column)
    {
        _row = row;
        _column = column;
        Matrix = new BlockController[row, column];
        for (var i = 0; i < row; i++)
        for (var j = 0; j < column; j++)
            Matrix[i, j] = null;
    }


    public BlockController At(MatrixIndex position)
    {
        return Matrix[position.Row, position.Column];
    }

    public void Set(MatrixIndex position, BlockController sand)
    {
        Matrix[position.Row, position.Column] = sand;
    }

    public List<BlockController> AtRow(int row)
    {
        var selectedRow = new List<BlockController>();

        for (var i = _column - 1; i >= 0; i--)
            if (Matrix[row, i])
                selectedRow.Add(Matrix[row, i]);
        return selectedRow;
    }

    public List<BlockController> AtColumn(int column)
    {
        var selectedColumn = new List<BlockController>();
        for (var i = 0; i < _row; i++)
            if (Matrix[i, column])
                selectedColumn.Add(Matrix[i, column]);
        return selectedColumn;
    }

    public void AddBlock(BlockController block)
    {
        Matrix[block.Position.Row, block.Position.Column] = block;
    }

    public void UpdateBlock(BlockController sand, MatrixIndex exPosition)
    {
        Matrix[exPosition.Row, exPosition.Column] = null;
        if (sand == null) return;
        Matrix[sand.Position.Row, sand.Position.Column] = sand;
    }
}