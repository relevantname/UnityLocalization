using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LanguageCodeTranslationPair))]
public class LanguageCodeTranslationPairDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var indent = EditorGUI.indentLevel;
        EditorGUI.BeginProperty(position, label, property);

        // Label for this property. (In this case it is Language Codes)
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        EditorGUI.indentLevel = 0;
        var textFieldRect = new Rect(position.x + 10, position.y, position.width, position.height);
        // Creating proprty field for Translation text. 
        EditorGUI.PropertyField(textFieldRect, property.FindPropertyRelative("Translation"), GUIContent.none);
        
        EditorGUI.EndProperty();
        EditorGUI.indentLevel = indent;
    }
}
