using UnityEngine;

public class Attack : MonoBehaviour
{
    /// <summary>
    /// State of the attack
    /// </summary>
    protected enum State
    {
        Idle,
        Preparing,
        Attacking
    }

    public float attackSpeed;

    protected State state;
    protected float timerCooldown;
    protected float timerPreparation;
    protected float timerAnimation;

    protected Entity entity;
    protected Entity currentTarget;

    protected Animator animator;

    public void Awake()
    {
        // Get the components
        entity = GetComponent<Entity>();
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        // Default State
        state = State.Idle;
    }

    protected void Update()
    {
        // Let the attack timer go down if the Entity is not swapping
        if (timerCooldown > 0 && entity.State != EntityState.Swapping)
        {
            timerCooldown -= Time.deltaTime;
        }

        switch (state)
        {
            case State.Idle:
                break;
            // Prepare attack and go to the attacking state
            case State.Preparing:
                timerPreparation -= Time.deltaTime;
                if (timerPreparation <= 0)
                {
                    if (ExecuteAttack(currentTarget))
                    {
                        currentTarget = null;

                        animator.SetTrigger("Attack");
                        state = State.Attacking;
                    }
                    else
                    {
                        ResetAttack();
                    }

                }
                /*
                // Cancel the attack if the entity move
                if (entity.State == EntityState.Swapping)
                {
                    ResetAttack();
                }
                */

                break;
            // End the attack and go back to idle status
            case State.Attacking:
                timerAnimation -= Time.deltaTime;
                if (timerAnimation <= 0) {
                    animator.SetTrigger("End Attack");
                    state = State.Idle;
                }
                break;
        }
    }

    /// <summary>
    /// Reset the attack and the cooldown
    /// </summary>
    public void ResetAttack()
    {
        state = State.Idle;
        timerCooldown = 0;
        timerPreparation = 0;
        timerAnimation = 0;

        // Reset the triggers to avoid animation bug
        animator.SetTrigger("End Attack");
        animator.ResetTrigger("Prepare Attack");
        animator.ResetTrigger("Attack");
    }

    /// <summary>
    /// Set Up for an Attack
    /// </summary>
    /// <param name="target">Entity to attack</param>
    /// <returns>action success</returns>
    public virtual bool PrepareAttack(Entity target)
    {
        // Attack only if the cooldown is at 0
        if (timerCooldown > 0)
        {
            return false;
        }

        // Set target
        currentTarget = target;

        //  Set state, timer and animations
        state = State.Preparing;
        timerCooldown = 1 / (attackSpeed * entity.AttackSpeedFactor);
        timerPreparation = timerCooldown / 4;
        timerAnimation = timerCooldown / 4;
        animator.ResetTrigger("End Attack");
        animator.SetTrigger("Prepare Attack");

        return true;
    }
    
    /// <summary>
    /// Launches an attack to the target
    /// </summary>
    /// <param name="target">Entity to attack</param>
    protected virtual bool ExecuteAttack(Entity target)
    {
        // Default action, override in heritage to change it
        return true;
    }
}
