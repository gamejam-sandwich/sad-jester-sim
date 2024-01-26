using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [SerializeField] private string currentTheme;

    public Sound[] soundArray;

    public static AudioManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Multiple instances of AudioManager detected");
        }

        instance = this;

        foreach (Sound s in soundArray)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void PlaySound(string desiredSoundName)
    {
        foreach (Sound s in soundArray)
        {
            if (s.name == desiredSoundName)
            {
                s.source.Play();
                break;
            }
        }
    }

    public void StopSound(string desiredSoundName)
    {
        foreach(Sound s in soundArray)
        {
            if(s.name == desiredSoundName)
            {
                if (!s.source.isPlaying)
                {
                    Debug.LogWarning("Not playing");
                    return;
                }
                s.source.Stop();
            }
        }
    }

    public void SwitchTheme(string desiredSoundName)
    {
        if(currentTheme != null)
        {
            StopSound(currentTheme);
        }

        currentTheme = desiredSoundName;
        foreach(Sound s in soundArray)
        {
            if(s.name == currentTheme && !s.source.isPlaying)
            {
                s.source.Play();
            }
        }
    }
}
