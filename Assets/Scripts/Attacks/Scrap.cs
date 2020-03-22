using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : RotatingProjectile
{
    public List<Sprite> spritesList; 

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = spritesList[Random.Range(0, spritesList.Count - 1)];
    }
}
