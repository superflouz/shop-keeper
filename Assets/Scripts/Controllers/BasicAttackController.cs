using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttackController : Controller
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (entity.CurrentState == Entity.State.Idle)
        {
            Vector2 a = entity.transform.position;
            Vector2 b = (Vector2)transform.position + Vector2.right * transform.localScale.x * ((float)entity.slotSize / 2f + entity.range);
            Collider2D[] hits = Physics2D.OverlapAreaAll(a, b, LayerMask.NameToLayer("Entity"));
            foreach (Collider2D hit in hits)
            {
                Entity enemy = hit.GetComponent<Entity>();
                // Check the first enemy in front of the entity
                if (enemy.faction != entity.faction)
                {
                    entity.attack.PrepareAttack(enemy);
                }
            }
        }
    }
}
