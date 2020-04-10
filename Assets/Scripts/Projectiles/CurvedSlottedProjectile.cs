using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedSlottedProjectile : CurvedProjectile
{
    public float FallSpeed { get; set; }
    public float Delay { get; set; }

    new void Start()
    {
        base.Start();

        // Calculate the middle point
        Vector2 middlePoint;
        middlePoint.x = (Origin.x + Destination.x) / 2;
        middlePoint.y = Origin.y + 7f;
        MiddlePoint = middlePoint;

        Destination +=  Vector2.up * 6;
    }

    // Update is called once per frame
    new void Update()
    {
        if (t < 1)
        {
            base.Update();
        }
        else if (Delay > 0)
        {
            Delay -= Time.deltaTime;
        }
        else
        {
            // Overlap detection
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.05f, 1 << LayerMask.NameToLayer("Entities"));
            foreach (Collider2D hit in hits)
            {
                Entity entity = hit.GetComponent<Entity>();

                // Something got hit
                Hit(entity);
            }

            transform.position = transform.position + (Vector3.down * FallSpeed * 0.5f * Time.deltaTime);

            if (transform.position.y < -2)
            {
                Destroy(gameObject);
            }
        }
    }
}
