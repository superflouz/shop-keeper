﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected Entity entity;
    protected Attack attack;

    // Start is called before the first frame update
    void Awake()
    {
        entity = GetComponent<Entity>();
        attack = GetComponent<Attack>();
    }

    private void Update()
    {
        
    }
}
