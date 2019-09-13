using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
public class TextTranslationsPair
{
    public string Text;
    public List<LanguageCodeTranslationPair> Translations;
}

