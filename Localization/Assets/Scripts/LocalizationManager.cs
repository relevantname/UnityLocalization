using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    private Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
    public string selectedLanguage = "TR";

    private void Awake()
    {
        #region Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
        #endregion

        DontDestroyOnLoad(this);

        LoadLocalizationFile();
    }

    /// <summary>
    /// Returns the localized text.
    /// </summary>
    /// <param name="textToLocalize"></param>
    /// <returns></returns>
    public string GetLocalizedText(string textToLocalize, string languageCode)
    {
        if (!dictionary.ContainsKey(textToLocalize))
            return textToLocalize;

        if (!dictionary[textToLocalize].ContainsKey(languageCode))
            return textToLocalize;

        return dictionary[textToLocalize][languageCode];
    }
    public string GetLocalizedText(string textToLocalize)
    {
        if (!dictionary.ContainsKey(textToLocalize))
            return textToLocalize;

        if (!dictionary[textToLocalize].ContainsKey(selectedLanguage))
            return textToLocalize;

        return dictionary[textToLocalize][selectedLanguage];
    }

    /// <summary>
    /// Gets the translation file from Resources folder. And load it to dictionary.
    /// </summary>
    private void LoadLocalizationFile()
    {
        TextAsset textAsset = Resources.Load("Localization/lang") as TextAsset;
        string langFileContent = textAsset.text;
        List<TextTranslationsPair> textTranslationPairs = JsonConvert.DeserializeObject<List<TextTranslationsPair>>(langFileContent);

        dictionary.Clear();
        for (int i = 0; i < textTranslationPairs.Count; i++)
        {
            Dictionary<string, string> langCodeTranslation = new Dictionary<string, string>();
            for (int j = 0; j < textTranslationPairs[i].Translations.Count; j++)
            {
                langCodeTranslation.Add(textTranslationPairs[i].Translations[j].LanguageCode, textTranslationPairs[i].Translations[j].Translation);
            }
            dictionary.Add(textTranslationPairs[i].Text, langCodeTranslation);
        }
    }
}
