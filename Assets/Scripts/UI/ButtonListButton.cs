using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListButton : MonoBehaviour
{
    public ButtonListControl buttonControl;
    public Image icon;
    public Text text;

    private Buyable buyable;
    public Buyable Buyable
    {
        get { return buyable; }
        set { buyable = value; }
    }

    private Entity entity;
    public Entity Entity
    {
        get { return entity; }
        set { entity = value; }
    }

    private int price;
    public int Price
    {
        get { return price; }
        set
        {
            text.text = CharaName + "\n\n" + value;
            price = value;
        }
    }

    private string charaName;
    public string CharaName
    {
        get { return charaName; }
        set
        {
            text.text = value + "\n\n" + CharaDescr;
            charaName = value;
        }
    }

    private string charaDescr;
    public string CharaDescr
    {
        get { return charaDescr; }
        set { charaDescr = value; }
    }

    private Sprite charaIcon;
    public Sprite CharaIcon
    {
        get { return charaIcon; }
        set
        {
            icon.sprite = value;
            charaIcon = value;
        }
    }

    public void OnClick()
    {
        // Buy Character on click
        buttonControl.ButtonClicked(Entity, Buyable);
    }
}
