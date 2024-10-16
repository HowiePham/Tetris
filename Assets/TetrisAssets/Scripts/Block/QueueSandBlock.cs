using System.Collections.Generic;
using UnityEngine;

public class QueueSandBlock : MonoBehaviour
{
    [SerializeField] private bool _isInQueueBoard;
    [SerializeField] private bool _isEmpty;

    [SerializeField] private Shape _shape;
    [SerializeField] private Vector3 _initialPosition;
    [SerializeField] private QueueBlockSpawner _queueBlockSpawner;

    private int _rotateCount;
    private bool _canRotate;

    private void Awake()
    {
        LoadComponents();
    }

    private void LoadComponents()
    {
        _queueBlockSpawner = GetComponentInChildren<QueueBlockSpawner>();
    }

    public void Init(Vector3 position, int objectValue)
    {
        _initialPosition = position;
        SetObjectValue(objectValue);
    }

    public void DestroyQueueBlock()
    {
        _shape.DestroyAllBlock();
    }


    public void HideQueueBlock(bool set)
    {
        _queueBlockSpawner.ActiveQueueBlock(!set);
    }

    private void SpawnQueueSandBlock()
    {
        _queueBlockSpawner.CreateQueueVirtualShape();
        _isEmpty = false;
    }

    public void SetShape(Shape shape)
    {
        _shape = shape;
        Debug.Log($"Creating Queue Sand: {_shape}");
        SpawnQueueSandBlock();
    }

    public Shape GetShape()
    {
        return _shape;
    }

    public bool IsInQueueBoard()
    {
        return _isInQueueBoard;
    }

    public bool IsEmpty()
    {
        return _isEmpty;
    }

    public void SetObjectValue(int objectValue)
    {
        _queueBlockSpawner.SetObjectValue(objectValue);
    }

    public int GetObjectValue()
    {
        return _queueBlockSpawner.GetObjectValue();
    }

    public bool CanOccupyByThisRotation(int column)
    {
        return _shape.CanOccupyByThisRotation(column, _shape.CurrentSide);
    }

    public bool CanOccupy(int column)
    {
        return _shape.CanOccupy(column);
    }
}