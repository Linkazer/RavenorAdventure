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

    [Header("Duration")]
    [SerializeField] private TextMeshProUGUI durationLeft;
    [SerializeField] private GameObject durationHandler;

    private AppliedEffect appliedEffect;

    public void SetEffect(AppliedEffect effect)
    {
        appliedEffect = effect;
        effectImage.sprite = effect.GetEffect.Icon;

        if (effect.Duration > 0)
        {
            effect.actOnDurationUpdate += UpdateEffect;
            durationHandler.SetActive(true);
            UpdateEffect();
        }
        else
        {
            durationHandler.SetActive(true);
        }

        effect.actOnRemoveEffect += UnsetEffect;

        gameObject.SetActive(true);
    }

    public void UnsetEffect()
    {
        if (appliedEffect != null)
        {
            appliedEffect.actOnDurationUpdate -= UpdateEffect;
            appliedEffect.actOnRemoveEffect -= UnsetEffect;
        }
        gameObject.SetActive(false);
    }

    public void UpdateEffect()
    {
        durationLeft.text = Mathf.CeilToInt(appliedEffect.Duration).ToString();
    }

    public void ShowEffectDescription()
    {
        effectName.text = appliedEffect.GetEffect.Name;
        effectDescription.text = appliedEffect.GetEffect.Description;
        effectTooltipHolder.alpha = 1;

        LayoutRebuilder.ForceRebuildLayoutImmediate(effectTooltipHolder.transform as RectTransform);
    }

    public void HideEffectDscription()
    {
        effectTooltipHolder.alpha = 0;
    }
}
