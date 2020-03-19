﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IBuyable
{
    public int health;

    public Action action;
    public List<CharacterAbility> abilities;

    public Sprite icon;
    public Sprite Icon { get { return icon; } }

    public int price;
    public int Price { get { return price; } }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
