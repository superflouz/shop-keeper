using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : CurveProjectile
{
    // Update is called once per frame
    new void Update()
    {
        Vector3 oldPosition = transform.position;

        base.Update();

        // Rotate the arrow
        if (transform.position != oldPosition) {
            var newRotation = Quaternion.LookRotation(transform.position - oldPosition, Vector3.forward);
            newRotation.x = 0.0f;
            newRotation.y = 0.0f;
            transform.rotation = newRotation;
        }
    }
}
