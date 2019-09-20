using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TextTranslationsPair))]
public class TextTranslationsPairDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        //var textFieldRect = new Rect(position.x + 10, position.y, position.width - 40, position.height);
        //EditorGUI.PropertyField(textFieldRect, property.FindPropertyRelative("Text"), GUIContent.none);
        EditorList.ShowList(property.FindPropertyRelative("Translations"));
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
