using System;
using UnityEngine;

[Serializable]
public struct MatrixIndex
{
    [SerializeField] private Vector2Int position;

    public int Row
    {
        get => position.y;
        set => position.y = value;
    }

    public int Column
    {
        get => position.x;
        set => position.x = value;
    }

    public MatrixIndex(int y, int x)
    {
        position = new Vector2Int();
        Column = x;
        Row = y;
    }

    public void SetPosition(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public override string ToString()
    {
        return $"[{Column},{Row}]";
    }
}