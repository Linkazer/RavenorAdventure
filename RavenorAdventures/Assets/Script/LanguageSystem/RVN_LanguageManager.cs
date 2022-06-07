using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RVN_LanguageManager : RVN_Singleton<RVN_LanguageManager>
{
    [SerializeField] private PossibleLanguage currentLanguage;

    public static PossibleLanguage Language => instance.currentLanguage;
}
