using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreathProjectile : Projectile
{
    public float delayDamages;
    public StatusEffect burnEffect;

    // Update is called once per frame
    new void Update()
    {
        if (delayDamages > 0)
        {
            delayDamages -= Time.deltaTime;
            if (delayDamages <= 0)
            {
                Collider2D[] hits = Physics2D.OverlapAreaAll(transform.position, transform.position + Vector3.up * 1 + Vector3.right * transform.localScale.x * 2.5f, 1 << LayerMask.NameToLayer("Entities"));
                foreach (Collider2D hit in hits)
                {
                    Entity entity = hit.GetComponent<Entity>();

                    // Something got hit
                    if (entity != null)
                        Hit(entity);
                }
            }
        }
    }

    protected override void Hit(Entity entity)
    {
        if (entity.Party.faction != Faction)
        {
            // Make the recipient of the projectile take damage
            entity.ApplyDamage(Damage, damageType, Source);

            StatusEffect effect = Instantiate(burnEffect, entity.transform);
            effect.Source = entity;

            entity.AddStatusEffect(effect);

            // Destroy this projectile
            Destroy(gameObject);
        }
    }
}
