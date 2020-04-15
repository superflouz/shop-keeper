using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buyable : MonoBehaviour
{
    public Entity entity;
    public Sprite icon;
    public string objectName;
    public string description;
    public int price;

    public Entity GetEntity()
    {
        return entity;
    }

    public string GetName()
    {
        return objectName;
    }

    public int GetPrice()
    {
        return price;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetDescription()
    {
        return description;
    }
}
