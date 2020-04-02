using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
{
    public int baseDamage;

    /// <summary>
    /// Launches an attack to the target
    /// </summary>
    /// <param name="target">Entity to attack</param>
    protected override bool ExecuteAttack(Entity target)
    {
        if (target == null)
        {
            return false;
        }

        // Do Damage to the target
        target.ApplyDamage(Mathf.RoundToInt(baseDamage * entity.AttackFactor), DamageType.Physical, entity);
        return true;
    }

    new public void Update()
    {
        base.Update();
    }
}
