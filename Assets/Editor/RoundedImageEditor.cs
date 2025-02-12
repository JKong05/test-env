#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.UI; // Needed to inherit from ImageEditor

[CustomEditor(typeof(RoundedImage))]
public class RoundedImageEditor : ImageEditor
{
    // Serialized properties for your custom fields
    SerializedProperty m_TopLeftRadius;
    SerializedProperty m_TopRightRadius;
    SerializedProperty m_BottomRightRadius;
    SerializedProperty m_BottomLeftRadius;
    SerializedProperty m_CornerSegments;

    protected override void OnEnable()
    {
        // Initialize the default ImageEditor properties.
        base.OnEnable();

        // Find your custom properties by name
        m_TopLeftRadius = serializedObject.FindProperty("m_TopLeftRadius");
        m_TopRightRadius = serializedObject.FindProperty("m_TopRightRadius");
        m_BottomRightRadius = serializedObject.FindProperty("m_BottomRightRadius");
        m_BottomLeftRadius = serializedObject.FindProperty("m_BottomLeftRadius");
        m_CornerSegments = serializedObject.FindProperty("m_CornerSegments");
    }

    public override void OnInspectorGUI()
    {
        // First draw the default Image properties.
        base.OnInspectorGUI();

        // Add some spacing and a header for your custom properties.
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Rounded Corners Settings", EditorStyles.boldLabel);

        // Draw your custom properties.
        EditorGUILayout.PropertyField(m_TopLeftRadius, new GUIContent("Top Left Radius"));
        EditorGUILayout.PropertyField(m_TopRightRadius, new GUIContent("Top Right Radius"));
        EditorGUILayout.PropertyField(m_BottomRightRadius, new GUIContent("Bottom Right Radius"));
        EditorGUILayout.PropertyField(m_BottomLeftRadius, new GUIContent("Bottom Left Radius"));
        EditorGUILayout.PropertyField(m_CornerSegments, new GUIContent("Corner Segments"));

        // Apply any changes made in the inspector.
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
