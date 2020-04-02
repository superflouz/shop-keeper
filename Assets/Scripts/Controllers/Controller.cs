using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected Entity entity;

    // Start is called before the first frame update
    protected void Awake()
    {
        // Get Components
        entity = GetComponent<Entity>();
    }
}
