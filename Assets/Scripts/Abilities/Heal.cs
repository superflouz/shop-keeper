using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Ability
{
    public TimedGameObject particle;
    public int healValue;

    protected override void CastAbility(Entity target)
    {
        if (target == null) {
            return;
        }
        Instantiate(particle, target.transform.position + Vector3.up * 1f, Quaternion.identity);

        int amount = Mathf.RoundToInt(healValue * (1 + (user.abilityPower / 100f)));
        target.Heal(amount, user);
    }
}
