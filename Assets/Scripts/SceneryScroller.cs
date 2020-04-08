using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneryScroller : MonoBehaviour
{
    public Party party;
    Transform middleGround;
    Transform backGround;

    // Start is called before the first frame update
    void Start()
    {
        middleGround = transform.Find("Middleground");
        backGround = transform.Find("Background");
    }

    // Update is called once per frame
    void Update()
    {
        if (party.IsMoving)
        {
            middleGround.Translate(Vector3.right * party.scrollSpeed / 5 * Time.deltaTime);
            backGround.Translate(Vector3.right * party.scrollSpeed / 2 * Time.deltaTime);
        }
    }
}
