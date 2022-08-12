using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip pickUp_Sound, dead_Sound;
    void Awake()
    {
        MakeInstance();
    }

    void MakeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void Play_pickUp_Sound()
    {
        AudioSource.PlayClipAtPoint(pickUp_Sound,transform.position);
    }

    public void Play_dead_Sound()
    {
        AudioSource.PlayClipAtPoint(dead_Sound,transform.position);
    }

    
}
