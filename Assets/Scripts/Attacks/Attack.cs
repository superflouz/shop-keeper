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

    protected State state;
    protected float timerAttack;
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
        // Let the attack timer go down if the Entity is not doing an other action
        if (timerAttack > 0 && entity.CurrentState != Entity.State.Swapping)
        {
            timerAttack -= Time.deltaTime;
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
                    ExecuteAttack(currentTarget);
                    currentTarget = null;

                    animator.SetTrigger("Attack");
                    state = State.Attacking;
                }
                // Cancel the attack if the entity move
                if (entity.CurrentState == Entity.State.Swapping)
                {
                    animator.SetTrigger("End Attack");
                    state = State.Idle;
                    entity.CurrentState = Entity.State.Idle;
                }

                break;
            // End the attack and go back to idle status
            case State.Attacking:
                timerAnimation -= Time.deltaTime;
                if (timerAnimation <= 0) {
                    animator.SetTrigger("End Attack");
                    state = State.Idle;
                    entity.CurrentState = Entity.State.Idle;
                }
                break;
        }
    }

    public void ResetAttack()
    {
        state = State.Idle;
        timerAttack = 0;
        timerPreparation = 0;
        timerAnimation = 0;
    }

    /// <summary>
    /// Set Up for an Attack
    /// </summary>
    /// <param name="target">Entity to attack</param>
    /// <returns>action success</returns>
    public virtual bool PrepareAttack(Entity target)
    {
        // Attack only if the cooldown is at 0 and the state allows it
        entity.CurrentState = Entity.State.Attacking;
        if (timerAttack > 0)
        {
            return false;
        }

        // Set target
        currentTarget = target;

        //  Set state, timer and animations
        state = State.Preparing;
        timerAttack = 1 / entity.attackSpeed;
        timerPreparation = timerAttack / 8;
        timerAnimation = timerAttack / 4;
        animator.SetTrigger("Prepare Attack");

        string s = name + " - Timer Attack: " + timerAttack + " - Timer Preparation: " + timerPreparation;
        Debug.Log(s);

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
