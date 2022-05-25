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
    [SerializeField] private List<Effect> effects;

    public Effect GetEffect => effects[0]; //CODE REVIEW : Voir pour donner une copie de l'effet ?

    public List<Effect> GetEffects => effects;
}
