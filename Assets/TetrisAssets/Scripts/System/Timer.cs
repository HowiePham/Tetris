using System;
using UnityEngine;

[Serializable]
public class Timer
{
    [SerializeField] private bool _shouldPause;

    [SerializeField] private bool _shouldShowLog;
    [SerializeField] private float _interval;
    private float _lastTime;

    public bool HasPastInterval()
    {
        if (Time.time > _interval + _lastTime)
        {
            _lastTime += _interval;
            Log("pass interval");
            return true;
        }

        return false;
    }

    public void SetInterval(float value)
    {
        _interval = value;
    }

    public float GetInterval()
    {
        return _interval;
    }

    private void Log(string message)
    {
        if (_shouldPause) Debug.LogError(message);
        if (_shouldShowLog) Debug.Log(message);
    }
}