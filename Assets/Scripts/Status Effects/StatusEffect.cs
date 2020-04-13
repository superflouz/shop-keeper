using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum StatusEffectType
{
    None,
    Burn,
    Stun,
    Fear,
    Poison
}

public class StatusEffect : MonoBehaviour
{
    public float duration;
    public Attributes bonusAttributes;
    public StatusEffectType statusEffectType;

    public float TimeRemaining { get; set; }
    public Entity Entity { get; set; }
    public Entity Source { get; set; }

    private void Start()
    {
        TimeRemaining = duration;
        transform.localPosition = Vector3.up * (float)Entity.slotCount / 2;
    }

    private void Update()
    {
        if (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
            if (TimeRemaining <= 0)
            {
                Entity.RemoveStatusEffect(this);
            }
        }
    }
}
