using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyToss : Ability
{
    public RotatingProjectile[] projectileList = new RotatingProjectile[4];

    protected override bool CastAbility(Entity target)
    {
        if (target == null) {
            return false;
        }

        int[] projectileCount = new int[4];
        projectileCount[0] = Random.Range(16, 25);
        projectileCount[1] = Random.Range(0, 4);
        projectileCount[2] = Random.Range(0, 4);
        projectileCount[3] = Random.Range(0, 2);

        for (int i = 0; i < projectileCount.Length; i++) 
        {
            for (int c = 0; c < projectileCount[i]; c++)
            {
                RotatingProjectile projectile;
                projectile = Instantiate(projectileList[i], transform.position + Vector3.up * 0.5f, Quaternion.identity);

                projectile.Origin = transform.position + Vector3.up * 2f;
                projectile.Destination = target.Party.transform.position + Vector3.right * transform.localScale.x * Random.Range(1f, 3f); 


                Vector2 middlePoint;
                middlePoint.x = (projectile.Origin.x + projectile.Destination.x) / 2;
                middlePoint.y = projectile.Origin.y + Mathf.Max(1f, Mathf.Abs(projectile.Origin.x - projectile.Destination.x) + Random.Range(-1f, 1f));
                projectile.MiddlePoint = middlePoint;

                projectile.ProjectileSpeed = 3;
                projectile.RotationAngle = Random.Range(-90f, 90f);

                projectile.Source = source;
            }
        }

        return true;
    }
}
