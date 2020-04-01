using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedGameObject : MonoBehaviour
{
    public float duration;
    public bool fade;
    private float timer;

    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        timer = duration;
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }

        if (fade)
        {
            // The sprite fade to transparency with the timer
            Color color = sprite.color;
            color.a = timer / duration;
            sprite.color = color;
        }
    }
}
