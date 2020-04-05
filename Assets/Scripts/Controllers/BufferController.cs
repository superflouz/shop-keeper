using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferController : Controller
{
    private Ability ability;

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        ability = GetComponent<Ability>();
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if spell can be cast. Only cast if there is enemies near
        if (entity.State == EntityState.Idle && entity.CurrentMana >= ability.manaCost)
        {
            // Maths for overlap
            Vector2 a = entity.transform.position;
            Vector2 b = (Vector2)transform.position + Vector2.right * transform.localScale.x * ((float)entity.slotCount / 2f + 10 + 0.5f) + Vector2.up * 10;

            // Overlap to find ennemy
            Collider2D[] hits = Physics2D.OverlapAreaAll(a, b, 1 << LayerMask.NameToLayer("Entities"));
            foreach (Collider2D hit in hits)
            {
                Entity hitEntity = hit.GetComponent<Entity>();
                // Check the first enemy in front of the entity
                if (hitEntity.Party.faction != entity.Party.faction)
                {
                    // Trigger the ability on itself if an enemy is found
                    ability.PrepareAbility(entity);
                    break;
                }
            }
        }
    }
}
