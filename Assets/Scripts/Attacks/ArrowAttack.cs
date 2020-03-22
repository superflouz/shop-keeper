using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : Attack
{
    public ArrowProjectile arrow;
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

    protected override void ExecuteAttack(Entity target)
    {
        if (target == null) {
            return;
        }

        ArrowProjectile projectile; 
        projectile = Instantiate(arrow, transform.position + Vector3.up * 1f, Quaternion.identity);

        projectile.Origin = transform.position + Vector3.up * 1f;
        projectile.Destination = target.transform.position + Vector3.up * 0.5f + Vector3.right * Random.Range(-0.2f, 0.2f);

        Vector2 middlePoint;
        middlePoint.x = (projectile.Origin.x + projectile.Destination.x) / 2;
        middlePoint.y = projectile.Origin.y + Mathf.Abs(projectile.Origin.x - projectile.Destination.x) + Random.Range(0, 0.4f);
        projectile.MiddlePoint = middlePoint;

        projectile.ProjectileSpeed = projectileSpeed;

        projectile.Source = user;
    }
}
