using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextTranslationsPair))]
public class TextTranslationsPairEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Debug.Log("OnInspectorGUI");
        //EditorGUILayout.LabelField(serializedObject.FindProperty("Text").stringValue);
        ////EditorGUILayout.PropertyField(serializedObject.FindProperty("Text"));
        EditorList.ShowList(serializedObject.FindProperty("Translations"));
       // EditorList.ShowList(serializedObject.FindProperty("test2List"));

        serializedObject.ApplyModifiedProperties();
    }
}
