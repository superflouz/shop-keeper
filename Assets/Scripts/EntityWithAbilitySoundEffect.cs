using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWithAbilitySoundEffect : EntitySoundEffect
{
    public AudioClip[] abilitySounds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void playAbilitySound() {
        if (abilitySounds.Length <= 0) return;
        int index = Random.Range(0, abilitySounds.Length);
        audioSource.PlayOneShot(abilitySounds[index]);
    }
}
