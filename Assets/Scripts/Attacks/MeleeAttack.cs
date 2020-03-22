using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
{

    protected override void ExecuteAttack(Entity target)
    {
        if (target == null) {
            return;
        }
        target.ApplyDamage(user.attackDamage, DamageType.Physical, user);
    }

    new public void Update()
    {
        base.Update();
    }
}
