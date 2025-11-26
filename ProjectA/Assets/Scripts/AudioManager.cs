using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    AudioManager instance;
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            //if theres no instance of this orig, create a new one
            instance = this;
        }
        else
        {
            //if there is destroy it
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }
    void Start()
    {
        Play("Passerby");
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound=>sound.name == name);
        s.source.Play();
    }
}
