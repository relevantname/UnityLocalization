using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LanguageCodeTranslationPair
{

    public readonly string LanguageCode;
    public string Translation;

    public LanguageCodeTranslationPair(string languageCode, string translation)
    {
        this.LanguageCode = languageCode;
        this.Translation = translation;
    }
}
