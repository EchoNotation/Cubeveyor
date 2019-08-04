using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /*
     * 0: Grabbing/Releasing
     * 1: Error
     * 2: Lights
     * 3: Play/Rewind
     */
    public AudioSource[] sounds;
    public AudioClip rewind, play;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LightsSound()
    {
        if(sounds[2].isPlaying)
        {
            sounds[2].Stop();
        }
        sounds[2].Play();
    }

    public void PlayRewindSound(bool rewinding)
    {
        if(sounds[3].isPlaying)
        {
            sounds[3].Stop();
        }

        if(rewinding)
        {
            sounds[3].clip = rewind;
        }
        else
        {
            sounds[3].clip = play;
        }

        sounds[3].Play();
    }

    public void PlayErrorSound()
    {
        if(sounds[1].isPlaying)
        {
            sounds[1].Stop();
        }

        sounds[1].Play();
    }
}
