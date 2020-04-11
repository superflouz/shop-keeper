using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarDrum : Ability
{
    public StatusEffect statusEffect;

    /// <summary>
    /// Cast the heal
    /// </summary>
    /// <param name="target">Target of the ability</param>
    /// <returns>sucess</returns>
    protected override bool CastAbility(Entity target)
    {
        if (target.Party == null)
            return false;

        foreach (Entity partyMember in target.Party.entities)
        {
            if (partyMember == target)
                continue;

            StatusEffect newStatus = Instantiate(statusEffect, partyMember.transform);
            newStatus.duration = 3;
            newStatus.bonusAttributes *= entity.AbilityFactor;
            newStatus.Source = entity;
            partyMember.AddStatusEffect(newStatus);
        }

        return true;
    }
}
