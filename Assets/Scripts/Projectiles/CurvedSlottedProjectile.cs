using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedSlottedProjectile : CurvedProjectile
{
    // Update is called once per frame
    new void Update()
    {
        if (t < 1)
        {
            base.Update();
        }
        else
        {
            // Overlap detection
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.01f, 1 << LayerMask.NameToLayer("Entities"));
            foreach (Collider2D hit in hits)
            {
                Entity entity = hit.GetComponent<Entity>();

                // Something got hit
                Hit(entity);
            }

            transform.position = transform.position + (Vector3.down * ProjectileSpeed * 0.5f * Time.deltaTime);
        }
    }
}
