using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Following;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Following != null) {
            if (Following.position.x > transform.position.x) {
                float newPositionX = Following.position.x;

                transform.position = new Vector3(newPositionX, transform.position.y, transform.position.z);
            }
        }
    }
}
