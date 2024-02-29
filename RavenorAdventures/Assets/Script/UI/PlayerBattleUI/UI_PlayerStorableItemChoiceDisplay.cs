using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_PlayerStorableItemChoiceDisplay : MonoBehaviour
{
    [SerializeField] private List<UI_PlayerStorableItem> itemIcons;

    [SerializeField] private Animator animator;

    private bool isBagOpen = false;

    private RVN_ComponentHandler currentCharacter;

    public RVN_ComponentHandler CurrentCharacter => currentCharacter;

    public void OpenCloseBag()
    {
        isBagOpen = !isBagOpen;

        if (isBagOpen)
        {
            animator.SetBool("IsOpen", true);
        }
        else
        {
            animator.SetBool("IsOpen", false);
        }
    }

    public void UE_SetCharacter(RVN_ComponentHandler nCharacter) //Appelé par l'Event du SetCharacter
    {
        if (nCharacter != currentCharacter)
        {
            UnsetCharacter();

            if (nCharacter.GetComponentOfType<CPN_InventoryHandler>(out CPN_InventoryHandler inventoryHandler))
            {
                currentCharacter = nCharacter;

                inventoryHandler.Caster.actOnSetActionLeft += CheckItemsUsabilities;
                inventoryHandler.actOnSelectItem += OnSelectItem;
                inventoryHandler.actOnUnselectItem += OnUnselectItem;

                SetItems();
                RVN_InventoryManager.Instance.actOnUpdateInventory += SetItems;
            }
        }
    }

    private void SetItems()
    {
        for (int i = 0; i < itemIcons.Count; i++)
        {
            if (RVN_InventoryManager.Instance.GetItem(i) != null)
            {
                itemIcons[i].SetItem(RVN_InventoryManager.Instance.GetItem(i));
            }
            else
            {
                itemIcons[i].UnsetSpell();
            }
        }

        CheckItemsUsabilities(0,0);
    }

    public void OnSelectItem(StorableItem selectedItem)
    {
        foreach (UI_PlayerStorableItem itm in itemIcons)
        {
            if (itm.Item == selectedItem)
            {
                itm.SetSelectItem();
            }
        }
    }

    public void OnUnselectItem(StorableItem selectedItem)
    {
        foreach (UI_PlayerStorableItem itm in itemIcons)
        {
            itm.SetUnselectItem();
        }
    }

    public void UnsetCharacter()
    {
        if (currentCharacter != null)
        {
            if (currentCharacter.GetComponentOfType<CPN_InventoryHandler>(out CPN_InventoryHandler inventoryHandler))
            {
                inventoryHandler.Caster.actOnSetActionLeft -= CheckItemsUsabilities;
                inventoryHandler.actOnSelectItem -= OnSelectItem;
                inventoryHandler.actOnUnselectItem -= OnUnselectItem;


                RVN_InventoryManager.Instance.actOnUpdateInventory -= SetItems;
            }

            foreach (UI_PlayerStorableItem itm in itemIcons)
            {
                itm.SetUnselectItem();
            }
        }
    }

    private void CheckItemsUsabilities(int maxAction, int actionLeft)
    {
        foreach (UI_PlayerStorableItem itm in itemIcons)
        {
            if (itm.Item != null)
            {
                CheckItemUsability(itm);
            }
        }
    }


    public void CheckItemUsability(UI_PlayerStorableItem uI_PlayerItem)
    {
        uI_PlayerItem.CheckUsable(currentCharacter);
    }
}
