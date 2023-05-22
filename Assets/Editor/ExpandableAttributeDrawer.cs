using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
/// <summary>
/// Draws the property field for any field.
/// </summary>
[CustomPropertyDrawer(typeof(UnityEngine.Object), true)]
public class ExpandableAttributeDrawer : PropertyDrawer
{

    Editor editor = null;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect fieldRect = new Rect(position);
        fieldRect.height = EditorGUIUtility.singleLineHeight;

        EditorGUI.PropertyField(fieldRect, property, label, true);

        if (property.objectReferenceValue == null)
            return;

        property.isExpanded = EditorGUI.Foldout(fieldRect, property.isExpanded, GUIContent.none, true);

        if (!property.isExpanded)
            return;

        SerializedObject targetObject = new SerializedObject(property.objectReferenceValue);

        if (targetObject == null)
            return;


        // Make child fields be indented
        EditorGUI.indentLevel++;

        // background
        GUILayout.BeginVertical("box");

        if (!editor)
            Editor.CreateCachedEditor(property.objectReferenceValue, null, ref editor);

        // Draw object properties
        EditorGUI.BeginChangeCheck();
        if (editor) // catch empty property
        {
            editor.OnInspectorGUI();
        }
        if (EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        GUILayout.EndVertical();

        // Set indent back to what it was
        EditorGUI.indentLevel--;

    }
}
#endif
