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

    [SerializeField] private UnityEvent onShow;
    [SerializeField] private UnityEvent onHide;

    private CPN_Character currentCharacter;

    public void Set(CPN_Character nCharacter)
    {
        currentCharacter = nCharacter;

        currentCharacter.ActOnBeginTeamTurn += Show;
        currentCharacter.ActOnEndSelfTurn += Hide;

        gameObject.SetActive(true);

        SetPortrait(currentCharacter.Scriptable.Portrait);

        OnSetCharacter?.Invoke(currentCharacter);
    }

    public void Unset()
    {
        OnUnsetCharacter?.Invoke();

        if (currentCharacter != null)
        {
            currentCharacter.ActOnBeginTeamTurn -= Show;
            currentCharacter.ActOnEndSelfTurn -= Hide;
        }

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
        portrait.color = Color.white;
    }

    private void Show(RVN_ComponentHandler handler)
    {
        onShow?.Invoke();
    }

    private void Hide(RVN_ComponentHandler handler)
    {
        onHide?.Invoke();
    }
}
