using ravenor.referencePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Spell/Effect/Base Effect")]
public class EffectScriptable : ScriptableObject
{
    [Header("Affichage")]
    [SerializeField] private bool hideOnApply;
    [SerializeField] private Sprite icon;

    [SerializeField] private RVN_Text effectName;
    [SerializeField] private RVN_Text effectDescription;
    //Voir les affichages

    [SerializeField] private GameObject effectDisplay;

    [Header("Effect")]
    [SerializeField] private float duration;
    [SerializeField, SerializeReference, ReferenceEditor(typeof(Effect))] private List<Effect> effects; //, [SerializeReference, ReferenceEditor(typeof(Effect))]

    public bool HideOnApply => hideOnApply;
    public Sprite Icon => icon;
    public GameObject EffectDisplay => effectDisplay;

    public float Duration => duration + 1;

    public List<Effect> GetEffects => effects;

    public string Name => effectName.GetText();
    public string Description => effectDescription.GetText();
}
