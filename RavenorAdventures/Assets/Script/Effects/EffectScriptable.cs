using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Spell/Effect/Base Effect")]
public class EffectScriptable : ScriptableObject
{
    [Header("Affichage")]
    [SerializeField] private Sprite icon;
    //Voir les affichages

    [Header("Effect")]
    [SerializeField] private int duration;
    [SerializeField] private List<Effect> effects;

    public Sprite Icon => icon;

    public int Duration => duration;

    public List<Effect> GetEffects => effects;
}
