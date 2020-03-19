using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public enum DamageType
    {
        heal,
        magic,
        physical,
        piercing
    }

    protected DamageType damageType;

    public DamageType GetDamageType { get { return damageType; } set { damageType = value; } }

    // Execute actions, use different surcharges for different parameters

    public virtual void ExecuteAction(Entity target)
    {
        // Default action, override in heritage to change it
    }
    public virtual void ExecuteAction(int a, int b) { }
}
