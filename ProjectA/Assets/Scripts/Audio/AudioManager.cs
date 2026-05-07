using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    // audio source
    [Header("------------- Audio Source ------------")]
    [SerializeField] AudioSource musicSource;
    [Header("------------- Sounds------------")]
    
    public AudioMixerGroup sfxGroup;
    public Sound[] musicSounds, sfxSounds;
    public static AudioManager instance;
    private AudioSource[] pool;
    public int poolSize = 10;
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

        pool = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = new GameObject("AudioSource_" + i);
            go.transform.parent = transform;

            AudioSource src = go.AddComponent<AudioSource>();
            src.playOnAwake = false;
            src.spatialBlend = 1f; // 3D sound
            src.outputAudioMixerGroup = sfxGroup;
            pool[i] = src;
        }
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
            musicSource.pitch = s.pitch;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        AudioSource SFXSource = GetFreeSource();
       
        Sound s = Array.Find(sfxSounds, x => x.name==name);
        if(s == null)
        {
            UnityEngine.Debug.Log("Sound Not Found");
        }
        else
        {   
            SFXSource.loop = false;
            SFXSource.pitch = s.pitch;
            SFXSource.clip = s.clip;
            SFXSource.volume = s.volume;
            SFXSource.Play();
        }
    }

    public void PlaySFXRandom(string name)
    {
        AudioSource SFXSource = GetFreeSource();
        Sound s = Array.Find(sfxSounds, x => x.name==name);
        if(s == null)
        {
            UnityEngine.Debug.Log("Sound Not Found");
        }
        else
        {   
            SFXSource.loop = false;
            SFXSource.pitch = UnityEngine.Random.Range(s.pitchStart, s.pitchEnd);
            SFXSource.clip = s.clip;
            SFXSource.volume = s.volume;
            SFXSource.Play();
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

    public AudioSource GetFreeSource()
    {
        foreach (var source in pool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        return pool[0]; //use the first if all are busy
    }
    void OnDestroy()
    {
        instance = null;
    }

    
}
