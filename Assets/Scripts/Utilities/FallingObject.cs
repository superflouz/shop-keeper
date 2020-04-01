using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public Vector3 Speed { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 speed = Speed;
        speed.y += Physics2D.gravity.y * Time.deltaTime;
        Speed = speed;

        transform.position = transform.position + Speed * Time.deltaTime;
    }
}
