using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    const int currencyCap = 99999;
    public Party party;

    private int currency;
    public int Currency
    {
        get { return currency; }
        set
        {
            if (currency > currencyCap)
                currency = currencyCap;
            else
            {
                currency = value;
                currencySet?.Invoke(value);
            }                             
        }
    }

    // Fire this event when the currency changes
    public delegate void OnCurrencyChanged(int amount, int currency);
    public OnCurrencyChanged currencyChanged;

    public delegate void OnCurrencySet(int currency);
    public OnCurrencySet currencySet;

    public void GainEntityReward(Entity killed, Entity killer)
    {
        AddCurrency(killed.GoldYield);
    }

    public void AddCurrency(int amount)
    {     
        Currency += amount;
        currencyChanged?.Invoke(amount, currency);
    }

    public bool BuyEntity(Entity entity, Buyable buyable)
    {
        int newCurrency = Currency - buyable.GetPrice();

        if (newCurrency >= 0)
        {
            // Instantiate Entity and Add it to party
            if (party.GetFreeSlotsCount() >= entity.slotCount)
            {
                Entity newEntity = Instantiate(entity, party.transform.position, Quaternion.identity);
                newEntity.transform.position = party.transform.position + Vector3.left * 10;
                newEntity.killEvent += GainEntityReward;
                party.AddToParty(newEntity);
                Currency = newCurrency;

                return true;
            }

            return false;
        }
        else
        {
            Debug.Log("ur poor");
            return false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Currency = 100;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
