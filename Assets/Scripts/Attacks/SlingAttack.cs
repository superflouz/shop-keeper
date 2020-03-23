using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingAttack : Attack
{
    public RotatingProjectile scrap;
    public float projectileSpeed;
    public int projectileCount;

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

    protected override bool ExecuteAttack(Entity target)
    {
        if (target == null) {
            return false;
        }

        for (int i = 0; i < projectileCount; i++) {
            RotatingProjectile projectile;
            projectile = Instantiate(scrap, transform.position + Vector3.up * 0.5f, Quaternion.identity);

            projectile.Origin = transform.position + Vector3.up * 1f;
            projectile.Destination = target.transform.position + Vector3.up * 0.5f + Vector3.right * Random.Range(-1f, 1f);


            Vector2 middlePoint;
            middlePoint.x = (projectile.Origin.x + projectile.Destination.x) / 2;
            middlePoint.y = projectile.Origin.y + Mathf.Max(1f, Mathf.Abs(projectile.Origin.x - projectile.Destination.x) + Random.Range(-2f, -1f));
            projectile.MiddlePoint = middlePoint;

            projectile.ProjectileSpeed = projectileSpeed;
            projectile.RotationAngle = Random.Range(-90f, 90f);

            projectile.Source = user;
        }

        return true;
    }

}
