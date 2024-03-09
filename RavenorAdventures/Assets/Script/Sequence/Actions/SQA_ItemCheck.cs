using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SQA_ItemCheck : SequenceAction
{
    [Serializable]
    private class ItemCheck
    {
        public StorableItem[] itemsToCheck;

        public SequenceAction actionOnItemFound;

        public bool PlayerHasAllItem()
        {
            bool toReturn = true;

            foreach (StorableItem item in itemsToCheck)
            {
                if (!RVN_InventoryManager.Instance.HasItem(item))
                {
                    toReturn = false;
                    break;
                }
            }

            return toReturn;
        }
    }

    [SerializeField] private ItemCheck[] checksToDo;
    [SerializeField] private SequenceAction actionToDoOnNoValidCheck;

    protected override void OnStartAction()
    {
        SequenceAction actionToPlay = actionToDoOnNoValidCheck;

        foreach (ItemCheck check in checksToDo)
        {
            if (check.PlayerHasAllItem())
            {
                actionToPlay = check.actionOnItemFound;
                break;
            }
        }

        if (actionToPlay != null)
        {
            actionToPlay.StartAction(EndAction);
        }
        else
        {
            EndAction();
        }
    }

    protected override void OnEndAction()
    {
        
    }

    protected override void OnSkipAction()
    {
        
    }

}
