using System.Collections;
using System.Collections.Generic;
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

    protected Entity user;
    protected Entity currentTarget;

    protected Animator animator;

    public void Awake()
    {
        // Get the components
        user = GetComponent<Entity>();
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        // Default State
        state = State.Idle;
    }

    public void Update()
    {
        // Let the attack timer go down if the Entity is not doing an other action
        if (timerAttack > 0 && user.CurrentState != Entity.State.Swapping)
            timerAttack -= Time.deltaTime;

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
                break;
            // End the attack and go back to idle status
            case State.Attacking:
                timerAnimation -= Time.deltaTime;
                if (timerAnimation <= 0) {
                    animator.SetTrigger("End Attack");
                    state = State.Idle;
                    user.CurrentState = Entity.State.Idle;
                }
                break;
        }
    }

    /// <summary>
    /// Set Up for an Attack
    /// </summary>
    /// <param name="target">Entity to attack</param>
    /// <returns>action success</returns>
    public virtual bool PrepareAttack(Entity target)
    {
        // Attack only if the cooldown is at 0 and the state allows it
        user.CurrentState = Entity.State.Attacking;
        if (timerAttack > 0)
        {
            return false;
        }

        // Set target
        currentTarget = target;

        //  Set state, timer and animations
        state = State.Preparing;
        timerAttack = 1 / user.attackSpeed;
        timerPreparation = timerAttack / 8;
        timerAnimation = timerAttack / 4;
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
