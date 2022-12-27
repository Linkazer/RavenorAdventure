using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CharacterSheet_Effect : MonoBehaviour
{
    [SerializeField] private Image effectImage;

    [SerializeField] private CanvasGroup effectTooltipHolder;
    [SerializeField] private TextMeshProUGUI effectName;
    [SerializeField] private TextMeshProUGUI effectDescription;

    private EffectScriptable effectScriptable;

    public void SetEffect(EffectScriptable effect)
    {
        effectScriptable = effect;
        effectImage.sprite = effect.Icon;
    }

    public void ShowEffectDescription()
    {
        effectName.text = effectScriptable.Name;
        effectDescription.text = effectScriptable.Description;
        effectTooltipHolder.alpha = 1;

        LayoutRebuilder.ForceRebuildLayoutImmediate(effectTooltipHolder.transform as RectTransform);
    }

    public void HideEffectDscription()
    {
        effectTooltipHolder.alpha = 0;
    }
}
