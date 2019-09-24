using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EditorList
{
    public static void ShowList(Rect pos, SerializedProperty list)
    {
        EditorGUI.PropertyField(pos, list); // Showing Property without list elements.
        EditorGUI.indentLevel += 1;  // Giving an padding(indent) from left
        if (list.isExpanded) // For foldout feature
        {
            for (int i = 0; i < list.arraySize; i++)
            {
                Rect listPos = new Rect(pos.x + 10, pos.y += EditorGUIUtility.singleLineHeight + 1, Screen.width * 0.9f, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(pos, list.GetArrayElementAtIndex(i));
            }
        }
        EditorGUI.indentLevel -= 1;
    }
}
