using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using UnityEditorInternal;

public class LocalizationEditor : EditorWindow
{
    #region Header Section Properties
    private Texture2D headerTexture;
    private Rect headerRect;
    private Color headerColor = new Color(0, 74.0f / 255.0f, 145.0f / 255.0f, 255.0f / 255.0f);
    #endregion

    //private Texture2D testtexture;
    //private Color testtextureColor = Color.green;

    #region Language Codes Section Properties
    private Rect languageCodesRect;

    private List<string> languageCodes;
    private ReorderableList languageCodesList;
    #endregion

    public List<TextTranslationsPair> translations;

    /// <summary>
    /// Initializes the editor window.
    /// </summary>
    [MenuItem("Window/Localization Editor")]
    private static void InitializeWindow()
    {
        LocalizationEditor window = (LocalizationEditor)GetWindow(typeof(LocalizationEditor));
        window.CreateNewTranslation();
        window.Show();
    }

    private void OnGUI()
    {
        DrawLayouts();
        HeaderSectionContent();
        LanguageCodesSectionContent();

        if (translations != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("translations");
            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Save Translations"))
                Save();
        }

        if (GUILayout.Button("Load Translations"))
            Load();

        if (GUILayout.Button("Clear Translations"))
            CreateNewTranslation();
    }

    void OnEnable()
    {
        InitializeTextures();
    }

    /// <summary>
    /// Initializes the section Textures
    /// </summary>
    private void InitializeTextures()
    {
        headerTexture = new Texture2D(1, 1);
        headerTexture.SetPixel(0, 0, headerColor);
        headerTexture.Apply();

        //testtexture = new Texture2D(1, 1);
        //testtexture.SetPixel(0, 0, testtextureColor);
        //testtexture.Apply();
    }

    /// <summary>
    /// Draws the sections of Editor Window
    /// </summary>
    private void DrawLayouts()
    {
        #region Header Section
        headerRect.x = 0;
        headerRect.y = 0;
        headerRect.width = Screen.width;
        headerRect.height = 30;

        GUI.DrawTexture(headerRect, headerTexture);
        #endregion

        #region Language Codes Section
        languageCodesRect.x = 0;
        languageCodesRect.y = headerRect.height;
        languageCodesRect.width = Screen.width;
        languageCodesRect.height = 100;
        #endregion
    }

    /// <summary>
    /// Creation of GUI Editor Content of Header Section
    /// </summary>
    private void HeaderSectionContent()
    {
        GUILayout.BeginArea(headerRect);
        GUIStyle headerTextGUIStyle = new GUIStyle();
        headerTextGUIStyle.alignment = TextAnchor.MiddleCenter;
        headerTextGUIStyle.fontStyle = FontStyle.BoldAndItalic;
        headerTextGUIStyle.normal.textColor = Color.white;
        EditorGUI.LabelField(headerRect, "LOCALIZATION EDITOR", headerTextGUIStyle);
        GUILayout.EndArea();
    }

    /// <summary>
    /// Creation of GUI Editor Content of Add/Remove Language Codes Section
    /// </summary>
    private void LanguageCodesSectionContent()
    {
        GUILayout.BeginArea(languageCodesRect);
        GUIStyle languageCodesGUIStyle = new GUIStyle();
        languageCodesGUIStyle.alignment = TextAnchor.UpperLeft;
        languageCodesGUIStyle.fontStyle = FontStyle.Italic;
        languageCodesGUIStyle.normal.textColor = Color.white;
        EditorGUI.LabelField(new Rect(10,10, 0,0), "Add/Remove Language Codes", languageCodesGUIStyle);

        languageCodesList = new ReorderableList(languageCodes, typeof(string), false, true, false, true);
        languageCodesList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Language Codes");
        //languageCodesList.DoList(new Rect(10, 25, 150, 150));  
        languageCodesList.DoLayoutList();
        GUILayout.EndArea();
    }

    private void Save()
    {
        string savePath = EditorUtility.SaveFilePanel("Save", "", "lang", ".txt");

        if (string.IsNullOrEmpty(savePath))
            return;

        string translationsAsJson = JsonConvert.SerializeObject(translations);
        File.WriteAllText(savePath, translationsAsJson);
    }

    private void Load()
    {
        string openPath = EditorUtility.OpenFilePanel("Open", "", ".txt");

        if (string.IsNullOrEmpty(openPath))
            return;

        string translationsAsJson = File.ReadAllText(openPath);
        translations = JsonConvert.DeserializeObject<List<TextTranslationsPair>>(translationsAsJson);

        languageCodes.Clear();
        for(int i=0; i < translations[0].Translations.Count; i++)
        {
            languageCodes.Add(translations[0].Translations[i].LanguageCode);
        }
    }

    private void CreateNewTranslation()
    {
        translations = new List<TextTranslationsPair>();
        languageCodes = new List<string>();
    }
}
