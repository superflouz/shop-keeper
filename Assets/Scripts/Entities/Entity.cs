using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Enum definition
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
/// States of the entity
/// </summary>
public enum EntityState
{
    Idle,
    Swapping,
    Stunned,
    Dead
}

#endregion

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
    [SerializeField, Tooltip("The prefab for the floating text.")]
    public FloatingText floatingText;

    #endregion
    // =================================================
    #region Other variables
    /// <summary>
    /// The party containing this entity
    /// </summary>
    public Party Party { get; set; }
    
    /// <summary>
    /// The entity can't do anything when stunned
    /// </summary>
    public bool Stun { get; set; }

    /// <summary>
    /// The entity is dead
    /// </summary>
    public bool Dead { get; set; }

    #endregion
    // =================================================
    #region Level and attributes
    // List of the status effects currently active on the entity
    public List<StatusEffect> statusEffects;

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
            Attributes attributes = new Attributes(level);
            //AttributesFactors factors = AttributesFactors.One;
            foreach (StatusEffect statusEffect in statusEffects)
            {
                attributes += statusEffect.bonusAttributes;
                //factors *= statusEffect.attributesFactors;
            }

            return attributes;
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

    // The current factor to apply to the physical damages of this entity
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
                if (Dead)
                    State = EntityState.Dead;     
                else if (Stun)
                    State = EntityState.Stunned;
                else
                    State = MoveToRightPosition();
                break;
            case EntityState.Swapping:
                if (Dead)
                    State = EntityState.Dead;
                else
                    State = MoveToRightPosition();
                break;
            case EntityState.Stunned:
                if (Dead)
                    State = EntityState.Dead;
                else if (!Stun)
                    State = EntityState.Idle;
                break;
            case EntityState.Dead:
                UpdateDeathAnimation();
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
            }
            if (transform.localPosition == wantedPosition)
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
            else
            {
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
        }

        // If no party is found, do nothing
        return State;
    }
    #endregion
    // =================================================
    #region Damages, heal and death
    public delegate void KillEvent(Entity killed, Entity killer);
    public KillEvent killEvent;

    // Variables for the daeth animation
    private Vector3 DeathSpeed = new Vector3(-1, 2, 0);
    private float timerFade = 1;
    private float rotationSpeed = 20;

    /// <summary>
    /// Calculates and apply damages to the Entity
    /// </summary>
    /// <param name="amount">amount of damage</param>
    /// <param name="type">type of damage</param>
    /// <param name="source">Entity that send those damages</param>
    public void ApplyDamage(int amount, DamageType type, Entity source)
    {
        float amountFloat = amount;

        FloatingText text = Instantiate(floatingText, transform.position, Quaternion.identity);
        text.transform.localPosition += Vector3.up * slotCount;

        // Applies damage based on resistances
        switch (type)
        {
            case DamageType.Physical:
                amountFloat -= (amountFloat * (Armor / 100f));
                text.Color = Color.red;
                break;
            case DamageType.Magical:
                amountFloat -= (amountFloat * (MagicResistance / 100f));
                text.Color = Color.magenta;
                break;
            case DamageType.Piercing:
                text.Color = Color.white;
                break;
        }

        text.Text = "-" + Mathf.RoundToInt(amountFloat);

        CurrentHealth -= Mathf.RoundToInt(amountFloat);
        if (CurrentHealth <= 0)
            Kill(source);
    }

    /// <summary>
    /// Heal the entity
    /// </summary>
    /// <param name="amount">amount to heal</param>
    /// <param name="source">Entity that healed</param>
    public void Heal(int amount, Entity source)
    {
        int healAmount = Mathf.Min(amount, MaxHealth - CurrentHealth);
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);

        FloatingText text = Instantiate(floatingText, transform);
        text.Text = "+" + healAmount;
        text.Color = Color.green;
        text.transform.localPosition += Vector3.up * slotCount;

        //string s = name + " heal " + amount + " health from " + source.name;
        //Debug.Log(s);
    }

    /// <summary>
    /// Kill the Entity
    /// </summary>
    /// <param name="killer">Entity that killed</param>
    public void Kill(Entity killer = null)
    {
        // Can't die two times
        if (Dead)
            return;

        Dead = true;
        Party.RemoveFromParty(this);

        // Disable all components
        Controller controller = GetComponent<Controller>();
        if (controller != null)
            controller.enabled = false;
        Attack[] attacks = GetComponents<Attack>();
        foreach (Attack attack in attacks)
            attack.enabled = false;
        Ability[] abilities = GetComponents<Ability>();
        foreach (Ability ability in abilities)
            ability.enabled = false;

        // Set the layer to dead
        gameObject.layer = LayerMask.NameToLayer("Deads");

        // Inverse the entity
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, 1);
        transform.position = new Vector3(transform.position.x, transform.position.y + slotCount, transform.position.z);
        DeathSpeed = new Vector3(DeathSpeed.x * transform.localScale.x, DeathSpeed.y, 0);
        rotationSpeed *= transform.localScale.x;

        //Mask the resource bar
        resourceBar.SetActive(false);

        // Send the event
        killEvent?.Invoke(this, killer);
    }

    private void UpdateDeathAnimation()
    {
        timerFade -= Time.deltaTime;
        if (timerFade <= 0)
            Destroy(gameObject);

        Vector3 speed = DeathSpeed;
        speed.y += Physics2D.gravity.y * Time.deltaTime;
        DeathSpeed = speed;

        transform.position += (DeathSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        Color color = Color.white;
        color.a = timerFade;
        body.color = color;
        legs.color = color;

    }
    #endregion
    // =================================================
    #region Components references and initialization

    protected Animator animator;
    protected SpriteRenderer body;
    protected SpriteRenderer legs;
    protected GameObject resourceBar;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Body"))
                body = child.GetComponent<SpriteRenderer>();
            if (child.CompareTag("Legs"))
                legs = child.GetComponent<SpriteRenderer>();
            if (child.CompareTag("ResourceBar"))
                resourceBar = child.gameObject;
        }
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

        // All entities get 2 health each seconds.
        if (currentHealth < MaxHealth)
            currentHealth = Mathf.MoveTowards(currentHealth, MaxHealth, 2 * Time.deltaTime);

        // Check if a click was buffered
        if (bufferClickTimer > 0)
        {
            if (Party.SwapEntity(this))
                bufferClickTimer = 0;
            else
                bufferClickTimer -= Time.deltaTime;
        }

        // Update the state machine and the entity movements
        UpdateStateMachine();
    }
    #endregion
    // =================================================
    #region Mouse over

    private float bufferClickTimer;

    void OnMouseOver()
    {
        // When you click on an entity
        if (Input.GetMouseButtonDown(0))
        {
            if (Party != null)
            {
                // Swap the clicked entity. If it fail, buffer the input for 0.5s.
                if (!Party.SwapEntity(this))
                    bufferClickTimer = 0.5f;
            }
        }
    }

    #endregion
    // =================================================
}
