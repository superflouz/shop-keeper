using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Ally,
    Enemy
}

public class Entity : MonoBehaviour, IBuyable
{
    public enum State
    {
        Idle,
        Swapping
    }

    public int health;

    public Action action;
    public List<EntityAbility> abilities;

    public Sprite icon;
    public Sprite Icon { get { return icon; } }

    public int price;
    public int Price { get { return price; } }

    public int slotCount;
    public Team team;
    
    public delegate void KillEvent(Entity killed, Entity killer);
    public KillEvent killEvent;

    public State CurrentState { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        switch (CurrentState) {
            case State.Idle:
                break;
            case State.Swapping:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyDamage(int amount, Action.DamageType type)
    {

    }

    public void Kill(Entity killer = null)
    {
        killEvent?.Invoke(this, killer);
        Destroy(gameObject);
    }
}
