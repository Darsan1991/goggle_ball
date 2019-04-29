using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RectEnvironment))]
public class RectEnvironmentEditor : Editor
{
    private const float MIN_DISTANCE_BETWEEN_START_END = 0.2f;
    private RectEnvironment _rectEnvironment;


    private void OnEnable()
    {
        _rectEnvironment = (RectEnvironment)target;

    }

    private void OnSceneGUI()
    {
        serializedObject.Update();
        Handles.color = Color.blue;
        var startGlobalPosition = _rectEnvironment.transform.TransformPoint(_rectEnvironment.StartPointLocalPosition);
        var style = new GUIStyle(EditorStyles.label) {normal = {textColor = Color.blue}};
        Handles.Label(startGlobalPosition + new Vector3(HandleUtility.GetHandleSize(startGlobalPosition) * 0.2f, HandleUtility.GetHandleSize(startGlobalPosition) * 0.2f, 0),
            "Start Line", style);

        Handles.color = Color.white;
        EditorGUI.BeginChangeCheck();
        startGlobalPosition = Handles.FreeMoveHandle(startGlobalPosition, _rectEnvironment.transform.rotation * Quaternion.AngleAxis(90, Vector3.right)
            ,1.5f
            , Vector3.zero, ArrowHandleCap);

        if (RectEnvironment.VERTICAL_DIRECTION)
        {
            if (EditorGUI.EndChangeCheck() &&
                _rectEnvironment.transform.InverseTransformPoint(startGlobalPosition).y +
                MIN_DISTANCE_BETWEEN_START_END < _rectEnvironment.EndPointLocalPosition.y)
            {
                if (_rectEnvironment.transform.TransformPoint(_rectEnvironment.EndPointLocalPosition).y >
                    startGlobalPosition.y + MIN_DISTANCE_BETWEEN_START_END)
                {
                    Undo.RecordObject(_rectEnvironment, "start pos");
                    var point = _rectEnvironment.transform.InverseTransformPoint(startGlobalPosition);
                    point.x = 0;
                    _rectEnvironment.StartPointLocalPosition = point;
                }
            }

            if (_rectEnvironment.EndPointLocalPosition.y <
                _rectEnvironment.StartPointLocalPosition.y + MIN_DISTANCE_BETWEEN_START_END)
            {
                _rectEnvironment.StartPointLocalPosition = _rectEnvironment.EndPointLocalPosition -
                                                           Vector3.up * MIN_DISTANCE_BETWEEN_START_END;
            }
        }
        else
        {
            if (EditorGUI.EndChangeCheck() &&
                _rectEnvironment.transform.InverseTransformPoint(startGlobalPosition).x +
                MIN_DISTANCE_BETWEEN_START_END < _rectEnvironment.EndPointLocalPosition.x)
            {
                if (_rectEnvironment.transform.TransformPoint(_rectEnvironment.EndPointLocalPosition).x >
                    startGlobalPosition.x + MIN_DISTANCE_BETWEEN_START_END)
                {
                    Undo.RecordObject(_rectEnvironment, "start pos");
                    var point = _rectEnvironment.transform.InverseTransformPoint(startGlobalPosition);
                    point.y = 0;
                    _rectEnvironment.StartPointLocalPosition = point;
                }
            }

            if (_rectEnvironment.EndPointLocalPosition.x <
                _rectEnvironment.StartPointLocalPosition.x + MIN_DISTANCE_BETWEEN_START_END)
            {
                _rectEnvironment.StartPointLocalPosition = _rectEnvironment.EndPointLocalPosition -
                                                           Vector3.right * MIN_DISTANCE_BETWEEN_START_END;
            }
        }



        Handles.color = Color.blue;
        var endGlobalPosition = _rectEnvironment.transform.TransformPoint(_rectEnvironment.EndPointLocalPosition);
        Handles.Label(endGlobalPosition + new Vector3(HandleUtility.GetHandleSize(endGlobalPosition) * 0.2f, HandleUtility.GetHandleSize(endGlobalPosition) * 0.2f),
            "end Line", style);
        Handles.color = Color.white;
        EditorGUI.BeginChangeCheck();
        endGlobalPosition = Handles.FreeMoveHandle(endGlobalPosition, _rectEnvironment.transform.rotation,
            1.5f, Vector3.zero, ArrowHandleCap);

        if (RectEnvironment.VERTICAL_DIRECTION)
        {
            if (EditorGUI.EndChangeCheck())
            {
                if (endGlobalPosition.y > startGlobalPosition.y + MIN_DISTANCE_BETWEEN_START_END)
                {
                    Undo.RecordObject(_rectEnvironment, "end pos");

                    var point = _rectEnvironment.transform.InverseTransformPoint(endGlobalPosition);
                    point.x = 0;
                    _rectEnvironment.EndPointLocalPosition = point;
                }
            }

            if (_rectEnvironment.EndPointLocalPosition.y <
                _rectEnvironment.StartPointLocalPosition.y + MIN_DISTANCE_BETWEEN_START_END)
            {
                _rectEnvironment.EndPointLocalPosition = _rectEnvironment.StartPointLocalPosition +
                                                         Vector3.up * MIN_DISTANCE_BETWEEN_START_END;
            }
        }
        else
        {
            if (EditorGUI.EndChangeCheck())
            {
                if (endGlobalPosition.x > startGlobalPosition.x + MIN_DISTANCE_BETWEEN_START_END)
                {
                    Undo.RecordObject(_rectEnvironment, "end pos");

                    var point = _rectEnvironment.transform.InverseTransformPoint(endGlobalPosition);
                    point.y = 0;
                    _rectEnvironment.EndPointLocalPosition = point;
                }
            }

            if (_rectEnvironment.EndPointLocalPosition.x <
                _rectEnvironment.StartPointLocalPosition.x + MIN_DISTANCE_BETWEEN_START_END)
            {
                _rectEnvironment.EndPointLocalPosition = _rectEnvironment.StartPointLocalPosition +
                                                         Vector3.right* MIN_DISTANCE_BETWEEN_START_END;
            }
        }


        serializedObject.ApplyModifiedProperties();

    }

