using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingAttack : Attack
{
    public int baseDamage;

    public CurvedProjectile scrap;
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
        if (target == null) 
        {
            return false;
        }

        for (int i = 0; i < projectileCount; i++) 
        {
            CurvedProjectile projectile;
            projectile = Instantiate(scrap, transform.position + Vector3.up * 0.5f, Quaternion.identity);

            // Calculate Origin and Destination
            projectile.Origin = transform.position + Vector3.right * transform.localScale.x * entity.slotCount / 4 + Vector3.up * entity.slotCount;
            projectile.Destination = target.transform.position + Vector3.up * 0.5f + Vector3.right * entity.transform.localScale.x * Random.Range(0f, i);

            // Set Speed
            projectile.ProjectileSpeed = projectileSpeed;

            // Set damage
            projectile.Damage = Mathf.RoundToInt(baseDamage * entity.AttackFactor);

            // Set rotation
            Rotation rotation = projectile.GetComponent<Rotation>();
            rotation.RotationAngle = Random.Range(-90f, 90f);

            projectile.Source = entity;
        }

        return true;
    }

}
