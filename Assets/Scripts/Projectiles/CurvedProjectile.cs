using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedProjectile : MonoBehaviour
{
    protected float t;
    public Entity Source { get; set; }
    public Vector2 Origin { get; set; }
    public Vector2 MiddlePoint { get; set; }
    private Vector2 destination;
    public Vector2 Destination
    { 
        get
        {
            return destination;
        }
        set
        {
            // Minimal Range of the projectile
            if (Mathf.Abs(value.x - Origin.x) < 1)
            {
                if (value.x - Origin.x < 0)
                {
                    value.x = Origin.x - 1;
                }
                else
                {
                    value.x = Origin.x + 1;
                }
            }
            destination = value;
        }
    }

    private float ratioSpeed;
    public float ProjectileSpeed { get; set; }

    // Start is called before the first frame update
    protected void Start()
    {
        // Set t (time) default value
        t = 0;
        // The projectile will have the same speed regardless of the distance
        ratioSpeed = 1 / (Mathf.Abs(Origin.x - Destination.x) + 1.5f * Mathf.Abs(Origin.y - MiddlePoint.y));

        // Calculate the middle point
        Vector2 middlePoint;
        middlePoint.x = (Origin.x + Destination.x) / 2;
        middlePoint.y = Origin.y + Mathf.Abs(Origin.x - Destination.x) + Random.Range(0, 0.4f);
        MiddlePoint = middlePoint;
    }

    // Update is called once per frame
    protected void Update()
    {
        // Overlap detection
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.05f, 1 << LayerMask.NameToLayer("Entities"));
        foreach (Collider2D hit in hits)
        {
            Entity entity = hit.GetComponent<Entity>();
            
            // Something got hit
            Hit(entity);
        }

        // Move projectile with time and speed
        if (t < 2) { t += ProjectileSpeed * ratioSpeed * Time.deltaTime; }
        else { Destroy(gameObject); }

        transform.position = CalculateQuadraticBezierCurve(t, Origin, MiddlePoint, Destination);
    }

    /// <summary>
    /// Overridable function called when an entity is hit by a projectile
    /// </summary>
    /// <param name="entity">Entity hit</param>
    virtual protected void Hit(Entity entity)
    {
        if (entity.faction != Source.faction) {
            // Make the recipient of the projectile take damage
            entity.ApplyDamage(Source.attackDamage, DamageType.Physical, Source);

            // Destroy this projectile
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Calculate the point in the curve (P1) from origin (P0) to destination (P2)
    /// </summary>
    /// <param name="t">from 0 to 1, point in the curve</param>
    /// <param name="P0">Origin</param>
    /// <param name="P1">Middle Point (Curve height)</param>
    /// <param name="P2">Destination</param>
    /// <returns>Point in the curve for (t)</returns>
    private Vector2 CalculateQuadraticBezierCurve(float t, Vector2 P0, Vector2 P1, Vector2 P2)
    {
        //P0 is origin
        //P1 is the top angle of the base triangle
        //P2 is target

        // B(t) = ((1-t)^2 * P0) + (2(1-t) * t * P1) + (t^2 * P2)
        //      =    (u ^2 * P0) +  (2 * u * t * P1) + (t^2 * P2)
        //      =      (uu * P0) +  (2 * u * t * P1) +  (tt * P2)

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        // Simplified equation
        Vector2 point = (uu * P0) + (2 * u * t * P1) + (tt * P2);

        return point;
    }

    private void OnDrawGizmos()
    {
        /*
        Gizmos.DrawWireSphere(origin, 0.1f);
        Gizmos.DrawWireSphere(middlePoint, 0.1f);
        Gizmos.DrawWireSphere(destination, 0.1f);
        */
    }
}
