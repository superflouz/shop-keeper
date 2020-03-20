using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
{
    public override void ExecuteAttack(Entity attacker, Entity target)
    {
        // Add Some pif paf poof animation here

        target.ApplyDamage(attacker.attackDamage, DamageType.Physical, attacker);
    }
}
