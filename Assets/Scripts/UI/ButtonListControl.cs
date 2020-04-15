using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; // Used for "Where" search

public class ButtonListControl : MonoBehaviour
{
    public ShopManager shopManager;
    public GameObject buttonTemplate;
    public List<Buyable> buyableUnits;

    private List<GameObject> buttons;

    // Start is called before the first frame update
    void Start()
    {
        buttons = new List<GameObject>();

        if (buttons.Count > 0)
        {
            foreach(GameObject button in buttons)
            {
                Destroy(button.gameObject);
            }

            buttons.Clear();
        }

        foreach (Buyable i in buyableUnits)
        {
            GameObject button = Instantiate(buttonTemplate);
            button.SetActive(true);
            button.transform.SetParent(buttonTemplate.transform.parent, false);
            
            ButtonListButton singleButton = button.GetComponent<ButtonListButton>();
            singleButton.Buyable = i;
            singleButton.Entity = i.GetEntity();    
            singleButton.CharaName = i.GetName();
            singleButton.Price = i.GetPrice();
            singleButton.CharaIcon = i.icon;
            singleButton.CharaDescr = i.GetDescription();

            buttons.Add(button);
        }
    }

    public void ButtonClicked(Entity entity, Buyable buyable)
    {
        shopManager.BuyEntity(entity, buyable);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
