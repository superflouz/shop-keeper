using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinKingAttack : Attack
{
    public LuckyProjectile[] projectileList = new LuckyProjectile[4];
    public float projectileSpeed;
    public float fallSpeed;

    public float ratioDamageCoins;
    public float ratioDamageGems;
    public float ratioDamageSpikes;
    public float ratioDamageGoblin;

    private TwoPhaseEntity twoPhaseEntity;

    new protected void Awake()
    {
        base.Awake();
        twoPhaseEntity = GetComponent<TwoPhaseEntity>();
    }

    protected override bool ExecuteAttack(Entity target)
    {
        if (target == null) {
            return false;
        }

        if (!twoPhaseEntity.SecondPhase)
        {
            // Launch coins in all cases
            for (int c = 0; c < 20; c++)
            {
                LuckyProjectile projectile;
                projectile = Instantiate(projectileList[0], transform.position + Vector3.up * 0.5f, Quaternion.identity);

                projectile.Origin = transform.position + Vector3.right * transform.localScale.x * entity.slotSize / 4 + Vector3.up * entity.slotSize;
                projectile.Destination = target.Party.transform.position +
                    Vector3.right * transform.localScale.x * (0.5f + Random.Range(0, 5) + Random.Range(-0.2f, 0.2f));
            
                // Set speed
                projectile.ProjectileSpeed = projectileSpeed;
                projectile.FallSpeed = fallSpeed;
                projectile.Delay = Random.Range(0, 0.5f);

                // Set damage
                projectile.Damage = Mathf.RoundToInt(entity.AttackDamage * ratioDamageCoins);

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
                    secondaryProjectileDamage = Mathf.RoundToInt(entity.AttackDamage * ratioDamageGems);
                    goldValue = 5;
                    break;
                case 1:
                    projectileIndex = 3;
                    secondaryProjectileCount = 1;
                    secondaryProjectileDamage = Mathf.RoundToInt(entity.AttackDamage * ratioDamageGoblin);
                    break;
                default:
                    projectileIndex = 2;
                    secondaryProjectileCount = 3;
                    secondaryProjectileDamage = Mathf.RoundToInt(entity.AttackDamage * ratioDamageSpikes);
                    break;
            }

            for (int i = 0; i < secondaryProjectileCount; i++)
            {
                LuckyProjectile projectile;
                projectile = Instantiate(projectileList[projectileIndex], transform.position + Vector3.up * 0.5f, Quaternion.identity);

                projectile.Origin = transform.position + Vector3.right * transform.localScale.x * entity.slotSize / 4 + Vector3.up * 2f;
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
        }
        else
        {
            target.ApplyDamage(entity.AttackDamage, DamageType.Physical, entity);
        }


        return true;
    }
}
