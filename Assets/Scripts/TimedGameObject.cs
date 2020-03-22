using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedGameObject : MonoBehaviour
{
    public float duration;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = duration;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            Destroy(gameObject);
        }
    }
}
