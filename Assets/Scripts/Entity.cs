using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Faction of the Entity
/// </summary>
public enum Faction
{
    Ally,
    Enemy
}

/// <summary>
/// Type of damage done or received
/// </summary>
public enum DamageType
{   
    Physical,
    Magical,
    Piercing
}

/// <summary>
/// Range type of the Entity
/// </summary>
public enum Range
{
    Melee, 
    Ranged
}

public class Entity : MonoBehaviour, IBuyable
{
    /// <summary>
    /// State of action of the Entity
    /// </summary>
    public enum State
    {
        Idle,
        Swapping,
        Attacking,
        Casting
    }

    public int slotSize;
    public Faction faction;

    public int health;
    public int mana;
    public int attackDamage;
    public float attackSpeed;
    public int abilityPower;
    public int physicArmor;
    public int magicArmor;


    public float range;
    public float Range { get { return range; } }

    public Sprite icon;
    public Sprite Icon { get { return icon; } }

    public int price;
    public int Price { get { return price; } }

    public Party Party { get; set; }


    public delegate void KillEvent(Entity killed, Entity killer);
    public KillEvent killEvent;

    public State CurrentState { get; set; }

    private int currentHealth;
    public int CurrentHealth {
        get {
            return currentHealth;
        }
        set {
            currentHealth = value;
        }
    }
    private float currentMana;
    public int CurrentMana {
        get {
            return Mathf.RoundToInt(currentMana);
        }
        set {
            currentMana = value;
        }
    }

    public int AttackDamage { 
        get {
            // Apply modifier here
            return attackDamage;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Default Values
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMana < mana)
        {
            currentMana = Mathf.MoveTowards(currentMana, mana, 10 * Time.deltaTime);
        }
    }

    /// <summary>
    /// Calculates and apply damages to the Entity
    /// </summary>
    /// <param name="amount">amount of damage</param>
    /// <param name="type">type of damage</param>
    /// <param name="source">Entity that send those damages</param>
    public void ApplyDamage(int amount, DamageType type, Entity source)
    {
        float amountFloat = amount;

        //string dmgType = "unknown";

        // Applies damage based on resistances
        switch (type)
        {
            case DamageType.Physical:
                amountFloat -= (amountFloat * (physicArmor / 100f));
                //dmgType = "physical";
                break;
            case DamageType.Magical:
                amountFloat -= (amountFloat * (magicArmor / 100f));
                //dmgType = "magical";
                break;
            case DamageType.Piercing:
                //dmgType = "piercing";
                break;
        }

        //string s = name + " take " + Mathf.RoundToInt(amountFloat) + " " + dmgType + " damage(s) from " + source.name;
        //Debug.Log(s);

        CurrentHealth -= Mathf.RoundToInt(amountFloat);
        if (currentHealth <= 0) {
            Kill(source);
        }
    }

    /// <summary>
    /// Heal the entity
    /// </summary>
    /// <param name="amount">amount to heal</param>
    /// <param name="source">Entity that healed</param>
    public void Heal(int amount, Entity source)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, health);

        //string s = name + " heal " + amount + " health from " + source.name;
        //Debug.Log(s);
    }

    /// <summary>
    /// Kill the Entity
    /// </summary>
    /// <param name="killer">Entity that killed</param>
    public void Kill(Entity killer = null)
    {
        killEvent?.Invoke(this, killer);
        Destroy(gameObject);
    }
}
