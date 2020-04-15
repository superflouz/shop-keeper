using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCurvedProjectile : CurvedProjectile
{
    public StatusEffect statusEffect;

    protected override void Hit(Entity entity)
    {
        if (entity.Party.faction != Faction)
        {
            // Make the recipient of the projectile take damage
            entity.ApplyDamage(Damage, damageType, Source);

            StatusEffect effect = Instantiate(statusEffect, entity.transform);
            effect.Source = Source;

            entity.AddStatusEffect(effect);

            // Destroy this projectile
            Destroy(gameObject);
        }
    }
}
