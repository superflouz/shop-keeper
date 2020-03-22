using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingProjectile : CurveProjectile
{
    public float RotationAngle { get; set; }

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        // Rotate the scrap
        transform.Rotate(Vector3.forward, RotationAngle * Time.deltaTime);
    }
}
