using System.Collections.Generic;
using UnityEngine;

public class PositionGridView : MonoBehaviour
{
    // public Transform[,] _positionGrid;
    public GameState gameState;
    public PositionRowView rowPfb;
    public List<PositionRowView> positionGrid;
    public float defaultVerticalSpace;
    public float defaultHorizontalSpace;
    public int deufaltRowSize;
    public int defaultColumnSize;
    public GridViewType boardType;

    private GameState GameState
    {
        get
        {
            if (GameState.Instance == null) return gameState;
            return GameState.Instance;
        }
    }

    private float VerticalSpace
    {
        get
        {
            if (GameState == null) return defaultVerticalSpace;
            return GameState.GetBoardVerticalSpace(boardType);
        }
    }

    private float HorizontalSpace
    {
        get
        {
            if (GameState == null) return defaultHorizontalSpace;
            return GameState.GetBoardHorizontalSpace(boardType);
        }
    }

    private int Row
    {
        get
        {
            if (GameState == null) return deufaltRowSize;
            return GameState.GetBoardRow(boardType);
        }
    }

    private int Column
    {
        get
        {
            if (GameState == null) return defaultColumnSize;
            return GameState.GetBoardColumn(boardType);
        }
    }

    [ContextMenu("Create Grid Row")]
    public void CreateGridRow()
    {
        positionGrid = new List<PositionRowView>();
        for (int i = 0; i < Row; i++)
        {
            var row = Instantiate(rowPfb, transform, true);
            row.horizontalSpace = HorizontalSpace;
            row.columnSize = Column;
            positionGrid.Add(row);
            row.CreateGridColumn();

            row.transform.localPosition = Vector3.down * i * VerticalSpace;
            row.name = string.Format($"R[{i}]");
        }
    }

    [ContextMenu("CLEAR ALL")]
    public void ClearAll()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }

        positionGrid.Clear();
    }

#if UNITY_EDITOR
    // private void Reset()
    // {
    //     CreateGridRow();
    // }
    //
    // private void OnValidate()
    // {
    //     Debug.Log("(DEBUG BOARD) Position Grid View --- Validate function...");
    //
    //     CreateGridRow();
    // }
#endif
}