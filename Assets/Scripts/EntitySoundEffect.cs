using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySoundEffect : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] attackSounds;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayAttackSound()
    {
        if (attackSounds.Length <= 0) return;
        int index = Random.Range(0, attackSounds.Length);
        audioSource.PlayOneShot(attackSounds[index]);
    }
}
