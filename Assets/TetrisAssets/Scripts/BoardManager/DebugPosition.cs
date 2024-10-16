#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DebugPosition : MonoBehaviour
{
    [SerializeField] public float DebugSize = 0.2f;
    [SerializeField] private Color _color = Color.blue;

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, DebugSize);
    }
}
#endif