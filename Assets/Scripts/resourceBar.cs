using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resourceBar : MonoBehaviour
{
    Entity entity;
    Transform health;
    Transform mana;
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

        health = transform.Find("Health");
        healthBar = health.transform.Find("HealthBar");

        mana = transform.Find("Mana");
        manaBar = mana.transform.Find("ManaBar");

        maxHP = entity.health;
        maxMana = entity.mana;

        if (maxMana == 0)
        {
            hasMana = false;
            mana.gameObject.SetActive(false);
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
        currentMana = entity.CurrentMana;

        healthBar.localScale = new Vector3(currentHP / maxHP, 1f);
        if (hasMana) manaBar.localScale = new Vector3(currentMana / maxMana, 1f);
    }


}
