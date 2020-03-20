using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
{

    protected override void ExecuteAttack(Entity target)
    {
        target.ApplyDamage(user.attackDamage, DamageType.Physical, user);
    }

    new public void Update()
    {
        base.Update();
    }
}
