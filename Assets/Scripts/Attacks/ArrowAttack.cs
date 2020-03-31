using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : Attack
{
    public CurvedProjectile arrow;
    public float projectileSpeed;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    
    /// <summary>
    /// Executes the attack
    /// </summary>
    /// <param name="target">Target to attack</param>
    protected override bool ExecuteAttack(Entity target)
    {
        if (target == null) {
            return false;
        }

        // Instantiate arrow
        CurvedProjectile projectile; 
        projectile = Instantiate(arrow, transform.position + Vector3.up * 1f, Quaternion.identity);

        // Calculate Origin and Destination
        projectile.Origin = transform.position + Vector3.up * 1f;
        projectile.Destination = target.transform.position + Vector3.up * 0.5f + Vector3.right * Random.Range(-0.2f, 0.2f);

        // Calculate middle point for height
        Vector2 middlePoint;
        middlePoint.x = (projectile.Origin.x + projectile.Destination.x) / 2;
        middlePoint.y = projectile.Origin.y + Mathf.Abs(projectile.Origin.x - projectile.Destination.x) + Random.Range(0, 0.4f);
        projectile.MiddlePoint = middlePoint;

        // Set Speed
        projectile.ProjectileSpeed = projectileSpeed;

        // Set user of the attack
        projectile.Source = user;

        return true;
    }
}
