using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : PersistentSingleton<AudioManager>
{
    public AudioClip itemSound;
    public AudioClip hitSound;
    public AudioClip attackSound;
    
    public void PlayItem()
    {
        GetComponent<AudioSource>().PlayOneShot(itemSound);
    }

    public void PlayHit()
    {
        GetComponent<AudioSource>().PlayOneShot(hitSound);
    }

    public void PlayAttack()
    {
        GetComponent<AudioSource>().PlayOneShot(attackSound);
    }
}
