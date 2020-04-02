using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyProjectile : CurvedSlottedProjectile
{
    public int GoldValue;

    protected override void Hit(Entity entity)
    {
        if (entity.Party.faction != Faction)
        {
            // Make the recipient of the projectile take damage
            entity.ApplyDamage(Damage, DamageType.Physical, Source);

            // Give gold

            // Destroy this projectile
            Destroy(gameObject);
        }
    }
}
