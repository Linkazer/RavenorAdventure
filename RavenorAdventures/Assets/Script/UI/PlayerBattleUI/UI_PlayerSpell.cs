using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UI_PlayerSpell : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image icon;

    [Header("Description")]
    [SerializeField] private RectTransform descriptionHandler;
    [SerializeField] private TextMeshProUGUI spellName;
    [SerializeField] private TextMeshProUGUI spellCost;
    [SerializeField] private GameObject spellUtilisationLeftHolder;
    [SerializeField] private TextMeshProUGUI spellUtilisationLeft;
    [SerializeField] private GameObject spellRessourceCostHolder;
    [SerializeField] private TextMeshProUGUI spellRessourceCost;
    [SerializeField] private TextMeshProUGUI spellCooldown;
    [SerializeField] private TextMeshProUGUI spellDescription;
    [SerializeField] private Image cooldown;

    [Header("Events")]
    [SerializeField] private UnityEvent OnSelectSpell;
    [SerializeField] private UnityEvent OnUnselectSpell;
    [SerializeField] private UnityEvent OnLockSpell;
    [SerializeField] private UnityEvent OnUnlockSpell;

    private SpellScriptable currentSpell;
    private int lastActionLeft = 1;

    public SpellScriptable Spell => currentSpell;

    public void CheckUsable(int actionLeft)
    {
        lastActionLeft = actionLeft;

        if (!currentSpell.IsLocked)
        {
            OnUnlockSpell?.Invoke();

            button.interactable = (actionLeft > 0 || currentSpell.CastType == SpellCastType.Fast) && (currentSpell.IsUsable);
        }
        else
        {
            OnLockSpell?.Invoke();

            button.interactable = false;
        }
    }

    public void SetSpell(SpellScriptable toSet)
    {
        currentSpell = toSet;
        icon.sprite = toSet.Icon;

        spellName.text = toSet.Name;
        spellDescription.SetText(toSet.GetDescription());

        if(toSet.RessourceCost != 0)
        {
            spellCost.text = toSet.RessourceCost.ToString();

            spellCost.gameObject.SetActive(true);
        }
        else
        {
            spellCost.gameObject.SetActive(false);
        }

        if(toSet.MaxUtilisation >= 0)
        {
            spellUtilisationLeft.text = toSet.UtilisationLeft.ToString();

            spellUtilisationLeftHolder.gameObject.SetActive(true);
        }
        else
        {
            spellUtilisationLeftHolder.gameObject.SetActive(false);
        }

        if(toSet.RessourceCost != 0)
        {
            spellRessourceCost.text = toSet.RessourceCost.ToString();

            spellRessourceCostHolder.SetActive(true);
        }
        else
        {
            spellRessourceCostHolder.SetActive(false);
        }

        if(toSet.StartCooldown > 0)
        {
            spellCooldown.text = toSet.StartCooldown.ToString();

            spellCooldown.gameObject.SetActive(true);
        }
        else
        {
            spellCooldown.gameObject.SetActive(false);
        }

        UpdateCooldown(currentSpell.CurrentCooldown);

        if (currentSpell.StartCooldown > 0)
        {
            currentSpell.OnUpdateCooldown += UpdateCooldown;
        }

        if (currentSpell.MaxUtilisation > 0)
        {
            currentSpell.OnUpdateUtilisationLeft += UpdateUtilisationLeft;
        }

        currentSpell.OnLockSpell += SetLock;

        gameObject.SetActive(true);
    }

    public void UnsetSpell()
    {
        if(currentSpell != null)
        {
            currentSpell.OnUpdateCooldown -= UpdateCooldown;

            currentSpell.OnUpdateUtilisationLeft -= UpdateUtilisationLeft;

            currentSpell.OnLockSpell -= SetLock;
        }

        currentSpell = null;

        gameObject.SetActive(false);
    }

    public void SelectSpell()
    {
        OnSelectSpell?.Invoke();
    }

    public void UnselectSpell()
    {
        OnUnselectSpell?.Invoke();
    }

    private void SetLock(bool toSet)
    {
        CheckUsable(lastActionLeft);
    }

    public void UpdateCooldown(int currentCooldown)
    {
        if (currentSpell.StartCooldown > 0)
        {
            cooldown.fillAmount = (float)currentCooldown / (float)currentSpell.StartCooldown;
        }
        else
        {
            cooldown.fillAmount = 0;
        }
    }

    public void UpdateUtilisationLeft(int currentUtilisation)
    {
        spellUtilisationLeft.text = currentUtilisation.ToString();
    }

    public void DisplaySpellDescription(bool show)
    {
        descriptionHandler.gameObject.SetActive(show);

        LayoutRebuilder.ForceRebuildLayoutImmediate(descriptionHandler);
    }
}
