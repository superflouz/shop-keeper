using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Party party;

    private int currency;
    public int Currency
    {
        get { return currency; }
        set { currency = value; }
    }

    public bool BuyEntity(Entity entity, Buyable buyable)
    {
        int newCurrency = Currency - buyable.GetPrice();

        if (newCurrency >= 0)
        {
            // Instantiate Entity and Add it to party
            if (party.GetFreeSlotsCount() >= entity.slotCount)
            {
                Entity newEntity = Instantiate(entity);        
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
        Currency = 10000;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
