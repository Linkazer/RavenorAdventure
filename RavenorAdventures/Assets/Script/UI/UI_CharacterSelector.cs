using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_CharacterSelector : MonoBehaviour
{
    [SerializeField] private Image portrait;
    [SerializeField] private Button button;

    [SerializeField] private UnityEvent<CPN_Character> OnSetCharacter;
    [SerializeField] private UnityEvent OnUnsetCharacter;

    private CPN_Character currentCharacter;

    public void Set(CPN_Character nCharacter)
    {
        currentCharacter = nCharacter;

        gameObject.SetActive(true);

        SetPortrait(currentCharacter.Scriptable.Portrait);

        OnSetCharacter?.Invoke(currentCharacter);
    }

    public void Unset()
    {
        OnUnsetCharacter?.Invoke();

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
