using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float t;
    private float height;
    private Vector2 origin;
    private Vector2 destination;
    private Vector2 middlePoint;

    private Entity target;
    public Entity Target { get { return target; } set { target = value; } }

    // Start is called before the first frame update
    void Start()
    {
        t = 0;
        height = 5;

        origin = transform.position;
        destination = target.transform.position;
        middlePoint.x = (origin.x + destination.x) / 2;
        middlePoint.y = origin.y + height;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.01f, 1 << LayerMask.NameToLayer("Entities"));
        foreach (Collider2D hit in hits)
        {
            Entity entity = hit.GetComponent<Entity>();
            if(entity.faction == Faction.Enemy)
            {
                //enemy.ApplyDamage(5, DamageType.Physical, sourceManquante);
                Debug.Log("arrow hit");
                Destroy(gameObject);
            }    
        }

        if(t < 1) t += Time.deltaTime;
        else { Destroy(gameObject); }

        transform.position = CalculateQuadraticBezierCurve(t, origin, middlePoint, destination);
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
        Gizmos.DrawWireSphere(origin, 0.1f);
        Gizmos.DrawWireSphere(middlePoint, 0.1f);
        Gizmos.DrawWireSphere(destination, 0.1f);
    }
}
