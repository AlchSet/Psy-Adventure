using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    AudioSource sfx;
    // Start is called before the first frame update
    void Start()
    {
        sfx = GetComponent<AudioSource>();
    }

    public void Play()
    {
        if(!sfx.isPlaying)
        {
            sfx.Play();
        }
    }

    public void Stop()
    {
        sfx.Stop();
    }
}
