using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LocalizationEditor : EditorWindow
{
    /// <summary>
    /// Initializes the editor window.
    /// </summary>
    [MenuItem("Window/Localization Editor")]
    private static void InitializeWindow()
    {
        LocalizationEditor window = (LocalizationEditor)GetWindow(typeof(LocalizationEditor));
        window.Show();
    }
}
