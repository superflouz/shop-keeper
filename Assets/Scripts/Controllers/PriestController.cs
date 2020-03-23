using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestController : Controller
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
        // Checks if spell can be cast
        if (entity.CurrentState == Entity.State.Idle && entity.CurrentMana >= ability.manaCost)
        {
            // Maths for the position of the overlap
            Vector2 a = entity.transform.position;
            Vector2 b = (Vector2)transform.position + Vector2.right * transform.localScale.x * ((float)entity.slotSize / 2f + 0.5f);

            // Overlap to check hits
            Collider2D[] hits = Physics2D.OverlapAreaAll(a, b, 1 << LayerMask.NameToLayer("Entities"));
            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject == gameObject)
                {
                    continue;
                }

                Entity ally = hit.GetComponent<Entity>();

                // Check the first ally in front of the entity
                if (ally.faction == entity.faction && ally.CurrentHealth < ally.health)
                {
                    ability.PrepareAbility(ally);
                    break;
                }
            }
        }
    }
}
