using System;
using UnityEngine;
using UnityEngine.Serialization;


public class PositionRowView : MonoBehaviour
{
    public PositionRow row;
    public GameObject columnPfb;

    public float horizontalSpace;
    public int columnSize;

    public Transform[] Positions => row.positions;

#if UNITY_EDITOR
    // private void Reset()
    // {
    //     Debug.Log("(DEBUG BOARD) Reset function...");
    //     var debugPositions = GetComponentsInChildren<DebugPosition>();
    //     ClearAll();
    //     row.positions = new Transform[columnSize];
    //
    //     for (int i = 0; i < columnSize; i++)
    //     {
    //         var column = Instantiate(columnPfb, transform, true);
    //
    //         // row.positions[i] = debugPositions[i].transform;
    //         // row.positions[i].localPosition = Vector3.zero + Vector3.right * i * horizontalSpace;
    //         column.transform.localPosition = Vector3.zero + Vector3.right * i * horizontalSpace;
    //         column.name = string.Format($"[{i}]");
    //         row.positions[i] = column.transform;
    //     }
    // }
    //
    // private void OnValidate()
    // {
    //     Debug.Log("(DEBUG BOARD) Validate function...");
    //
    //     CreateGridColumn();
    // }

#endif

    [ContextMenu("Create Grid Column")]
    public void CreateGridColumn()
    {
        // var debugPositions = GetComponentsInChildren<DebugPosition>();
        ClearAll();
        row.positions = new Transform[columnSize];

        for (int i = 0; i < columnSize; i++)
        {
            var column = Instantiate(columnPfb, transform, true);

            // row.positions[i] = debugPositions[i].transform;
            // row.positions[i].localPosition = Vector3.zero + Vector3.right * i * horizontalSpace;
            column.transform.localPosition = Vector3.zero + Vector3.right * i * horizontalSpace;
            column.name = string.Format($"C[{i}]");
            row.positions[i] = column.transform;
        }
    }

    private void ClearAll()
    {
        foreach (var column in row.positions)
        {
            DestroyImmediate(column.gameObject);
        }
    }
}

[Serializable]
public class PositionRow
{
    public Transform[] positions;
}