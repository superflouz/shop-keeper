using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public bool constantMovingAnimation;
    public float SlotCount { get { return entity.slotCount; } }
    public Team Team { get { return entity.team; } }

    public Party Party { get; set; }

    private Entity entity;
    private Animator animator;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        entity.killEvent += EntityKilled;
        if (constantMovingAnimation)
            animator.SetBool("Moving", true);
    }

    // Update is called once per frame
    void Update()
    {
        // How the entity move if it's on the party
        if (Party != null) {
            Vector3 wantedPosition = Party.GetEntityLocalPosition(this);

            if (transform.localPosition != wantedPosition) {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, wantedPosition, Party.swapSpeed * Time.deltaTime);

                // Moving Animation
                if (!constantMovingAnimation)
                    animator.SetBool("Moving", true);

                // The entity look in the movement direction
                if (transform.localPosition.x > wantedPosition.x) {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
                else {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
            else {
                // The entity doesn't move
                if (!constantMovingAnimation)
                    animator.SetBool("Moving", false);
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (Party != null) {
                Party.SwapEntity(this);
            }
        }
    }

    public void EntityKilled(Entity killed, Entity killer)
    {
        if (Party != null) {
            Party.RemoveFromParty(this);
        }
        Destroy(gameObject);
    }
}
