using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{
    public float scrollSpeed;
    public float swapSpeed;
    public int maxSlots;
    public List<EntityMovement> entities;

    private bool isMoving;
    public bool IsMoving { get { return isMoving; } }

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
        // Check if the party is moving
        isMoving = true;
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + (Vector3.left * 0.5f), Vector2.one, 0);
        foreach (Collider2D hit in hits) {
            EntityMovement entity = hit.GetComponent<EntityMovement>();
            if (entity != null && entity.Team != Team.Ally) {
                isMoving = false;
            }
        }

        if (isMoving) {
            transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);
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

    public void SwapEntity(EntityMovement entity)
    {
        int index = entities.IndexOf(entity);
        if (index < 0) {
            return;
        }

        if (entities.Count > index + 1) {
            entities[index] = entities[index + 1];
            entities[index + 1] = entity;
        }
    }

    public Vector3 GetEntityLocalPosition(EntityMovement entity)
    {
        if (!entities.Contains(entity)) {
            return entity.transform.localPosition;
        }

        float offset = -(entity.SlotCount / 2);
        foreach (EntityMovement listEntity in entities) {
            if (listEntity == entity) {
                break;
            }
            offset -= listEntity.SlotCount;
        }

        return new Vector3(offset, 0, 0);
    }
}
