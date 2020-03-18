using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    public bool scrolling;
    public float scrollingSpeed;
    private Party party;

    void Awake()
    {
        party = GetComponent<Party>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (scrolling) {
            bool blocked = false;
            Debug.Log(party.FrontPositionX);
            Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(party.FrontPositionX, 0.5f), Vector2.one, 0);
            foreach (Collider2D hit in hits) {
                Character character = hit.GetComponent<Character>();
                if (character != null && !character.ally) {
                    blocked = true;
                }
            }

            if (!blocked) {
                transform.Translate(new Vector3(scrollingSpeed * Time.deltaTime, 0, 0));
            }
        }
    }
}
