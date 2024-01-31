using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CPN_InventoryHandler : CPN_CharacterAction
{
    [SerializeField] private CPN_SpellCaster spellCaster;

    private StorableItem selectedItem = null;
    private SpellScriptable selectedItemAction = null;

    public CPN_SpellCaster Caster => spellCaster;

    public Action<StorableItem> actOnSelectItem;
    public Action<StorableItem> actOnUnselectItem;

    public override void ResetData()
    {

    }

    public override void SetComponent(RVN_ComponentHandler handler)
    {

    }

    public override void Activate()
    {
        
    }

    public override void Disactivate()
    {

    }

    public override bool CanSelectAction()
    {
        return selectedItem != null && selectedItem is StorableItem_Usable;
    }

    public bool CanItemBeUsed(int itemIndex)
    {
        return CanItemBeUsed(RVN_InventoryManager.Instance.GetItem(itemIndex));
    }

    public bool CanItemBeUsed(StorableItem itemToCheck)
    {
        return itemToCheck is StorableItem_Usable
            && (spellCaster.ActionLeftThisTurn > 0 || selectedItemAction.CastType == SpellCastType.Fast);
    }

    internal bool SelectItem(int itemIndex)
    {
        selectedItem = RVN_InventoryManager.Instance.GetItem(itemIndex);

        if(selectedItem != null)
        {
            selectedItemAction = RVN_InventoryManager.Instance.GetItemAction(selectedItem);
            spellCaster.SelectSpell(selectedItemAction);

            return true;
        }
        else
        {
            selectedItemAction = null;
            spellCaster.UnselectSpell();
        }

        return false;
    }

    public override void DisplayAction(Vector2 actionTargetPosition)
    {
        spellCaster.DisplayAction(actionTargetPosition);
    }

    public override bool IsActionUsable(Vector2 actionTargetPosition)
    {
        return spellCaster.IsActionUsable(actionTargetPosition);
    }

    public override bool TryDoAction(Vector2 actionTargetPosition, Action callback)
    {
        if(spellCaster.TryDoAction(actionTargetPosition, callback))
        {
            RVN_InventoryManager.Instance.UseItem(selectedItem);

            return true;
        }

        return false;
    }

    public override void UndisplayAction(Vector2 actionTargetPosition)
    {
        spellCaster.UndisplayAction(actionTargetPosition);
    }

    public override void UnselectAction()
    {
        spellCaster.UnselectSpell();
    }
}
