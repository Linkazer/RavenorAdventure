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
    [SerializeField, TextArea(2,5)] private string frenchText = "";
    [SerializeField, TextArea(2,5)] private string englishText = "";

    public string GetText()
    {
        switch(RVN_LanguageManager.Language)
        {
            case PossibleLanguage.Francais:
                return frenchText;
            case PossibleLanguage.English:
                if (englishText != "")
                {
                    return englishText;
                }
                break;
        }

        return frenchText;
    }
}
