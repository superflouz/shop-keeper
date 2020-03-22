using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{
    public float scrollSpeed;
    public float swapSpeed;
    public int maxSlots;
    public bool inversed;
    public List<EntityMovement> entities;

    private bool isMoving;
    public bool IsMoving { get { return isMoving; } }

    public float delayMovement;
    private float timerDelay;

    // Start is called before the first frame update
    void Start()
    {
        foreach (EntityMovement entity in entities) {
            entity.Party = this;
            entity.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerDelay > 0) {
            timerDelay -= Time.deltaTime;
        }

        if (!inversed && timerDelay <= 0) {
            // Check if there is an ennemy in the front of the party
            isMoving = true;
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + (Vector3.left * 0.5f), Vector2.one, 0);
            foreach (Collider2D hit in hits) {
                EntityMovement entity = hit.GetComponent<EntityMovement>();
                if (entity != null && entity.Team != Faction.Ally) {
                    isMoving = false;
                    timerDelay = delayMovement;
                }
            }

            if (isMoving) {
                transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);
            }
        }
    }

    public int GetFreeSlotsCount()
    {
        int freeSlotsCount = maxSlots;
        foreach (EntityMovement entity in entities) {
            maxSlots -= Mathf.RoundToInt(entity.SlotCount);
        }
        return freeSlotsCount;
    }

    public void AddToParty(EntityMovement entity)
    {
        entities.Add(entity);
        entity.Party = this;
        entity.transform.parent = transform;
    }

    public void RemoveFromParty(EntityMovement entity)
    {
        entities.Remove(entity);
        entity.Party = null;
        entity.transform.parent = null;
    }

    public bool SwapEntity(EntityMovement entity)
    {
        int index = entities.IndexOf(entity);
        if (index < 0) {
            return false;
        }

        if (entities.Count > index + 1) {
            entities[index] = entities[index + 1];
            entities[index + 1] = entity;
        }

        return true;
    }

    public Vector3 GetEntityLocalPosition(EntityMovement entity)
    {
        if (!entities.Contains(entity)) {
            return entity.transform.localPosition;
        }

        float offset = (entity.SlotCount / 2);
        foreach (EntityMovement listEntity in entities) {
            if (listEntity == entity) {
                break;
            }
            offset += listEntity.SlotCount;
        }

        if (inversed) {
            return new Vector3(offset, 0, 0);
        }
        else {
            return new Vector3(-offset, 0, 0);
        }  
    }
}
