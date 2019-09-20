using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EditorList
{
    public static void ShowList(SerializedProperty list)
    {
        EditorGUILayout.PropertyField(list);
        EditorGUI.indentLevel += 1;  // Giving an padding(indent) from left
        if (list.isExpanded) // For foldout feature
        {
            for (int i = 0; i < list.arraySize; i++)
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
        }
        EditorGUI.indentLevel -= 1;
    }
}
