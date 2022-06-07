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
    [SerializeField] private List<string> texts = new List<string>(Enum.GetNames(typeof(PossibleLanguage)).Length);

    public string Text()
    {
        return texts[((int)RVN_LanguageManager.Language)];
    }
}
