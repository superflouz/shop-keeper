using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float speed;
    public float duration;

    private float timer;

    public string Text { get; set; }
    public Color Color { get; set; }

    private TextMesh textMesh;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        textMesh.text = Text;
        textMesh.color = Color;

        timer = duration;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }

        
        // The sprite fade to transparency with the timer
        Color color = textMesh.color;
        color.a = timer / duration;
        textMesh.color = color;
        

        transform.position += Vector3.up * speed * Time.deltaTime;
    }
}
