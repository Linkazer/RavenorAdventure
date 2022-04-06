using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpellData
{
    //CODE REVIEW : Mettre les affichages dans le Scriptable ? (Voir comment on peut les récupérer pendant l'utilisation du sort, et si c'est nécessaire)
    [SerializeField] protected string name;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected string description;

    [SerializeField] protected int range;

    public string Name => name;
    public Sprite Icon => icon;
    public string Description => description;

    public int Range => range;

    /// <summary>
    /// Créer une copie du SpellData
    /// </summary>
    /// <returns>La copie du SpellData.</returns>
    public virtual SpellData GetCopy()
    {
        SpellData toReturn = new SpellData();

        toReturn.name = name;
        toReturn.icon = icon;
        toReturn.description = description;

        return toReturn;
    }
}
