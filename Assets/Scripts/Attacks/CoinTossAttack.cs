using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTossAttack : Attack
{
    public int baseDamageCoins;
    public int baseDamageGems;
    public int baseDamageSpikes;
    public int baseDamageGoblin;

    public LuckyProjectile[] projectilesList = new LuckyProjectile[4];
    public float projectileSpeed;
    public float fallSpeed;

    new protected void Awake()
    {
        base.Awake();
    }

    protected override bool ExecuteAttack(Entity target)
    {
        if (target == null) {
            return false;
        }

        // Launch coins in all cases
        for (int c = 0; c < 20; c++)
        {
            LuckyProjectile projectile;
            projectile = Instantiate(projectilesList[0], transform.position + Vector3.up * 0.5f, Quaternion.identity);

            projectile.Origin = transform.position + Vector3.right * transform.localScale.x * entity.slotCount / 4 + Vector3.up * entity.slotCount;
            projectile.Destination = target.Party.transform.position +
                Vector3.right * transform.localScale.x * (0.5f + Random.Range(0, 5) + Random.Range(-0.2f, 0.2f));
            
            // Set speed
            projectile.ProjectileSpeed = projectileSpeed;
            projectile.FallSpeed = fallSpeed;
            projectile.Delay = Random.Range(0, 0.5f);

            // Set damage
            projectile.Damage = Mathf.RoundToInt(baseDamageCoins * entity.AttackFactor);

            // Set gold value
            projectile.GoldValue = 1;

            Rotation rotation = projectile.GetComponent<Rotation>();
            rotation.RotationAngle = Random.Range(-90f, 90f);

            projectile.Source = entity;
        }

        // Choose the projectile
        int rng = Random.Range(0, 5);
        int projectileIndex;
        int secondaryProjectileCount;
        int secondaryProjectileDamage;
        int goldValue = 0;
        switch (rng)
        {
            case 0:
                projectileIndex = 1;
                secondaryProjectileCount = 4;
                secondaryProjectileDamage = Mathf.RoundToInt(baseDamageGems * entity.AttackFactor);
                goldValue = 5;
                break;
            case 1:
                projectileIndex = 3;
                secondaryProjectileCount = 1;
                secondaryProjectileDamage = Mathf.RoundToInt(baseDamageGoblin * entity.AttackFactor);
                break;
            default:
                projectileIndex = 2;
                secondaryProjectileCount = 3;
                secondaryProjectileDamage = Mathf.RoundToInt(baseDamageSpikes * entity.AttackFactor);
                break;
        }

        for (int i = 0; i < secondaryProjectileCount; i++)
        {
            LuckyProjectile projectile;
            projectile = Instantiate(projectilesList[projectileIndex], transform.position + Vector3.up * 0.5f, Quaternion.identity);

            projectile.Origin = transform.position + Vector3.right * transform.localScale.x * entity.slotCount / 4 + Vector3.up * 2f;
            projectile.Destination = target.Party.transform.position +
                Vector3.right * transform.localScale.x * (0.5f + Random.Range(0, 5) + Random.Range(-0.2f, 0.2f));

            // Set Speed
            projectile.ProjectileSpeed = projectileSpeed;
            projectile.FallSpeed = fallSpeed;
            projectile.Delay = Random.Range(0, 0.5f);

            // Set Damage
            projectile.Damage = secondaryProjectileDamage;

            // Set gold value
            projectile.GoldValue = goldValue;

            Rotation rotation = projectile.GetComponent<Rotation>();
            if (rotation != null)
                rotation.RotationAngle = Random.Range(-90f, 90f);

            projectile.Source = entity;
        }

        return true;
    }
}
