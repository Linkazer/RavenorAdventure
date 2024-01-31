using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQA_AddItemToInventory : SequenceAction
{
    [Serializable]
    private struct ItemToAdd
    {
        public StorableItem item;
        public int amount;
    }

    [SerializeField] private ItemToAdd[] storableItemsToAdd;

    protected override void OnStartAction()
    {
        foreach(ItemToAdd item in storableItemsToAdd)
        {
            RVN_InventoryManager.Instance.AddItem(item.item, item.amount);
        }

        EndAction();
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
        foreach (ItemToAdd item in storableItemsToAdd)
        {
            RVN_InventoryManager.Instance.AddItem(item.item, item.amount);
        }
    }
}
