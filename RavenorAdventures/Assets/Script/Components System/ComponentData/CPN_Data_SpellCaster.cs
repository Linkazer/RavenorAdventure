using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CPN_Data_SpellCaster
{
    /// <summary>
    /// Sorts disponibles.
    /// </summary>
    /// <returns></returns>
    public List<SpellScriptable> AvailableSpells();
    public SpellScriptable OpportunitySpell();
    /// <summary>
    /// Nombre de sort utilisable par tour.
    /// </summary>
    /// <returns></returns>
    public int MaxSpellUseByTurn();
    /// <summary>
    /// Nombre de relance de d� disponible.
    /// </summary>
    /// <returns></returns>
    public int OffensiveRerolls();
    /// <summary>
    /// Stat de Pr�cision.
    /// </summary>
    /// <returns></returns>
    public int Accuracy();
    /// <summary>
    /// Stat de Puissance.
    /// </summary>
    /// <returns></returns>
    public int Power();

    public SpellRessource Ressource();
}
