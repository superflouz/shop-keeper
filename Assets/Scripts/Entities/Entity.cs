using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public enum EntityState
{
    Idle,
    Swapping,
    Stunned
}

#region Required components
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
#endregion
public class Entity : MonoBehaviour
{
    // =================================================
    #region Inspector variables

    [SerializeField, Tooltip("The number of slot this entity will occupy.")]
    public int slotCount;
    [SerializeField, Tooltip("The starting level of the entity. The higher it is, the stronger the unit will be.")]
    public int startingLevel;
    [SerializeField, Tooltip("The base health of the entity without any modifier.")]
    public int baseHealth;
    [SerializeField, Tooltip("The base mana of the entity without any modifier.")]
    public int baseMana;
    [SerializeField, Tooltip("The armor of the entity without any modifier.")]
    public int armor;
    [SerializeField, Tooltip("The magic resistance of the entity without any modifier.")]
    public int magicResistance;

    #endregion
    // =================================================
    #region Other variables
    /// <summary>
    /// The party containing this entity
    /// </summary>
    public Party Party { get; set; }
    
    /// <summary>
    /// The entities can't do anything when stunned
    /// </summary>
    public bool Stun { get; set; }

    #endregion
    // =================================================
    #region Level and attributes

    // The level of the entity. Define the general power level of the entity.
    private int level;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            // When the entity level up, it gain max health and as much current health
            int oldMaxHealth = MaxHealth;
            level = value;
            CurrentHealth += MaxHealth - oldMaxHealth;
        }
    }

    // The attributes define the modifier of the base statistics of the entity. 
    // It's dynamically calculated from the current level and the buff/debuff.
    // This is the only place where the buff/debuff have to be applied!
    // You can't modify the attributes without using a buff/debuff or a level change.
    protected Attributes Attributes
    {
        get
        {
            return new Attributes(level); // + buff/debuff currently on (TODO)
        }
    }

    // The health is stored as float for the operations and read as an integer
    private float currentHealth;
    public int CurrentHealth 
    {
        get 
        {
            return Mathf.RoundToInt(currentHealth);
        }
        set
        {
            currentHealth = Mathf.Min(value, MaxHealth);
        }
    }

    // Max health is calculated with the base health and the current constitution
    public int MaxHealth
    {
        get
        {
            return Mathf.RoundToInt((float)baseHealth * ((float)Attributes.Constitution / 10));
        }
    }

    // The mana is stored as float for the operations and read as an integer
    private float currentMana;
    public int CurrentMana
    {
        get
        {
            return Mathf.RoundToInt(currentMana);
        }
        set
        {
            currentMana = Mathf.Min(value, MaxHealth);
        }
    }

    // Max mana has no possible modifier at the moment
    public int MaxMana
    {
        get
        {
            return baseMana;
        }
    }

    // Armor is calculated with the base armor and the current bonus armor
    public int Armor
    {
        get
        {
            return armor + Attributes.BonusArmor;
        }
    }

    // Magic resistance is calculated with the base magic resistance and the current bonus magic resistance
    public int MagicResistance
    {
        get
        {
            return magicResistance + Attributes.BonusMagicResistance;
        }
    }

    // The current factor to apply to the physaical damages of this entity
    public float AttackFactor
    {
        get
        {
            return (float)Attributes.Strength / 10;
        }
    }

    // The current factor to apply to the values of the abilities used by this entity
    public float AbilityFactor
    {
        get
        {
            return (float)Attributes.Intelligence / 10;
        }
    }

    // The current factor to apply to the attack speed of this entity
    public float AttackSpeedFactor
    {
        get
        {
            return (float)Attributes.Dexterity / 10;
        }
    }
    #endregion
    // =================================================
    #region State machine

    public EntityState State { get; protected set; }

    void UpdateStateMachine()
    {
        switch (State)
        {
            case EntityState.Idle:
                if (Stun)
                    State = EntityState.Stunned;
                else
                    State = MoveToRightPosition();
                break;
            case EntityState.Swapping:
                if (Stun)
                    State = EntityState.Stunned;
                else
                    State = MoveToRightPosition();
                break;
            case EntityState.Stunned:
                if (!Stun)
                    State = EntityState.Idle;
                break;
        }
    }

    EntityState MoveToRightPosition()
    {
        // How the entity move if it's on the party
        if (Party != null)
        {
            Vector3 wantedPosition = Party.GetEntityLocalPosition(this);

            // It's not where it's supposed to be
            if (transform.localPosition != wantedPosition)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, wantedPosition, Party.swapSpeed * Time.deltaTime);

                // Moving Animation
                animator.SetBool("Moving", true);

                // The entity look in the movement direction
                if (transform.localPosition.x > wantedPosition.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }

                // Tell the entity it's swapping
                return EntityState.Swapping;
            }
            else
            {
                // The entity move only if the party is moving
                animator.SetBool("Moving", Party.IsMoving);

                // Look in the party direction
                if (Party.faction == Faction.Ally)
                    transform.localScale = new Vector3(1, 1, 1);
                else
                    transform.localScale = new Vector3(-1, 1, 1);

                // Tell the entity it's idle
                return EntityState.Idle;
            }
        }

        // If no party is found, do nothing
        return State;
    }
    #endregion
    // =================================================
    #region Damages, heal and death

    public delegate void KillEvent(Entity killed, Entity killer);
    public KillEvent killEvent;

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
                amountFloat -= (amountFloat * (Armor / 100f));
                //dmgType = "physical";
                break;
            case DamageType.Magical:
                amountFloat -= (amountFloat * (MagicResistance / 100f));
                //dmgType = "magical";
                break;
            case DamageType.Piercing:
                //dmgType = "piercing";
                break;
        }

        //string s = name + " take " + Mathf.RoundToInt(amountFloat) + " " + dmgType + " damage(s) from " + source.name;
        //Debug.Log(s);

        CurrentHealth -= Mathf.RoundToInt(amountFloat);
        if (CurrentHealth <= 0)
        {
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
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);

        //string s = name + " heal " + amount + " health from " + source.name;
        //Debug.Log(s);
    }

    /// <summary>
    /// Kill the Entity
    /// </summary>
    /// <param name="killer">Entity that killed</param>
    public void Kill(Entity killer = null)
    {
        Party.RemoveFromParty(this);

        killEvent?.Invoke(this, killer);
        Destroy(gameObject);
    }
    #endregion
    // =================================================
    #region Components references and initialization

    protected Animator animator;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected void Start()
    {
        level = startingLevel;
        State = EntityState.Idle;
        CurrentHealth = MaxHealth;
    }
    #endregion
    // =================================================
    #region Update
    protected void Update()
    {
        // All entities get 10 mana each seconds.
        if (currentMana < MaxMana)
            currentMana = Mathf.MoveTowards(currentMana, MaxMana, 10 * Time.deltaTime);

        UpdateStateMachine();
    }
    #endregion
    // =================================================
    #region Mouse over

    void OnMouseOver()
    {
        // When you click on an entity
        if (Input.GetMouseButtonDown(0))
        {
            if (Party != null)
            {
                // Swap the clicked entity
                Party.SwapEntity(this);
            }
        }
    }

    #endregion
}

/*
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
        Casting,
        Transitioning
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

    // --------------------------------------------------------------------------

    public delegate void KillEvent(Entity killed, Entity killer);
    public KillEvent killEvent;

    // --------------------------------------------------------------------------

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

    // --------------------------------------------------------------------------

    protected Animator animator;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    protected void Start()
    {
        // Default Values
        currentHealth = health;
    }

    // Update is called once per frame
    protected void Update()
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
*/
