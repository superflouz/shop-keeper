using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyToss : Ability
{
    public CurvedSlottedProjectile[] projectileList = new CurvedSlottedProjectile[4];

    protected override bool CastAbility(Entity target)
    {
        if (target == null) {
            return false;
        }

        int random = Random.Range(1, 4);

        for (int c = 0; c < 20; c++)
        {
            CurvedSlottedProjectile projectile;
            projectile = Instantiate(projectileList[0], transform.position + Vector3.up * 0.5f, Quaternion.identity);

            projectile.Origin = transform.position + Vector3.right * transform.localScale.x * source.slotSize / 4 + Vector3.up * 2f;
            projectile.Destination = target.Party.transform.position + 
                Vector3.right * transform.localScale.x * (0.5f + Random.Range(0, 5) + Random.Range(-0.5f, 0.5f)) +
                Vector3.up * 4; 

            Vector2 middlePoint;
            middlePoint.x = (projectile.Origin.x + projectile.Destination.x) / 2;
            middlePoint.y = projectile.Origin.y + 5f;
            projectile.MiddlePoint = middlePoint;

            projectile.ProjectileSpeed = 3;

            Rotation rotation = projectile.GetComponent<Rotation>();
            rotation.RotationAngle = Random.Range(-90f, 90f);

            projectile.Source = source;
        }

        return true;
    }
}
