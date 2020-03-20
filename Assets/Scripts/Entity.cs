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
    public int slotCount;
    public Team team;

    public int health;
    public int attackDamage;
    public float attackSpeed;
    public int abilityPower;
    public int physicArmor;
    public int magicArmor;
    
    public Attack attack;
    public List<EntityAbility> abilities;

    public Sprite icon;
    public Sprite Icon { get { return icon; } }

    public int price;
    public int Price { get { return price; } }


    public delegate void KillEvent(Entity killed, Entity killer);
    public KillEvent killEvent;

    public State CurrentState { get; set; }

    private float timerAttack;

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
        if (timerAttack > 0) {
            timerAttack -= Time.deltaTime;
        }
    }

    public bool CanAttack()
    {
        return timerAttack <= 0;
    }

    public void SetTimerAttack()
    {
        timerAttack = 1 / attackSpeed;
    }

    public void ApplyDamage(int amount, Attack.DamageType type)
    {

    }

    public void Kill(Entity killer = null)
    {
        killEvent?.Invoke(this, killer);
        Destroy(gameObject);
    }
}
