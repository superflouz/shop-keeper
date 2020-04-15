using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Faction of the party
/// </summary>
public enum Faction
{
    Ally,
    Enemy
}

public class Party : MonoBehaviour
{
    public float scrollSpeed;
    public float swapSpeed;
    public int maxSlots;
    public Faction faction;
    public List<Entity> entities;

    private bool isMoving;
    public bool IsMoving { get { return isMoving; } }

    public float delayMovement;
    private float timerDelay;

    // Start is called before the first frame update
    void Start()
    {
        // Set the transform of the child to the one of the party
        foreach (Entity entity in entities)
        {
            entity.Party = this;
            entity.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerDelay > 0) timerDelay -= Time.deltaTime;

        if (faction == Faction.Ally && timerDelay <= 0)
        {
            // Check if there is an ennemy in the front of the party
            isMoving = true;
            Collider2D[] hits = Physics2D.OverlapAreaAll(transform.position, transform.position + Vector3.right + Vector3.up * 5);
            foreach (Collider2D hit in hits)
            {
                Entity entity = hit.GetComponent<Entity>();
                if (entity != null && entity.Party != null && entity.Party.faction != Faction.Ally)
                {
                    isMoving = false;
                    timerDelay = delayMovement;
                }
            }

            // Move Party
            if (isMoving)
            {
                transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// Check the number of free slots in the party
    /// </summary>
    /// <returns>Number of free slots</returns>
    public int GetFreeSlotsCount()
    {
        int freeSlotsCount = maxSlots;
        foreach (Entity entity in entities)
        {
            freeSlotsCount -= Mathf.RoundToInt(entity.slotCount);
        }

        return freeSlotsCount;
    }

    /// <summary>
    /// Check the number of occupied slots in the party
    /// </summary>
    /// <returns>Number of occupied slots</returns>
    public int GetOccupiedSlotsCount()
    {
        int occupiedSlotsCount = 0;
        foreach (Entity entity in entities)
        {
            occupiedSlotsCount += Mathf.RoundToInt(entity.slotCount);
        }

        return occupiedSlotsCount;
    }

    /// <summary>
    /// Add an Entity to your party
    /// </summary>
    /// <param name="entity">Entity to add</param>
    public void AddToParty(Entity entity)
    {
        entities.Add(entity);
        entity.Party = this;
        entity.transform.parent = transform;
    }

    /// <summary>
    /// Remove an Entity from your party
    /// </summary>
    /// <param name="entity">Entity to remove</param>
    public void RemoveFromParty(Entity entity)
    {
        entities.Remove(entity);
        entity.Party = null;
        entity.transform.parent = null;
    }

    /// <summary>
    /// Swap an Entity with the one in front of them
    /// </summary>
    /// <param name="entity">Entity to swap</param>
    /// <returns>Entity can be swapped</returns>
    public bool SwapEntity(Entity entity)
    {
        int index = entities.IndexOf(entity);
        if (index < 0)
        {
            return false;
        }

        if (entities.Count > index + 1)
        {
            // Can only swap two idle entities
            if (entities[index].State != EntityState.Idle || entities[index + 1].State != EntityState.Idle)
                return false;

            entities[index] = entities[index + 1];
            entities[index + 1] = entity;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Get the position of the Entity in the party
    /// </summary>
    /// <param name="entity">Entity to get</param>
    /// <returns>Position of the Entity</returns>
    public Vector3 GetEntityLocalPosition(Entity entity)
    {
        // Get the position
        if (!entities.Contains(entity))
        {
            return entity.transform.localPosition;
        }

        // Calculate offset
        float offset = (entity.slotCount / 2f);
        foreach (Entity listEntity in entities)
        {
            if (listEntity == entity)
            {
                break;
            }
            offset += listEntity.slotCount;
        }

        // Enemy Party position
        if (faction == Faction.Enemy)
        {
            return new Vector3(offset, 0, 0);
        }
        else
        {
            return new Vector3(-offset, 0, 0);
        }  
    }
}
