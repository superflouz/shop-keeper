using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : Attack
{
    public Arrow arrow;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    protected override void ExecuteAttack(Entity target)
    {
        Arrow a; 
        a = Instantiate(arrow, transform.position, Quaternion.identity);
        a.Target = target;
    }

}
