using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction
{
    Ally,
    Enemy
}

public enum DamageType
{
    Magical,
    Physical,
    Piercing
}

public enum DamageType
{
    heal,
    magic,
    physical,
    piercing
}

public enum Range
{
    melee, 
    ranged
}

public class Entity : MonoBehaviour, IBuyable
{
    public enum State
    {
        Idle,
        Swapping
    }
    public int slotCount;
    public Faction faction;

    public int health;
    public int attackDamage;
    public float attackSpeed;
    public int abilityPower;
    public int physicArmor;
    public int magicArmor;
    
    public Attack attack;
    public int health;

    public float range;
    public float Range { get { return range; } }

    public Action action;
    public List<EntityAbility> abilities;

    public Sprite icon;
    public Sprite Icon { get { return icon; } }

    public int price;
    public int Price { get { return price; } }


    public delegate void KillEvent(Entity killed, Entity killer);
    public KillEvent killEvent;

    public State CurrentState { get; set; }

    private float timerAttack;
    private int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerAttack > 0) {
            timerAttack -= Time.deltaTime;
        }
    }

    public bool CanAttack()
    {
        return timerAttack <= 0;
    }

    public void SetTimerAttack()
    {
        timerAttack = 1 / attackSpeed;
    }

    public void ApplyDamage(int amount, DamageType type, Entity source)
    {
        float amountFloat = amount;

        string dmgType = "unknown";

        switch (type) {
            case DamageType.Physical:
                amountFloat -= (amountFloat * (physicArmor / 100f));
                dmgType = "physical";
                break;
            case DamageType.Magical:
                amountFloat -= (amountFloat * (magicArmor / 100f));
                dmgType = "magical";
                break;
            case DamageType.Piercing:
                dmgType = "piercing";
                break;
        }

        string s = name + " take " + Mathf.RoundToInt(amountFloat) + " " + dmgType + " damage(s) from " + source.name;
        Debug.Log(s);

        currentHealth -= Mathf.RoundToInt(amountFloat);
        if (currentHealth <= 0) {
            Kill(source);
        }
    }

    public void Kill(Entity killer = null)
    {
        killEvent?.Invoke(this, killer);
        Destroy(gameObject);
    }
}
