using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected Entity entity;

    // Start is called before the first frame update
    void Awake()
    {
        entity = GetComponent<Entity>();
    }

    private void Update()
    {
        
    }
}
