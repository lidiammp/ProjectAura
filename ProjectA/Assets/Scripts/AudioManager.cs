using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection;

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
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
        if (instance != null && instance != this)
        {
             //if there is destroy it
            Destroy(gameObject);
            return;
        }
        //if theres no instance of this orig, create a new one
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "SplashScreen")
        {
            PlayMusic("MainMenu");
        }
        
    }
    
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name==name);
        if(s == null)
        {
            UnityEngine.Debug.Log("Sound Not Found");
        }
        else
        {   
            if(musicSource.clip == s.clip) return;
            musicSource.clip = s.clip;
            musicSource.loop = true;
            SFXSource.pitch = s.pitch;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name==name);
        if(s == null)
        {
            UnityEngine.Debug.Log("Sound Not Found");
        }
        else
        {   
            SFXSource.loop = false;
            SFXSource.pitch = s.pitch;
            SFXSource.PlayOneShot(s.clip, s.volume);
        }
    }

    public void PlaySFXRandom(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name==name);
        if(s == null)
        {
            UnityEngine.Debug.Log("Sound Not Found");
        }
        else
        {   
            float originalPitch = SFXSource.pitch;
            SFXSource.loop = false;
            SFXSource.pitch = UnityEngine.Random.Range(s.pitchStart, s.pitchEnd);
            SFXSource.PlayOneShot(s.clip, s.volume);
            SFXSource.pitch = originalPitch;
        }
    }

    public void PlayAudioSource(string name, AudioSource source)
    {
        //find string name that is same as the sounds name
        Sound s = Array.Find(musicSounds, x => x.name==name);
        if(s == null)
        {
            UnityEngine.Debug.Log("Sound "+name+"not found");
        }
        else
        {
            //uses inputed audiosource
            source.loop = true;
            source.clip = s.clip;
            source.pitch = s.pitch;
            source.Play();
        }
    }

    public void PlaySFXAudioSource(string name, AudioSource source)
    {
        //find string name that is same as the sounds name
        Sound s = Array.Find(sfxSounds, x => x.name==name);
        if(s == null)
        {
            UnityEngine.Debug.Log("Sound "+name+" not found");
        }
        else
        {
            //uses inputed audiosource
            float originalPitch = source.pitch;
            source.loop = false;
            source.pitch = UnityEngine.Random.Range(s.pitchStart, s.pitchEnd);
            source.PlayOneShot(s.clip, s.volume);
            source.pitch = originalPitch;
        }
    }
    void OnDestroy()
    {
        instance = null;
    }

    
}
