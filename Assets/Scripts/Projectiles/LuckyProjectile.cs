using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyProjectile : CurvedSlottedProjectile
{
    public int damage;
    public int goldValue;

    protected override void Hit(Entity entity)
    {
        if (entity.faction != Source.faction)
        {
            // Make the recipient of the projectile take damage
            int amount = Mathf.RoundToInt(damage * (1 + (Source.abilityPower / 100f)));
            entity.ApplyDamage(amount, DamageType.Magical, Source);

            // Destroy this projectile
            Destroy(gameObject);
        }
    }
}
