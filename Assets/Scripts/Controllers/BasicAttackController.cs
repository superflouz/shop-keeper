using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackController : Controller
{
    public float range;
    protected Attack attack;

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        attack = GetComponent<Attack>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if can attack
        if (entity.State == EntityState.Idle)
        {
            // Maths for overlap
            Vector2 a = entity.transform.position;
            Vector2 b = (Vector2)transform.position + Vector2.right * transform.localScale.x * ((float)entity.slotCount / 2f + range + 0.5f) + Vector2.up * range;

            Entity target = null;

            // Overlap to find ennemy
            Collider2D[] hits = Physics2D.OverlapAreaAll(a, b, 1 << LayerMask.NameToLayer("Entities"));
            foreach (Collider2D hit in hits)
            {
                Entity hitEntity = hit.GetComponent<Entity>();
                // Check the first enemy in front of the entity
                if (hitEntity.Party.faction != entity.Party.faction)
                {
                    // Check if the new one is closer
                    if (target == null)
                        target = hitEntity;
                    else if (Vector3.Distance(entity.transform.position, hitEntity.transform.position) < Vector3.Distance(entity.transform.position, target.transform.position))
                        target = hitEntity;
                }
            }

            // Trigger attack if a target is found
            if (target != null)
                attack.PrepareAttack(target);
        }
    }
}
