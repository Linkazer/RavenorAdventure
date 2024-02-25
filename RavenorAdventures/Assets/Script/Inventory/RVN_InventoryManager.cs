using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RVN_InventoryManager : RVN_Singleton<RVN_InventoryManager>
{
    private Dictionary<StorableItem, int> objectsQuantityInInventory = new Dictionary<StorableItem, int>();

    public Action actOnUpdateInventory;

    public void AddItem(StorableItem item, int quantityToAdd)
    {
        if(!objectsQuantityInInventory.ContainsKey(item))
        {
            objectsQuantityInInventory.Add(item, 0);
        }

        objectsQuantityInInventory[item] += quantityToAdd;

        if (item.MaxStacks > 0 && objectsQuantityInInventory[item] > item.MaxStacks)
        {
            objectsQuantityInInventory[item] = item.MaxStacks;
        }

        actOnUpdateInventory?.Invoke();
    }

    public void RemoveItem(StorableItem item, int quantityToRemove = 1)
    {
        if (objectsQuantityInInventory.ContainsKey(item))
        {
            objectsQuantityInInventory[item] -= quantityToRemove;

            if(objectsQuantityInInventory[item] <= 0)
            {
                objectsQuantityInInventory.Remove(item);
            }

            actOnUpdateInventory?.Invoke();
        }
    }

    public StorableItem GetItem(int itemIndex)
    {
        if (objectsQuantityInInventory.Count > itemIndex)
        {
            return objectsQuantityInInventory.ElementAt(itemIndex).Key;
        }
        return null;
    }

    public int GetItemQuantity(StorableItem item)
    {
        return objectsQuantityInInventory[item];
    }

    public SPL_SpellScriptable GetItemAction(int itemIndex)
    {
        return GetItemAction(GetItem(itemIndex));
    }

    public SPL_SpellScriptable GetItemAction(StorableItem item)
    {
        if (item is StorableItem_Usable)
        {
            return (item as StorableItem_Usable).ObjectAction;
        }

        return null;
    }

    public void UseItem(int itemIndex)
    {
        UseItem(GetItem(itemIndex));
    }

    public void UseItem(StorableItem item)
    {
        if(item is StorableItem_Usable)
        {
            RemoveItem(item, (item as StorableItem_Usable).QuantityLostOnUse);
        }
    }

    public bool HasItem(StorableItem item)
    {
        return objectsQuantityInInventory.ContainsKey(item);
    }
}
