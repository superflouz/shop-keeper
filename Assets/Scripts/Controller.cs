using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Controller nextCharacter;
    public Controller previousCharacter;

    public bool ally;
    public float size;
    public bool IsMoving { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (ally) {
            if (nextCharacter != null) {
                if (Mathf.Abs((nextCharacter.transform.position.x - (nextCharacter.size / 2)) - (transform.position.x + (size / 2))) < 0.03125f) {
                    IsMoving = nextCharacter.IsMoving;
                    transform.transform.position = nextCharacter.transform.position + (Vector3.left * (nextCharacter.size + size) / 2);

                } else if (nextCharacter.transform.position.x - (nextCharacter.size / 2) > transform.position.x + (size / 2)) {
                    Debug.Log(Mathf.Abs((nextCharacter.transform.position.x - (nextCharacter.size / 2)) - (transform.position.x + (size / 2))));
                    if (FirstCharacter().IsMoving) {
                        transform.Translate(Vector3.right * 2 * Global.MoveSpeed * Time.deltaTime);
                        IsMoving = true;

                    } else {
                        transform.Translate(Vector3.right * Global.MoveSpeed * Time.deltaTime);
                        IsMoving = true;
                    }
                } else {
                    Debug.Log(Mathf.Abs((nextCharacter.transform.position.x - (nextCharacter.size / 2)) - (transform.position.x + (size / 2))));
                    if (FirstCharacter().IsMoving) {
                        IsMoving = false;

                    } else {
                        transform.Translate(Vector3.left * Global.MoveSpeed * Time.deltaTime);
                        IsMoving = true;
                    }
                }

            } else {
                bool enemyMelee = false;

                Collider2D[] hits = Physics2D.OverlapBoxAll((Vector2)transform.position + (Vector2.up * 0.5f), Vector2.one, 0);

                foreach (Collider2D hit in hits) {
                    Controller charaHit = hit.GetComponent<Controller>();
                    if (charaHit != null && !charaHit.ally) {
                        enemyMelee = true;
                    }
                }
                if (!enemyMelee) {
                    transform.Translate(Vector3.right * Global.MoveSpeed * Time.deltaTime);
                    IsMoving = true;
                } else {
                    IsMoving = false;
                }

            }
        }
    }

    public Controller FirstCharacter()
    {
        if (nextCharacter != null) {
            return nextCharacter.FirstCharacter();

        } else {
            return this;
        }
    }
}
