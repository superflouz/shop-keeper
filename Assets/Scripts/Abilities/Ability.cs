﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    // State of the Ability
    protected enum State
    {
        Idle,
        Preparing,
        Casting
    }

    public int manaCost;
    public float preparationTime;
    public float animationTime;

    protected State state;
    protected float timerPreparation;
    protected float timerAnimation;

    protected Entity user;
    protected Entity currentTarget;
    protected Vector2 currentPosition;

    protected Animator animator;

    public void Awake()
    {
        // Get Components
        user = GetComponent<Entity>();
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        // Default values
        state = State.Idle;
    }

    public void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            // Prepare ability and go to the casting state
            case State.Preparing:
                timerPreparation -= Time.deltaTime;
                if (timerPreparation <= 0) {
                    CastAbility();
                    CastAbility(currentTarget);
                    CastAbility(currentPosition);
                    currentTarget = null;

                    animator.SetTrigger("Cast Ability");
                    state = State.Casting;
                }
                break;
            //End the ability and go back to idle status
            case State.Casting:
                timerAnimation -= Time.deltaTime;
                if (timerAnimation <= 0) {
                    animator.SetTrigger("End Ability");
                    state = State.Idle;
                    user.CurrentState = Entity.State.Idle;
                }
                break;
        }
    }

    /// <summary>
    /// Setup for an ability cast
    /// </summary>
    /// <returns>success</returns>
    public virtual bool PrepareAbility()
    {
        user.CurrentState = Entity.State.Casting;
        user.CurrentMana -= manaCost;

        state = State.Preparing;
        timerPreparation = preparationTime;
        timerAnimation = animationTime;
        animator.SetTrigger("Prepare Ability");

        return true;
    }

    /// <summary>
    /// Setup for an ability cast
    /// </summary>
    /// <param name="target">Target of the cast</param>
    /// <returns>success</returns>
    public virtual bool PrepareAbility(Entity target)
    {
        currentTarget = target;

        return PrepareAbility();
    }

    /// <summary>
    /// Setup for an ability cast
    /// </summary>
    /// <param name="position">Position of the cast</param>
    /// <returns>success</returns>
    public virtual bool PrepareAbility(Vector2 position)
    {
        currentPosition = position;

        return PrepareAbility();
    }

    /// <summary>
    /// Cast the ability
    /// </summary>
    protected virtual bool CastAbility()
    {
        // Default action, override in heritage to change it
        return true;
    }

    /// <summary>
    /// Cast the ability
    /// </summary>
    /// <param name="target">target of the ability</param>
    protected virtual bool CastAbility(Entity target)
    {
        // Default action, override in heritage to change it
        return CastAbility();
    }

    /// <summary>
    /// Cast the ability
    /// </summary>
    /// <param name="position">position of the ability</param>
    /// <returns></returns>
    protected virtual bool CastAbility(Vector2 position)
    {
        // Default action, override in heritage to change it
        return CastAbility();
    }
}
