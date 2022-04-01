using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpellData
{
    //CODE REVIEW : Mettre les affichages dans le Scriptable ? (Voir comment on peut les r�cup�rer pendant l'utilisation du sort, et si c'est n�cessaire)
    [SerializeField] private string name;
    [SerializeField] private Sprite icon;
    [SerializeField] private string description;

    public string Name => name;

    public Sprite Icon => icon;

    public string Description => description;
}
