using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PossibleLanguage
{
    Francais,
    English
}

[Serializable]
public class RVN_Text
{
    [SerializeField, TextArea(2,3)] private List<string> texts = new List<string>(Enum.GetNames(typeof(PossibleLanguage)).Length);

    public string GetText()
    {
        return texts[((int)RVN_LanguageManager.Language)];
    }
}
