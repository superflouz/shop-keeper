using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPhaseEntity : Entity
{
    public float healthRatioTransition;
    public float transitionTime;

    protected float timerTransition;
    public bool SecondPhase { get; set; }
    protected TransitionParticle transition;

    new protected void Awake()
    {
        base.Awake();
        transition = GetComponent<TransitionParticle>();
    }

    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();
        SecondPhase = false;
    }

    // Update is called once per frame
    new protected void Update()
    {
        base.Update();

        if (!SecondPhase && (float)CurrentHealth / health <= healthRatioTransition)
        {
            animator.SetTrigger("Begin Transition");
            timerTransition = transitionTime;
            SecondPhase = true;
            CurrentState = State.Transitioning;
            transition.Execute();
        }
        else if (timerTransition > 0)
        {
            timerTransition -= Time.deltaTime;
            if (timerTransition <= 0)
            {
                // Reset the trigger to avoid animation bug
                animator.ResetTrigger("Prepare Attack");
                animator.ResetTrigger("Attack");
                animator.ResetTrigger("End Attack");

                animator.ResetTrigger("Prepare Ability");
                animator.ResetTrigger("Cast Ability");
                animator.ResetTrigger("End Abiltiy");

                animator.SetTrigger("End Transition");
                CurrentState = State.Idle;
                slotSize = 1;
                attackSpeed = 0.75f;
                Attack attack = GetComponent<Attack>();
                attack.ResetAttack();
                BoxCollider2D collider = GetComponent<BoxCollider2D>();

                Vector2 size = collider.size;
                size.x = 0.5f;
                size.y = 1.5f;
                collider.size = size;

                Vector2 offset = collider.offset;
                offset.y = 0.5f;
                collider.offset = offset;
            }
        }
    }
}
