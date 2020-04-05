using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBar : MonoBehaviour
{
    Entity entity;
    Transform healthBar;
    Transform manaBar;
    Transform manaBarBorder;

    bool hasMana;
    float maxHP;
    float maxMana;
    float currentHP;
    float currentMana;

    // Start is called before the first frame update
    void Start()
    {
        // Get the objects we need to affect
        entity = GetComponentInParent<Entity>();
        healthBar = transform.Find("Health_ResourceBar");
        manaBar = transform.Find("Mana_ResourceBar");
        manaBarBorder = transform.Find("Mana_ResourceBarBorder");

        maxHP = entity.MaxHealth;
        maxMana = entity.MaxMana;

        // Defines if this entity has mana
        if (entity.baseMana == 0)
            hasMana = false;
        else
            hasMana = true;

        // Disable Mana Bar if it doesn't have mana
        manaBarBorder.gameObject.SetActive(hasMana);
        manaBar.gameObject.SetActive(hasMana);
    }

    // Update is called once per frame
    void Update()
    {
        maxHP = entity.MaxHealth;
        maxMana = entity.MaxMana;

        // Get current resources
        currentHP = entity.CurrentHealth;
        currentMana = entity.CurrentMana;

        // Set health and mana bar based on current resources
        healthBar.localScale = new Vector3(currentHP / maxHP, 1f);
        if (hasMana) manaBar.localScale = new Vector3(currentMana / maxMana, 1f);
    }

}
