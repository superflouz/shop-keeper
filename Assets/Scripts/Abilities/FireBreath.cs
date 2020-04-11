using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBreath : Ability
{
    public Projectile projectile;
    public int baseDamage;

    protected override bool CastAbility(Vector2 position)
    {
        Projectile newProjectile = Instantiate(projectile, position, Quaternion.identity);
        newProjectile.Damage = Mathf.RoundToInt((float)baseDamage * entity.AbilityFactor);
        newProjectile.transform.localScale = entity.transform.localScale;
        // Set user of the attack
        newProjectile.Source = entity;

        return true;
    }
}
