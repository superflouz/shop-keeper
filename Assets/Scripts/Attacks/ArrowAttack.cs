using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : Attack
{
    public int baseDamage;

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
        // Instantiate arrow
        CurvedProjectile projectile; 
        projectile = Instantiate(arrow, transform.position + Vector3.up * 1f, Quaternion.identity);

        // Calculate Origin and Destination
        projectile.Origin = transform.position + Vector3.right * transform.localScale.x * entity.slotCount / 4 + Vector3.up * entity.slotCount;
        projectile.Destination = target.transform.position + Vector3.up * 0.5f + Vector3.right * Random.Range(-0.2f, 0.2f);

        // Set Speed
        projectile.ProjectileSpeed = projectileSpeed;

        // Set damage
        projectile.Damage = Mathf.RoundToInt(baseDamage * entity.AttackFactor);

        // Set user of the attack
        projectile.Source = entity;

        return true;
    }
}
