using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    public float duration;
    public Attributes bonusAttributes;
    //public AttributesFactors attributesFactors;
    public float TimeRemaining { get; set; }
    public Entity Entity { get; set; }

    private void Start()
    {
        TimeRemaining = duration;
        transform.localPosition = Vector3.up * (float)Entity.slotCount / 2;
    }

    private void Update()
    {
        TimeRemaining -= Time.deltaTime;
        if (TimeRemaining <= 0)
        {
            Entity.statusEffects.Remove(this);
            Destroy(gameObject);
        }
    }
}
