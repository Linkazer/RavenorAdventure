using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPL_AB_ProjectileAction : SPL_SpellActionBehavior<SPL_AS_ProjectileAction>
{
    protected override void ResolveAction(SPL_AS_ProjectileAction actionToResolve, SPL_SpellResolver spellResolver)
    {
        //Possibilit� de faire un projectile par target dans la zone : Demande de revoir le Resolver
        // - Demande de revoir le Resolver
        // - Demande de pouvoir avoir un target sp�cifique
        // - Possibilit� d'avoir des Resolver dans le Resolver ?
        //   En gros, on cr�e un nouveau Resolver avec la nouvelle CastedData (Target modifi�) et on vient le renseigner dans le Resolver.
        //   Une fois le SubResolver fini, on dit au Resolver qu'un Sub a fini, et on fonctionne comme pour les anims
        //   (On attend que tous les Sub ait fini avant de finir)
        SpellProjectile projectile = Instantiate(actionToResolve.ProjectileToUse, transform);

        projectile.SetProjectile(spellResolver.CastedSpellData.Caster.CurrentNode, spellResolver.CastedSpellData.TargetNode, () => OnProjectileReachTarget(actionToResolve, spellResolver));
    }

    private void OnProjectileReachTarget(SPL_AS_ProjectileAction actionToResolve, SPL_SpellResolver spellResolver)
    {
        EndResolve(actionToResolve.GetActionOnReachTarget(spellResolver.CastedSpellData), spellResolver);
    }
}
