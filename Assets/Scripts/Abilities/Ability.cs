using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
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
        user = GetComponent<Entity>();
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        state = State.Idle;
    }

    public void Update()
    {
        switch (state) {
            case State.Idle:
                break;
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

    public virtual bool PrepareAbility(Entity target)
    {
        currentTarget = target;

        return PrepareAbility();
    }

    public virtual bool PrepareAbility(Vector2 position)
    {
        currentPosition = position;

        return PrepareAbility();
    }

    protected virtual void CastAbility()
    {
        // Default action, override in heritage to change it
    }

    protected virtual void CastAbility(Entity target)
    {
        // Default action, override in heritage to change it
    }

    protected virtual void CastAbility(Vector2 position)
    {
        // Default action, override in heritage to change it
    }
}
