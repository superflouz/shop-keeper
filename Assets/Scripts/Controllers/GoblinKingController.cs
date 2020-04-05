using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Required components
[RequireComponent(typeof(CoinTossAttack))]
[RequireComponent(typeof(MeleeAttack))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(GoblinKingPhaseChangeEffect))]
#endregion
public class GoblinKingController : Controller
{
    public float healthRatioTransition;
    public float phaseChangeTime;


    protected VisualEffect phaseChangeParticle;
    protected CoinTossAttack coinTossAttack;
    protected MeleeAttack meleeAttack;
    bool SecondPhase;
    float timerPhaseChange;

    private Animator animator;

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        coinTossAttack = GetComponent<CoinTossAttack>();
        meleeAttack = GetComponent<MeleeAttack>();
        animator = GetComponent<Animator>();
        phaseChangeParticle = GetComponent<GoblinKingPhaseChangeEffect>();
    }

    void Start()
    {
        SecondPhase = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!SecondPhase && (float)entity.CurrentHealth / entity.MaxHealth <= healthRatioTransition)
        {
            animator.SetTrigger("Begin Phase Changement");
            timerPhaseChange = phaseChangeTime;
            SecondPhase = true;
            entity.Stun = true;
            phaseChangeParticle.Execute();
        }
        else if (timerPhaseChange > 0)
        {
            timerPhaseChange -= Time.deltaTime;
            if (timerPhaseChange <= 0)
            {
                animator.SetTrigger("End Phase Changement");
                entity.Stun = false;
                Attack attack = GetComponent<Attack>();
                attack.ResetAttack();
                BoxCollider2D collider = GetComponent<BoxCollider2D>();

                Vector2 size = collider.size;
                size.y = 1.5f;
                collider.size = size;

                Vector2 offset = collider.offset;
                offset.y = 0.5f;
                collider.offset = offset;
            }
        }

        if (entity.State == EntityState.Idle)
        {
            float range = 0;

            // Maths for overlap
            Vector2 a = entity.transform.position;
            Vector2 b = (Vector2)transform.position + Vector2.right * transform.localScale.x * ((float)entity.slotCount / 2f + range + 0.5f) + Vector2.up * range;

            // Overlap to find ennemy
            Collider2D[] hits = Physics2D.OverlapAreaAll(a, b, 1 << LayerMask.NameToLayer("Entities"));
            foreach (Collider2D hit in hits)
            {
                Entity enemy = hit.GetComponent<Entity>();
                // Check the first enemy in front of the entity
                if (enemy.Party.faction != entity.Party.faction)
                {
                    if (!SecondPhase)
                        coinTossAttack.PrepareAttack(enemy);
                    else
                        meleeAttack.PrepareAttack(enemy);
                }
            }
        }
    }
}
