using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinKingController : Controller
{
    protected Ability ability;

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        ability = GetComponent<Ability>();
    }

    // Update is called once per frame
    void Update()
    {
        float spellRange = 5;

        // Checks if spell can be cast
        if (entity.CurrentState == Entity.State.Idle && entity.CurrentMana >= ability.manaCost)
        {
            // Maths for overlap
            Vector2 a = entity.transform.position;
            Vector2 b = (Vector2)transform.position + Vector2.right * transform.localScale.x * ((float)entity.slotSize / 2f + spellRange + 0.5f) + Vector2.up * entity.range;

            // Overlap to find ennemy
            Collider2D[] hits = Physics2D.OverlapAreaAll(a, b, 1 << LayerMask.NameToLayer("Entities"));
            foreach (Collider2D hit in hits)
            {
                Entity enemy = hit.GetComponent<Entity>();
                // Check the first enemy in front of the entity
                if (enemy.faction != entity.faction)
                {
                    ability.PrepareAbility(enemy);
                }
            }
        }

        // Check if can attack
        else if (entity.CurrentState == Entity.State.Idle)
        {
            // Maths for overlap
            Vector2 a = entity.transform.position;
            Vector2 b = (Vector2)transform.position + Vector2.right * transform.localScale.x * ((float)entity.slotSize / 2f + entity.range + 0.5f) + Vector2.up * entity.range;

            // Overlap to find ennemy
            Collider2D[] hits = Physics2D.OverlapAreaAll(a, b, 1 << LayerMask.NameToLayer("Entities"));
            foreach (Collider2D hit in hits)
            {
                Entity enemy = hit.GetComponent<Entity>();
                // Check the first enemy in front of the entity
                if (enemy.faction != entity.faction)
                {
                    attack.PrepareAttack(enemy);
                }
            }
        }
    }
}
