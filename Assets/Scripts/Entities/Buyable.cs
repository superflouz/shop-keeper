using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buyable : MonoBehaviour
{
    public Sprite icon;
    public string objectName;
    public string description;
    public int price;

    public string GetName()
    {
        return objectName;
    }

    public string GetDescription()
    {
        return description;
    }
}
