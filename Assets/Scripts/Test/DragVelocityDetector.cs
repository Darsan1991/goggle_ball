using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DragVelocityDetector
{
    private readonly int _bufferCount;

    public Vector2 Velocity => _velocityBufferList.Count == 0 ? Vector2.zero : _velocityBufferList.Aggregate(Vector2.zero, (sum, current) => sum + current) /
                                                                               _velocityBufferList.Count;

    private float _lastDragTime = 0;
    private readonly List<Vector2> _velocityBufferList = new List<Vector2>();


    public DragVelocityDetector(int bufferCount)
    {
        _bufferCount = bufferCount;
    }

    public void OnStartDrag()
    {
        _lastDragTime = 0;
        _velocityBufferList.Clear();
    }

    public void OnStopDrag()
    {
        Debug.Log($"Buffer Count:{_velocityBufferList.Count}");
    }

    public void OnDrag(Vector2 delta)
    {
        var timeInterval = Time.time - _lastDragTime;
        var intervalCount = Mathf.CeilToInt(timeInterval/Time.time);

        _velocityBufferList.AddRange(Enumerable.Range(0,Mathf.Max(intervalCount-1,0)).Select(i => new Vector2()));
        _velocityBufferList.Add(delta/Time.deltaTime);

        if(_velocityBufferList.Count > _bufferCount)
            _velocityBufferList.RemoveAt(0);
        _lastDragTime = Time.time;
    }
}