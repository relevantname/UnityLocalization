using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TextTranslationsPair))]
public class TextTranslationsPairDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var indent = EditorGUI.indentLevel;
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.indentLevel = 0;

        EditorGUI.PropertyField(position, property, false);
        if (property.isExpanded)
        {
            var textFieldRect = new Rect(position.x + 10, position.y + EditorGUIUtility.singleLineHeight, position.width - 40, position.height);
            property.FindPropertyRelative("Text").stringValue = EditorGUI.TextField(textFieldRect, "Text To Translate", property.FindPropertyRelative("Text").stringValue);
            var listPosRect = new Rect(textFieldRect.x + 10, textFieldRect.y + EditorGUIUtility.singleLineHeight, position.width - 40, position.height);
            EditorList.ShowList(listPosRect, property.FindPropertyRelative("Translations"));
        }
        EditorGUI.EndProperty();
        EditorGUI.indentLevel = indent;
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property);
    }
}
