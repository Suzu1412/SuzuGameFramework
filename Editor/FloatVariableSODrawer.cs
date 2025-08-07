#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FloatVariableSO))]
public class FloatVariableSODrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var so = property.objectReferenceValue as FloatVariableSO;
        return so == null
            ? EditorGUIUtility.singleLineHeight
            : EditorGUIUtility.singleLineHeight * 4;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var lineHeight = EditorGUIUtility.singleLineHeight;
        var y = position.y;

        // Always draw the object field
        EditorGUI.ObjectField(new Rect(position.x, y, position.width, lineHeight), property, label);
        y += lineHeight;

        // Draw SO info if assigned
        var so = property.objectReferenceValue as FloatVariableSO;
        if (so != null)
        {
            EditorGUI.LabelField(new Rect(position.x, y, position.width, lineHeight), $"Base Value: {so.BaseValue}");
            y += lineHeight;
            EditorGUI.LabelField(new Rect(position.x, y, position.width, lineHeight), $"Min: {so.MinValue}   Max: {so.MaxValue}");
            y += lineHeight;
            EditorGUI.LabelField(new Rect(position.x, y, position.width, lineHeight), $"Current Value: {so.Value}");

        }

        EditorGUI.EndProperty();
    }
}
#endif