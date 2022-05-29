using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterSelector : MonoBehaviour
{
    [SerializeField] private Image portrait;
    [SerializeField] private Button button;

    private CPN_Character currentCharacter;

    public void Set(CPN_Character nCharacter)
    {
        currentCharacter = nCharacter;

        gameObject.SetActive(true);

        SetPortrait(nCharacter.Scriptable.Portrait);
    }

    public void Unset()
    {
        currentCharacter = null;

        gameObject.SetActive(false);
    }

    public void SelectCharacter()
    {
        RVN_CombatInputController.ChangeCharacter(currentCharacter);
    }

    public void SetPortrait(Sprite sprite)
    {
        portrait.sprite = sprite;
    }
}
