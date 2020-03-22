using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
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
        user = GetComponent<Entity>();
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        state = State.Idle;
    }

    public void Update()
    {
        if (timerAttack > 0 && user.CurrentState != Entity.State.Swapping) {
            timerAttack -= Time.deltaTime;
        }

        switch (state) {
            case State.Idle:
                break;
            case State.Preparing:
                timerPreparation -= Time.deltaTime;
                if (timerPreparation <= 0) {
                    ExecuteAttack(currentTarget);
                    currentTarget = null;

                    animator.SetTrigger("Attack");
                    state = State.Attacking;
                }
                break;
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

    public virtual bool PrepareAttack(Entity target)
    {
        user.CurrentState = Entity.State.Attacking;
        if (timerAttack > 0) {
            return false;
        }
        currentTarget = target;

        state = State.Preparing;
        timerAttack = 1 / user.attackSpeed;
        timerPreparation = timerAttack / 8;
        timerAnimation = timerAttack / 4;
        animator.SetTrigger("Prepare Attack");

        return true;
    }

    protected virtual void ExecuteAttack(Entity target)
    {
        // Default action, override in heritage to change it
    }
}