    public void ArrowHandleCap(int controlID, Vector3 position, Quaternion rotation, float size,
        EventType eventType)
    {
        var distanceToStart = ((Vector2)position - (Vector2)_rectEnvironment.transform.TransformPoint(_rectEnvironment.StartPointLocalPosition)).magnitude;
        var distanceToEnd = ((Vector2)position - (Vector2)_rectEnvironment.transform.TransformPoint(_rectEnvironment.EndPointLocalPosition)).magnitude;
        var startMove = distanceToStart < distanceToEnd;
        Handles.ArrowHandleCap(controlID, position
            , rotation *(RectEnvironment.VERTICAL_DIRECTION? Quaternion.AngleAxis(startMove ? 90 : -90, Vector3.right): Quaternion.AngleAxis(startMove ? -90 : 90, Vector3.up))
            , size, eventType);
    }


    [MenuItem("Environment", menuItem = "GameObject/Rect Environment")]
    static void CreateBlockGroup()
    {
        var rectEnv = new GameObject("Environment");
        if (Selection.activeTransform != null)
        {
            rectEnv.transform.parent = Selection.activeTransform;
            rectEnv.transform.position = Selection.activeTransform.position;
        }

        var vertical = RectEnvironment.VERTICAL_DIRECTION;
        var bGroup = rectEnv.AddComponent<RectEnvironment>();
        bGroup.StartPointLocalPosition = new Vector3(vertical?0:-5f, vertical?-5f:0f);
        bGroup.EndPointLocalPosition = new Vector3(vertical?0:5f, vertical?5f:0f);

        Selection.activeObject = rectEnv;
    }
}