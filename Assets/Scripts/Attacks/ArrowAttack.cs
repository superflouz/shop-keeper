using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : Attack
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    // Launch the arrow up
    public override void ExecuteAttack(Entity Entity, Entity Target)
    {

    }

    private Vector2 CalculateQuadraticBezierCurve(float t, Vector2 P0, Vector2 P1, Vector2 P2)
    {
        //P0 is origin
        //P1 is the top angle of the base triangle
        //P2 is target

        // B(t) = ((1-t)^2 * P0) + (2(1-t) * t * P1) + (t^2 * P2)
        //      =    (u ^2 * P0) +  (2 * u * t * P1) + (t^2 * P2)
        //      =      (uu * P0) +  (2 * u * t * P1) +  (tt * P2)

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector2 point = (uu * P0) + (2 * u * t * P1) + (tt * P2);

        return point;
    }


}
