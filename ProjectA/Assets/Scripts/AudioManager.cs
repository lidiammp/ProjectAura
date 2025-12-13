using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // audio source
    [Header("------------- Audio Source ------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [Header("------------- Sounds------------")]

    public Sound[] musicSounds, sfxSounds;
    public static AudioManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        // persistance
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


    }
    void Start()
    {
        PlayMusic("MainMenu");
    }
    
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name==name);
        if(s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {   
            if(musicSource.clip == s.clip) return;
            musicSource.clip = s.clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name==name);
        if(s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {   
            SFXSource.PlayOneShot(s.clip);
        }
    }
    
}
