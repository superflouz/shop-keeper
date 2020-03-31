using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage { get; set; }
    public Entity Source { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        // Overlap detection
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.05f, 1 << LayerMask.NameToLayer("Entities"));
        foreach (Collider2D hit in hits)
        {
            Entity entity = hit.GetComponent<Entity>();

            // Something got hit
            Hit(entity);
        }
    }


    /// <summary>
    /// Overridable function called when an entity is hit by a projectile
    /// </summary>
    /// <param name="entity">Entity hit</param>
    virtual protected void Hit(Entity entity)
    {
        if (entity.faction != Source.faction)
        {
            // Make the recipient of the projectile take damage
            entity.ApplyDamage(Damage, DamageType.Physical, Source);

            // Destroy this projectile
            Destroy(gameObject);
        }
    }
}
