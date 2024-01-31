using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemChecker : MonoBehaviour
{
    [Serializable]
    private class ItemCheck
    {
        public StorableItem[] itemsToCheck;

        public UnityEvent onHaveItems;

        public bool PlayerHasAllItem()
        {
            bool toReturn = true;

            foreach (StorableItem item in itemsToCheck)
            {
                if(!RVN_InventoryManager.Instance.HasItem(item))
                {
                    toReturn = false;
                    break;
                }
            }

            return toReturn;
        }
    }

    [SerializeField] private ItemCheck[] itemChecksTodo;

    [SerializeField] private UnityEvent onHaveNoItem;

    public void UA_CheckItemPossession()
    {
        foreach(ItemCheck check in itemChecksTodo)
        {
            if(check.PlayerHasAllItem())
            {
                check.onHaveItems?.Invoke();
                return;
            }
        }

        onHaveNoItem?.Invoke();
    }
}
