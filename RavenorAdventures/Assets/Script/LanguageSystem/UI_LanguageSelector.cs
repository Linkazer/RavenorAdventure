using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_LanguageSelector : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    private void Start()
    {
        dropdown.SetValueWithoutNotify((int)RVN_LanguageManager.Language);
    }

    public void UE_SetLanguage(int langIndex)
    {
        RVN_LanguageManager.SetLanguage(langIndex);
        RVN_SceneManager.ReloadScene();
    }
}
