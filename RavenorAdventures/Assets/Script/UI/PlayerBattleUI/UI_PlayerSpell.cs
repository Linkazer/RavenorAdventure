using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UI_PlayerSpell : MonoBehaviour
{
    [System.Serializable]
    private class RessourceHolder
    {
        public GameObject gameObject;
        public TextMeshProUGUI text;
    }

    [SerializeField] private UI_SpellChoiceDisplay spellChoiceUI;
    [SerializeField] private int spellIndex;

    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private Image icon;

    [Header("Description")]
    [SerializeField] private RectTransform descriptionHandler;
    [SerializeField] private TextMeshProUGUI spellName;
    [SerializeField] private TextMeshProUGUI spellCost;
    [SerializeField] private TextMeshProUGUI spellCooldown;
    [SerializeField] private TextMeshProUGUI spellDescription;
    [SerializeField] private Image cooldown;

    [Header("Utilisation")]
    [SerializeField] private GameObject spellUtilisationLeftHolder;
    [SerializeField] private TextMeshProUGUI spellUtilisationLeft;

    [Header("Ressources")]
    [SerializeField] private RessourceHolder[] ressourceHolders;

    [Header("Cast Time")]
    [SerializeField] private GameObject actionCostNormal;
    [SerializeField] private GameObject actionCostFast;

    [Header("Events")]
    [SerializeField] private UnityEvent OnSelectSpell;
    [SerializeField] private UnityEvent OnUnselectSpell;
    [SerializeField] private UnityEvent OnLockSpell;
    [SerializeField] private UnityEvent OnUnlockSpell;

    private SpellScriptable currentSpell;

    public SpellScriptable Spell => currentSpell;

    public void CheckUsable(RVN_ComponentHandler characterHandler)
    {
        if (characterHandler.GetComponentOfType(out CPN_SpellCaster caster))
        {
            if (!currentSpell.IsLocked)
            {
                OnUnlockSpell?.Invoke();

                if (currentSpell.name == "Lisbeth_ComboFast")
                {
                    Debug.Log(currentSpell.IsUsable);
                    Debug.Log(currentSpell.CurrentCooldown);
                }

                button.interactable = (caster.ActionLeftThisTurn > 0 || currentSpell.CastType == SpellCastType.Fast)
                                    && (currentSpell.IsUsable)
                                    && (caster.Ressource == null || currentSpell.RessourceCost <= caster.Ressource.CurrentAmount);
            }
            else
            {
                OnLockSpell?.Invoke();

                button.interactable = false;
            }
        }
        else
        {
            button.interactable = false;
        }
    }

    public void SetSpell(SpellScriptable toSet)
    {
        currentSpell = toSet;
        icon.sprite = toSet.Icon;

        spellName.text = toSet.Name;
        spellDescription.SetText(toSet.GetDescription());

        switch(toSet.CastType)
        {
            case SpellCastType.Normal:
                actionCostNormal.SetActive(true);
                actionCostFast.SetActive(false);
                break;
            case SpellCastType.Fast:
                actionCostFast.SetActive(true);
                actionCostNormal.SetActive(false);
                break;
        }

        if(toSet.RessourceCost != 0)
        {
            spellCost.text = toSet.RessourceCost.ToString();

            spellCost.gameObject.SetActive(true);

            if (spellChoiceUI.CurrentCharacter.GetComponentOfType(out CPN_SpellCaster caster))
            {
                if (caster.Ressource != null)
                {
                    ressourceHolders[(int)caster.Ressource.RessourceType].text.text = toSet.RessourceCost.ToString();

                    ressourceHolders[(int)caster.Ressource.RessourceType].gameObject.SetActive(true);
                }
            }
        }
        else
        {
            spellCost.gameObject.SetActive(false);

            foreach(RessourceHolder ress in ressourceHolders)
            {
                if (ress.gameObject != null)
                {
                    ress.gameObject.SetActive(false);
                }
            }
        }

        

        if (toSet.MaxUtilisation >= 0)
        {
            spellUtilisationLeft.text = toSet.UtilisationLeft.ToString();

            spellUtilisationLeftHolder.gameObject.SetActive(true);
        }
        else
        {
            spellUtilisationLeftHolder.gameObject.SetActive(false);
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

        actionCostNormal.SetActive(false);
        actionCostFast.SetActive(false);

        foreach (RessourceHolder ress in ressourceHolders)
        {
            if (ress.gameObject != null)
            {
                ress.gameObject.SetActive(false);
            }
        }

        currentSpell = null;

        gameObject.SetActive(false);
    }

    public void UE_SelectSpell()
    {
        RVN_CombatInputController.SelectSpell(spellIndex);
    }

    public void SetSelectSpell()
    {
        OnSelectSpell?.Invoke();
    }

    public void SetUnselectSpell()
    {
        OnUnselectSpell?.Invoke();
    }

    private void SetLock(bool toSet)
    {
        spellChoiceUI.CheckSpellUsability(this);
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
