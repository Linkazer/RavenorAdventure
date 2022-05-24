using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Spell/Create Effect")]
public class EffectScriptable : ScriptableObject
{
    [Header("Affichage")]
    [SerializeField] private Sprite icon;
    //Voir les affichages

    [Header("Effect")]
    [SerializeField] private Effect effect;

    public Effect GetEffect => effect; //CODE REVIEW : Voir pour donner une copie de l'effet ?
}
