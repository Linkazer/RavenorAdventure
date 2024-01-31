using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_PlayerStorableItem : MonoBehaviour
{
    [SerializeField] private UI_PlayerStorableItemChoiceDisplay playerInventoryChoiceUI;
    [SerializeField] private int itemIndex;

    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private Image icon;

    [Header("Description")]
    [SerializeField] private RectTransform descriptionHandler;
    [SerializeField] private TextMeshProUGUI spellName;
    [SerializeField] private TextMeshProUGUI spellDescription;

    [Header("Utilisation")]
    [SerializeField] private GameObject spellUtilisationLeftHolder;
    [SerializeField] private TextMeshProUGUI utilisationLeftText;

    [Header("Cast Time")]
    [SerializeField] private GameObject actionCostNormal;
    [SerializeField] private GameObject actionCostFast;

    [Header("Events")]
    [SerializeField] private UnityEvent OnSelectItem;
    [SerializeField] private UnityEvent OnUnselectItem;

    [SerializeField] private StorableItem currentItem;

    public StorableItem Item => currentItem;

    public void CheckUsable(RVN_ComponentHandler characterHandler)
    {
        if (characterHandler.GetComponentOfType(out CPN_InventoryHandler inventoryHandler))
        {
            button.interactable = inventoryHandler.CanItemBeUsed(itemIndex);
        }
        else
        {
            button.interactable = false;
        }
    }

    public void SetItem(StorableItem toSet)
    {
        if (currentItem != toSet)
        {
            currentItem = toSet;
            icon.sprite = toSet.Icon;

            spellName.text = toSet.Name;
            spellDescription.SetText(toSet.GetDescription());

            if (toSet is StorableItem_Usable)
            {
                switch ((toSet as StorableItem_Usable).ObjectAction.CastType)
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
            }
            else
            {
                actionCostFast.SetActive(false);
                actionCostNormal.SetActive(false);
            }

            gameObject.SetActive(true);
        }

        if (toSet is StorableItem_Usable)
        {
            utilisationLeftText.text = RVN_InventoryManager.Instance.GetItemQuantity(currentItem).ToString();
            spellUtilisationLeftHolder.SetActive(true);
        }
        else
        {
            spellUtilisationLeftHolder.SetActive(false);
        }
    }

    public void UnsetSpell()
    {
        currentItem = null;

        actionCostFast.SetActive(false);
        actionCostNormal.SetActive(false);

        gameObject.SetActive(false);
    }

    public void UE_SelectItem()
    {
        RVN_CombatInputController.SelectItem(itemIndex);
    }

    public void SetSelectItem()
    {
        OnSelectItem?.Invoke();
    }

    public void SetUnselectItem()
    {
        OnUnselectItem?.Invoke();
    }

    public void UpdateUtilisationLeft()
    {
        utilisationLeftText.text = RVN_InventoryManager.Instance.GetItemQuantity(currentItem).ToString();
    }

    public void DisplayItemDescription(bool show)
    {
        descriptionHandler.gameObject.SetActive(show);

        LayoutRebuilder.ForceRebuildLayoutImmediate(descriptionHandler);
    }
}
