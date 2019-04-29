using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTest : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private readonly List<Vector2> _velocityBuffer = new List<Vector2>();

    private Vector2 _lastDragDelta;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _velocityBuffer.Clear();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(nameof(OnEndDrag)+$" Velocity:{(_velocityBuffer.Aggregate(Vector2.zero,(sum, current) => sum+current)/_velocityBuffer.Count)}");
    }

    public void OnDrag(PointerEventData eventData)
    {
        _lastDragDelta = eventData.delta;
        
        if(_velocityBuffer.Count>10)
            _velocityBuffer.RemoveAt(0);
        var rectTransform = eventData.pointerDrag.transform;
        rectTransform.position +=(Vector3)eventData.delta;
        Debug.Log($"{nameof(OnDrag)} Dragging:{eventData.dragging} delta:{eventData.delta} DragGO:{eventData.pointerDrag}");
    }

    public void Log(string text)
    {
        Debug.Log(text);
    }

    private void Update()
    {
        _velocityBuffer.Add(_lastDragDelta / Time.deltaTime);
        _lastDragDelta = Vector2.zero;
    }
}