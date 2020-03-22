using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveProjectile : MonoBehaviour
{
    protected float t;
    public Vector2 Origin { get; set; }
    public Vector2 Destination { get; set; }
    public Vector2 MiddlePoint { get; set; }

    public Entity Source { get; set; }

    private float ratioSpeed;
    public float ProjectileSpeed;

    // Start is called before the first frame update
    protected void Start()
    {
        t = 0;
        // The projectile will have the same speed regardless of the distance
        ratioSpeed = 1 / (Mathf.Abs(Origin.x - Destination.x) + 1.5f * Mathf.Abs(Origin.y - MiddlePoint.y));

    }

    // Update is called once per frame
    protected void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.01f, 1 << LayerMask.NameToLayer("Entities"));
        foreach (Collider2D hit in hits) {
            Entity entity = hit.GetComponent<Entity>();
            if (entity.faction != Source.faction) {
                entity.ApplyDamage(Source.attackDamage, DamageType.Physical, Source);

                Destroy(gameObject);
            }
        }

        if (t < 2)
            t += ProjectileSpeed * ratioSpeed * Time.deltaTime;
        else { Destroy(gameObject); }
        transform.position = CalculateQuadraticBezierCurve(t, Origin, MiddlePoint, Destination);
    }
    private Vector2 CalculateQuadraticBezierCurve(float t, Vector2 P0, Vector2 P1, Vector2 P2)
    {
        // Draw points


        //P0 is origin
        //P1 is the top angle of the base triangle
        //P2 is target

        // B(t) = ((1-t)^2 * P0) + (2(1-t) * t * P1) + (t^2 * P2)
        //      =    (u ^2 * P0) +  (2 * u * t * P1) + (t^2 * P2)
        //      =      (uu * P0) +  (2 * u * t * P1) +  (tt * P2)

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector2 point = (uu * P0) + (2 * u * t * P1) + (tt * P2);

        return point;
    }

    private void OnDrawGizmos()
    {
        /*Gizmos.DrawWireSphere(origin, 0.1f);
        Gizmos.DrawWireSphere(middlePoint, 0.1f);
        Gizmos.DrawWireSphere(destination, 0.1f);*/
    }
}
