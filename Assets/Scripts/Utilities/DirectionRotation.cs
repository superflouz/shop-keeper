using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionRotation : MonoBehaviour
{
    Vector3 oldPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != oldPosition)
        {
            var newRotation = Quaternion.LookRotation(transform.position - oldPosition, Vector3.forward);
            newRotation.x = 0.0f;
            newRotation.y = 0.0f;
            transform.rotation = newRotation;
        }

        oldPosition = transform.position;
    }
}
