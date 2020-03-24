using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Ability
{
    public TimedGameObject particle;
    public int healValue;

    /// <summary>
    /// Cast the heal
    /// </summary>
    /// <param name="target">Target of the ability</param>
    /// <returns>sucess</returns>
    protected override bool CastAbility(Entity target)
    {
        if (target == null)
        {
            return false; ;
        }
        Instantiate(particle, target.transform.position + Vector3.up * 1f, Quaternion.identity, target.transform);

        int amount = Mathf.RoundToInt(healValue * (1 + (source.abilityPower / 100f)));
        target.Heal(amount, source);

        return true;
    }
}
