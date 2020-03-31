using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuckyToss : Ability
{
    public CurvedSlottedProjectile[] projectileList = new CurvedSlottedProjectile[4];
    public float projectileSpeed;
    public float fallSpeed;

    protected override bool CastAbility(Entity target)
    {
        if (target == null) {
            return false;
        }

        int random = Random.Range(0, 5);
        if (random == 0)
            random = 1;
        else if (random == 4)
            random = 3;
        else
            random = 2;

        for (int c = 0; c < 20; c++)
        {
            CurvedSlottedProjectile projectile;
            projectile = Instantiate(projectileList[0], transform.position + Vector3.up * 0.5f, Quaternion.identity);

            projectile.Origin = transform.position + Vector3.right * transform.localScale.x * source.slotSize / 4 + Vector3.up * 2f;
            projectile.Destination = target.Party.transform.position +
                Vector3.right * transform.localScale.x * (0.5f + Random.Range(0, 5) + Random.Range(-0.2f, 0.2f));

            projectile.ProjectileSpeed = projectileSpeed;
            projectile.FallSpeed = fallSpeed;
            projectile.Delay = Random.Range(0, 0.5f);

            Rotation rotation = projectile.GetComponent<Rotation>();
            rotation.RotationAngle = Random.Range(-90f, 90f);

            projectile.Source = source;
        }

        int secondaryProjectileCount;
        switch (random)
        {
            case 1:
                secondaryProjectileCount = 4;
                break;
            case 2:
                secondaryProjectileCount = 4;
                break;
            case 3:
                secondaryProjectileCount = 1;
                break;
            default:
                secondaryProjectileCount = 0;
                break;
        }

        for (int i = 0; i < secondaryProjectileCount; i++)
        {
            CurvedSlottedProjectile projectile;
            projectile = Instantiate(projectileList[random], transform.position + Vector3.up * 0.5f, Quaternion.identity);

            projectile.Origin = transform.position + Vector3.right * transform.localScale.x * source.slotSize / 4 + Vector3.up * 2f;
            projectile.Destination = target.Party.transform.position +
                Vector3.right * transform.localScale.x * (0.5f + Random.Range(0, 5) + Random.Range(-0.2f, 0.2f));

            projectile.ProjectileSpeed = projectileSpeed;
            projectile.FallSpeed = fallSpeed;
            projectile.Delay = Random.Range(0, 0.5f);

            Rotation rotation = projectile.GetComponent<Rotation>();
            if (rotation != null)
                rotation.RotationAngle = Random.Range(-90f, 90f);

            projectile.Source = source;
        }

        return true;
    }
}
