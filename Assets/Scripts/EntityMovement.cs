using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public float SlotCount { get { return entity.slotCount; } }
    public Team Team { get { return entity.team; } }

    public Party Party { get; set; }

    public Entity entity;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    public void Start()
    {
        entity.killEvent += EntityKilled;
    }

    // Update is called once per frame
    void Update()
    {
        if (Party != null) {
            Vector3 wantedPosition = Party.GetEntityLocalPosition(this);

            if (transform.localPosition != wantedPosition) {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, wantedPosition, Global.SwapSpeed * Time.deltaTime);
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
