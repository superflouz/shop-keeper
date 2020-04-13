using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLizardController : Controller
{
    public float range;
    protected Attack attack;
    protected Ability ability;

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        attack = GetComponent<Attack>();
        ability = GetComponent<Ability>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if can attack
        if (entity.State == EntityState.Idle)
        {
            Vector2 positionTarget = (Vector2)entity.transform.position;
            positionTarget.y = 0;
            positionTarget += Vector2.right * entity.transform.localScale.x * (float)range;

            // Maths for overlap
            Vector2 a = positionTarget + Vector2.left * 0.2f;
            Vector2 b = positionTarget + Vector2.right * 0.2f + Vector2.up * (0.2f + range - 1 + entity.transform.position.y);

            Entity target = null;

            // Overlap to find ennemy
            Collider2D[] hits = Physics2D.OverlapAreaAll(a, b, 1 << LayerMask.NameToLayer("Entities"));
            foreach (Collider2D hit in hits)
            {
                Entity hitEntity = hit.GetComponent<Entity>();
                // Check the first enemy in front of the entity
                if (hitEntity.Party.faction != entity.Party.faction)
                {
                    target = hitEntity;
                    break;
                }
            }

            // Trigger attack if a target is found
            if (target != null)
            {
                // Use ability is possible; else, use attack
                if (!ability.PrepareAbility(transform.position + Vector3.right * transform.localScale.x * (float)entity.slotCount / 2))
                {
                    if (!ability.IsCasting)
                        attack.PrepareAttack(target);
                }
            }

        }
    }
}
