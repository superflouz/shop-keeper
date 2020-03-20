using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : Controller
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (entity.CurrentState == Entity.State.Idle) {
            Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)transform.position + (Vector2.right * transform.localScale.x * entity.slotSize) / 2, 0.1f);
            foreach (Collider2D hit in hits) {
                Entity enemy = hit.GetComponent<Entity>();
                // Check the first enemy in front of the entity
                if (enemy.faction != entity.faction) {
                    entity.attack.PrepareAttack(enemy);
                }
            }
        }
    }
}
