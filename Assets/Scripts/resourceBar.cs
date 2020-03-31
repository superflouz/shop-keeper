using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBar : MonoBehaviour
{
    Entity entity;
    Transform healthBar;
    Transform manaBar;

    bool hasMana;
    float maxHP;
    float maxMana;
    float currentHP;
    float currentMana;

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponentInParent<Entity>();

        healthBar = transform.Find("Health_ResourceBar");
        manaBar = transform.Find("Mana_ResourceBar");

        maxHP = entity.health;
        maxMana = entity.mana;

        if (maxMana == 0)
        {
            hasMana = false;
        }
        else
        {
            hasMana = true;          
        }      
    }

    // Update is called once per frame
    void Update()
    {
        currentHP = entity.CurrentHealth;

        healthBar.localScale = new Vector3(currentHP / maxHP, 1f);
        if (hasMana) manaBar.localScale = new Vector3(currentMana / maxMana, 1f);
    }

}
