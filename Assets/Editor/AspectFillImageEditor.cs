using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(AspectFillImage))]
public class AspectFillImageEditor : Editor
{
    private SerializedProperty _imageProperty;
    private AspectFillImage _aspectFillImage;
    private Editor _imageEditor;


    private void OnEnable()
    {
        _imageProperty = serializedObject.FindProperty("_image");
        _aspectFillImage = (AspectFillImage)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUI.indentLevel++;
        EditorGUILayout.BeginHorizontal();
        _imageProperty.isExpanded = EditorGUILayout.Foldout(_imageProperty.isExpanded, "Image");
        _imageProperty.objectReferenceValue = EditorGUILayout.ObjectField(_imageProperty.objectReferenceValue,typeof(Image),true);
        if (_imageProperty.objectReferenceValue == null)
        {
            if (GUILayout.Button("Create"))
            {
                var gameObject = new GameObject("Image");
                var image = gameObject.AddComponent<Image>();
                image.transform.parent = _aspectFillImage.transform;
                _imageProperty.objectReferenceValue = image;
                serializedObject.ApplyModifiedProperties();
            }
        }
        EditorGUILayout.EndHorizontal();
        if (_imageProperty.isExpanded && _imageProperty.objectReferenceValue != null)
        {
            EditorGUILayout.Space();
//            EditorGUI.indentLevel++;
            CreateCachedEditor(_imageProperty.objectReferenceValue, typeof(ImageEditor),ref _imageEditor);
            _imageEditor.OnInspectorGUI();
//            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();
        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}