using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;

public class LocalizationEditor : EditorWindow
{
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

    private void CreateNewTranslation()
    {
        translations = new List<TextTranslationsPair>();
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
    }
}
