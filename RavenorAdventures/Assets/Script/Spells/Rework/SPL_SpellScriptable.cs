using ravenor.referencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spell/Create Spell")]
public class SPL_SpellScriptable : ScriptableObject
{
    [Header("Général Informations")]
    [SerializeField] private RVN_Text displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private RVN_Text description;

    [Header("Ressources")]
    [SerializeField] private int ressourceCost;
    [SerializeField] private int maxUtilisations = -1;

    [Header("Cast Data")]
    [SerializeField] private int range;
    [SerializeField, SerializeReference, ReferenceEditor(typeof(SPL_SpellActionShape))] private SPL_SpellActionShape previsualizationZone = new SPL_Shape_TargetNode();
    [SerializeField] private SpellTargets hitableTarget = SpellTargets.All;
    [SerializeField] private SpellTargets castTarget = SpellTargets.All;
    [SerializeField] private SpellCastType castType = SpellCastType.Normal;

    [Header("Cooldowns")]
    //[SerializeField] private bool isCooldownGlobal = false;
    [SerializeField] private int cooldown;

    [Header("Action")]
    [SerializeField] private SPL_SpellAcionChooser spellAction;

    public string DisplayName => displayName.GetText();
    public string Description => description.GetText();
    public Sprite Icon => icon;

    public int RessourceCost => ressourceCost;
    public int MaxUtilisations => maxUtilisations;

    public int Cooldown => cooldown;

    public int Range => range;
    public SpellCastType CastType => castType;
    public SpellTargets CastTarget => castTarget;

    public SPL_SpellAcionChooser SpellAction => spellAction;

    public SPL_SpellAction GetSpellAction(SPL_CastedSpell castedSpellData)
    {
        return spellAction.GetFirstUsableAction(castedSpellData);
    }

    public List<Node> GetDisplayzone(Node casterNode, Node targetNode)
    {
        return previsualizationZone.GetZone(casterNode, targetNode);
    }
}
