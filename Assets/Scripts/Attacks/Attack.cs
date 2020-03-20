using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    protected DamageType damageType;

    public DamageType GetDamageType { get { return damageType; } set { damageType = value; } }

    // Execute actions, use different surcharges for different parameters

    public virtual void ExecuteAttack(Entity attacker, Entity target)
    {
        // Default action, override in heritage to change it
    }
}
