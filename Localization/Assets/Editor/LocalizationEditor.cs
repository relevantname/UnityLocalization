using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using UnityEditorInternal;
using UnityEditor.AnimatedValues;

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
    private string languageCodeToAdd = "";
    private string languageCodeToRemove = "";
    private Vector2 scrollPos;
    int selectedIndex = -1;
    #endregion

    #region Translation Panel Section
    private Rect translationPanelRect;
    private ReorderableList reordarableList_Translations;
    
    private bool show;
    #endregion

    #region LocalizationEditor Buttons Section Properties(Save, Load, Clear)
    private Rect localizationEditorButtonsRect;
    #endregion

    SerializedObject serializedObject;
    SerializedProperty serializedProperty;
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

    /// <summary>
    /// Draws All GUI Elements inside Editor Window
    /// </summary>
    private void OnGUI()
    {
        DrawLayouts();
        HeaderSectionContent();
        LanguageCodesSectionContent();
        TranslationsPanelSectionContent();
        LocalizationEditorButtonsSectionContent();
    }

    /// <summary>
    /// When the Editor Window opened, we draw the textures and create reordarable list.
    /// Currenty it only draws a texture for Header Title, but additional textures can be added.
    /// 
    /// </summary>
    private void OnEnable()
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
    /// Draws the sections of Editor Window.
    /// We seperate all the main sections of Editor Window for easily editing the layouts.
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
        languageCodesRect.y = headerRect.height + 5;
        languageCodesRect.width = Screen.width;
        languageCodesRect.height = 200;
        #endregion

        #region Translations Panel Section
        translationPanelRect.x = 0;
        translationPanelRect.y = headerRect.height + languageCodesRect.height + 5;
        translationPanelRect.width = Screen.width;
        translationPanelRect.height = 500;
        #endregion

        #region Localization Editor Buttons Section
        localizationEditorButtonsRect.x = 0;
        localizationEditorButtonsRect.y = headerRect.height + languageCodesRect.height + translationPanelRect.height;
        localizationEditorButtonsRect.width = Screen.width;
        localizationEditorButtonsRect.height = 100;
        #endregion
    }

    /// <summary>
    /// Creation of GUI Editor Content of Header Section
    /// </summary>
    private void HeaderSectionContent()
    {
        GUILayout.BeginArea(headerRect);

        #region Editor Window Title
        GUIStyle headerTextGUIStyle = new GUIStyle();
        headerTextGUIStyle.alignment = TextAnchor.MiddleCenter;
        headerTextGUIStyle.fontStyle = FontStyle.BoldAndItalic;
        headerTextGUIStyle.normal.textColor = Color.white;
        EditorGUI.LabelField(headerRect, "LOCALIZATION EDITOR", headerTextGUIStyle);
        #endregion

        GUILayout.EndArea();
    }

    /// <summary>
    /// Creation of GUI Editor Content of Add/Remove Language Codes Section
    /// </summary>
    private void LanguageCodesSectionContent()
    {
        GUILayout.BeginArea(languageCodesRect);

        GUILayout.BeginVertical();

        #region Section Title
        GUIStyle languageCodesGUIStyle = new GUIStyle();
        languageCodesGUIStyle.alignment = TextAnchor.UpperLeft;
        languageCodesGUIStyle.fontStyle = FontStyle.Italic;
        languageCodesGUIStyle.normal.textColor = Color.white;
        EditorGUI.LabelField(new Rect(10,10, 0,0), "Add/Remove Language Codes", languageCodesGUIStyle);
        #endregion

        GUILayout.Space(30);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width-10), GUILayout.Height(150));

        #region Selectable Language Codes List
        Color color_default = GUI.backgroundColor;
        Color color_selected = Color.grey;

        GUIStyle itemStyle = new GUIStyle(GUI.skin.button);  //make a new GUIStyle

        itemStyle.alignment = TextAnchor.MiddleLeft; //align text to the left
        itemStyle.active.background = itemStyle.normal.background;  //gets rid of button click background style.
        itemStyle.margin = new RectOffset(0, 0, 0, 0); //removes the space between items (previously there was a small gap between GUI which made it harder to select a desired item)
        
        for (int i = 0; i < languageCodes.Count; i++)
        {
            GUI.backgroundColor = (selectedIndex == i) ? color_selected : Color.clear;

            //show a button using the new GUIStyle
            if (GUILayout.Button(languageCodes[i], itemStyle))
            {
                selectedIndex = i;
                languageCodeToRemove = languageCodes[i];
            }

            GUI.backgroundColor = color_default;
        }
        #endregion

        EditorGUILayout.EndScrollView();

        GUILayout.Space(5);

        EditorGUILayout.BeginHorizontal();

        #region Language Codes List Controls
        languageCodeToAdd = EditorGUILayout.TextField("Language Code To Add:", languageCodeToAdd);

        if (GUILayout.Button("Add Language Code"))
            AddNewLanguageCode();

        if (GUILayout.Button("Remove Language Code"))
            RemoveLanguageCode();
        #endregion

        EditorGUILayout.EndHorizontal();

        GUILayout.EndVertical();

        GUILayout.Space(5);

        GUILayout.EndArea();
    }

    /// <summary>
    /// Panel For Adding/Removing Translation Texts and Editing Translations
    /// </summary>
    private void TranslationsPanelSectionContent()
    {
        GUILayout.BeginArea(translationPanelRect);

        GUILayout.BeginVertical();

        #region Title
        GUIStyle translationPanelGUIStyle = new GUIStyle();
        translationPanelGUIStyle.alignment = TextAnchor.UpperLeft;
        translationPanelGUIStyle.fontStyle = FontStyle.Italic;
        translationPanelGUIStyle.normal.textColor = Color.white;
        EditorGUI.LabelField(new Rect(10, 10, 0, 0), "Manage Translations", translationPanelGUIStyle);
        #endregion

        GUILayout.Space(30);

        reordarableList_Translations.DoLayoutList();
        reordarableList_Translations.serializedProperty.serializedObject.ApplyModifiedProperties();

        GUILayout.EndVertical();
          
        GUILayout.EndArea();
    }

    /// <summary>
    /// Creation of GUI Editor Content of LocalizationEditorButtonsSection
    /// </summary>
    private void LocalizationEditorButtonsSectionContent()
    {
        GUILayout.BeginArea(localizationEditorButtonsRect);

        #region Button Implementations
        if (GUILayout.Button("Save Translations"))
            Save();

        if (GUILayout.Button("Load Translations"))
            Load();

        if (GUILayout.Button("Clear Translations"))
            CreateNewTranslation();
        #endregion

        GUILayout.EndArea();
    }

    #region Reordarable List Methods
    private void DrawReordarableTranslationsList(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = reordarableList_Translations.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;

        SerializedProperty elementName = element.FindPropertyRelative("Text");
        string elementTitle = string.IsNullOrEmpty(elementName.stringValue) ? "New Text To Translate" : elementName.stringValue;

        Rect propertyFieldRect = new Rect(rect.x += 10, rect.y, Screen.width * 0.8f, 150);

        //GUILayout.BeginArea(propertyFieldRect);

        //element.FindPropertyRelative("Text").stringValue = EditorGUILayout.TextField("Text To Translate:", element.FindPropertyRelative("Text").stringValue);
        //show = EditorGUILayout.BeginFoldoutHeaderGroup(show, EditorGUILayout.TextField("Text To Translate:", element.FindPropertyRelative("Text").stringValue));

        //SerializedProperty translations = element.FindPropertyRelative("Translations");
        //for (int i = 0; i < translations.arraySize; i++)
        //{
        //    var lctp = translations.GetArrayElementAtIndex(i);
        //    lctp.FindPropertyRelative("Translation").stringValue = EditorGUILayout.TextField(lctp.FindPropertyRelative("LanguageCode").stringValue, lctp.FindPropertyRelative("Translation").stringValue);
        //}
        //EditorGUILayout.EndFoldoutHeaderGroup();

        //GUILayout.EndArea();

        //Rect propertyFieldRect = new Rect(rect.x += 10, rect.y + 50, Screen.width * 0.8f, EditorGUIUtility.singleLineHeight);
        //GUILayout.BeginArea(propertyFieldRect);

        //element.FindPropertyRelative("Text").stringValue = EditorGUILayout.TextField("Text To Translate:", element.FindPropertyRelative("Text").stringValue);
        //EditorList.ShowList(element.FindPropertyRelative("Translations"));

        //GUILayout.EndArea();
        element.FindPropertyRelative("Text").stringValue = EditorGUILayout.TextField("Text To Translate:", element.FindPropertyRelative("Text").stringValue);
        EditorGUI.PropertyField(propertyFieldRect, element, new GUIContent(elementTitle), true);
    }
    private float ElementHeightCallback(int index)
    {
        //return (EditorGUIUtility.singleLineHeight*2) + (EditorGUIUtility.singleLineHeight * languageCodes.Count);
        float propertyHeight = EditorGUI.GetPropertyHeight(reordarableList_Translations.serializedProperty.GetArrayElementAtIndex(index), true);
        float spacing = EditorGUIUtility.singleLineHeight / 2;

        return propertyHeight + spacing;
    }
    private void OnAddCallback(ReorderableList reordarableList)
    {
        var index = reordarableList.serializedProperty.arraySize;
        reordarableList.serializedProperty.arraySize++;
        reordarableList.index = index;

        var element = reordarableList.serializedProperty.GetArrayElementAtIndex(index);

        TextTranslationsPair ttp = translations[translations.Count - 1];
        foreach (string langCode in languageCodes)
        {
            LanguageCodeTranslationPair lctp = new LanguageCodeTranslationPair(langCode, "");
            ttp.Translations.Add(lctp);
        }

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
    #endregion

    #region Add/Remove Translation Languages
    /// <summary>
    /// Adds the new language(with language code) to the translations if not exists.
    /// </summary>
    private void AddNewLanguageCode()
    {
        if (languageCodeToAdd.Length <= 0)
            return;

        if (languageCodes.Contains(languageCodeToAdd))
            return;

        for(int i=0; i < translations.Count; i++)
        {
            LanguageCodeTranslationPair newLangTranslationPair = new LanguageCodeTranslationPair(languageCodeToAdd, "");
            //newLangTranslationPair.LanguageCode = languageCodeToAdd;
            translations[i].Translations.Add(newLangTranslationPair);
        }

        languageCodes.Add(languageCodeToAdd);
        languageCodeToAdd = "";
        
        serializedObject.Update();
    }

    /// <summary>
    /// Removes the selected language(with language code) from the translations if exists.
    /// </summary>
    private void RemoveLanguageCode()
    {
        if (languageCodeToRemove.Length <= 0)
            return;

        if (!languageCodes.Contains(languageCodeToRemove))
            return;

        for (int i = 0; i < translations.Count; i++)
        {
            for(int j=translations[i].Translations.Count-1; j>=0; j--)
            {
                if(translations[i].Translations[j].LanguageCode == languageCodeToRemove)
                {
                    translations[i].Translations.RemoveAt(j);
                    break;
                }
            }
        }

        languageCodes.Remove(languageCodeToRemove);
        languageCodeToRemove = "";

        serializedObject.Update();
    }
    #endregion

    #region Save,Load and New Translation
    /// <summary>
    /// Converts the translations to JSON format and saves to selected directory.
    /// </summary>
    private void Save()
    {
        if (translations == null)
            return;

        string savePath = EditorUtility.SaveFilePanel("Save", "", "lang", ".txt");

        if (string.IsNullOrEmpty(savePath))
            return;

        string translationsAsJson = JsonConvert.SerializeObject(translations);
        File.WriteAllText(savePath, translationsAsJson);
    }
    /// <summary>
    /// Converts selected text(Json) file to translations object and show it in the editor window.
    /// </summary>
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

        serializedObject = new SerializedObject(this);
        serializedProperty = serializedObject.FindProperty("translations");
        reordarableList_Translations = new ReorderableList(serializedObject, serializedProperty, true, false, true, true);
        reordarableList_Translations.drawElementCallback = DrawReordarableTranslationsList;
        reordarableList_Translations.elementHeightCallback += ElementHeightCallback;
    }
    /// <summary>
    /// Creates a new translations.
    /// </summary>
    private void CreateNewTranslation()
    {
        translations = new List<TextTranslationsPair>();
        languageCodes = new List<string>();

        serializedObject = new SerializedObject(this);
        serializedProperty = serializedObject.FindProperty("translations");
        reordarableList_Translations = new ReorderableList(serializedObject, serializedProperty, true, false, true, true);
        reordarableList_Translations.drawElementCallback = DrawReordarableTranslationsList;
        reordarableList_Translations.elementHeightCallback += ElementHeightCallback;
    }
    #endregion
}
//SerializedObject serializedObject = new SerializedObject(this);
//SerializedProperty serializedProperty = serializedObject.FindProperty("translations");
//EditorGUILayout.PropertyField(serializedProperty, true);
//serializedObject.ApplyModifiedProperties();