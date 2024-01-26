using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CharacterSheet : RVN_Singleton<UI_CharacterSheet>
{
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Image characterIcon;

    [SerializeField] private List<TextMeshProUGUI> stats = new List<TextMeshProUGUI>();

    [SerializeField] private List<UI_CharacterSheet_Effect> effects;

    private CPN_Character currentCharacter;

    protected override void OnAwake()
    {
        UnsetCharacter();
    }

    public static void TrySetCharacter(CPN_ClicHandler touchedObject)
    {
        if(touchedObject.Handler is CPN_Character)
        {
            instance.SetCharacter(touchedObject.Handler as CPN_Character);
        }
        else
        {
            instance.UnsetCharacter();
        }
    }

    public void SetCharacter(CPN_Character nCharacter)
    {
        if (nCharacter != currentCharacter)
        {
            UnsetCharacter();

            gameObject.SetActive(true);
        }

        currentCharacter = nCharacter;

        CharacterScriptable_Battle charaScriptable = currentCharacter.Scriptable;

        characterName.text = charaScriptable.Nom;
        characterIcon.sprite = charaScriptable.Portrait;

        if (nCharacter.GetComponentOfType<CPN_SpellCaster>(out CPN_SpellCaster casterHandler))
        {
            stats[1].text = casterHandler.Power.ToString();
            stats[3].text = casterHandler.Accuracy.ToString();
        }

        if (nCharacter.GetComponentOfType<CPN_HealthHandler>(out CPN_HealthHandler healthHandler))
        {
            stats[2].text = healthHandler.Defense.ToString();
            stats[0].text = healthHandler.CurrentHealth.ToString();
        }

        if (nCharacter.GetComponentOfType<CPN_EffectHandler>(out CPN_EffectHandler effectHandler))
        {
            for(int i = 0; i < effectHandler.Effects.Count; i++)
            {
                if (!effectHandler.Effects[i].GetEffect.HideOnApply)
                {
                    effects[i].SetEffect(effectHandler.Effects[i].GetEffect);
                    effects[i].gameObject.SetActive(true);
                }
            }
        }
    }

    public void UnsetCharacter()
    {
        foreach (UI_CharacterSheet_Effect effect in effects)
        {
            effect.gameObject.SetActive(false);
        }

        currentCharacter = null;


        gameObject.SetActive(false);
    }
}
