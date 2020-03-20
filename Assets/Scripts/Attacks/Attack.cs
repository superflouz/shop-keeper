﻿using System.Collections;
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


    public Entity user;
    protected Entity currentTarget;

    public Animator animator;

    public void Update()
    {
        if (timerAttack > 0) {
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
                }
                break;
        }
    }

    public virtual bool PrepareAttack(Entity target)
    {
        if (timerAttack > 0) {
            return false;
        }

        currentTarget = target;

        state = State.Preparing;
        timerAttack = 1 / user.attackSpeed;
        timerPreparation = timerAttack / 3;
        timerAnimation = timerAttack / 3;
        animator.SetTrigger("Prepare Attack");

        return true;
    }

    protected virtual void ExecuteAttack(Entity target)
    {
        // Default action, override in heritage to change it
    }
}
