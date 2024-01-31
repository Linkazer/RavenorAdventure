using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Usable Storable Item", menuName = "Inventory/Usable Item")]
public class StorableItem_Usable : StorableItem
{
    [SerializeField] private SpellScriptable objectAction;

    [SerializeField] private int quantityLostOnUse = 1;

    public int QuantityLostOnUse => quantityLostOnUse;
    public SpellScriptable ObjectAction => objectAction;

    public override string GetDescription()
    {
        return objectAction.GetDescription() + "\n\n" + base.GetDescription();
    }
}
