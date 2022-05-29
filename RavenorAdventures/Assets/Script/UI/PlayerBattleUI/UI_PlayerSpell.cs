using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_PlayerSpell : MonoBehaviour
{
    [SerializeField] private EventTrigger handler;
    [SerializeField] private Image icon;

    [Header("Description")]
    [SerializeField] private GameObject descriptionHandler;
    [SerializeField] private TextMeshProUGUI spellName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image cooldown;

    private SpellScriptable currentSpell;

    public void SetUsable(int actionLeft)
    {
        handler.enabled = actionLeft > 0;
    }

    public void SetSpell(SpellScriptable toSet)
    {
        currentSpell = toSet;
        icon.sprite = toSet.Icon;

        spellName.text = toSet.Name;
        description.text = toSet.Description;

        UpdateCooldown(currentSpell.CurrentCooldown);

        if (currentSpell.StartCooldown > 0)
        {
            currentSpell.OnUpdateCooldown += UpdateCooldown;
        }

        gameObject.SetActive(true);
    }

    public void UnsetSpell()
    {
        if(currentSpell != null)
        {
            currentSpell.OnUpdateCooldown -= UpdateCooldown;
        }

        currentSpell = null;

        gameObject.SetActive(false);
    }

    public void UpdateCooldown(int currentCooldown)
    {
        if (currentSpell.StartCooldown > 0)
        {
            cooldown.fillAmount = (float)currentCooldown / (float)currentSpell.StartCooldown;

            if(cooldown.fillAmount > 0)
            {
                handler.enabled = false;
            }
            else
            {
                handler.enabled = true;
            }
        }
        else
        {
            cooldown.fillAmount = 0;
        }
    }

    public void DisplaySpellDescription(bool show)
    {
        descriptionHandler.SetActive(show);
    }
}
