using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

[Serializable]
public struct Sound
{
    #region Equality Overrides

    public static bool operator ==(Sound s1, Sound s2) 
    {
        return s1.Equals(s2);
    }

    public static bool operator !=(Sound s1, Sound s2) 
    {
        return !s1.Equals(s2);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (obj.GetType() != this.GetType()) return false;
        return this.clip == ((Sound)obj).clip;
    }
    
    public override int GetHashCode ()
    {
        return this.clip.GetHashCode ();
    }

    #endregion
    
    public string name;
    public AudioClip clip;
    public float volumePercent;
}

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer audioMixer;
    
    [Header("Master")]
    public bool masterMute = false;
    public float masterVolume;

    [Header("Music")]
    public bool musicMute;
    public float musicVolume;
    public AudioMixerGroup musicMix;
    private AudioSource musicSource;
    private Sound currentMusic;

    [Header("Sound Effects (Sfx)")]
    public bool sfxMute = false;
    public float sfxVolume;
    public AudioMixerGroup sfxMix;
    private List<AudioSource> _activeSources = new List<AudioSource>();

    #region Unity Functions

    private void Start()
    {
        //Reset volumes to saved values.
        ChangeMasterVol(masterVolume);
        ChangeSfxVol(sfxVolume);
        ChangeMusicVol(musicVolume);
    }
    
    //Audio Clean-up
    private void Update()
    {
        CleanUpSoundEffects();
    }

    #endregion

    #region Master

    public void ChangeMasterVol(float sliderValue)
    {
        masterVolume = sliderValue;
        audioMixer.SetFloat("MasterVol", Mathf.Log10(masterVolume) * 20);
    }

    #endregion
    
    #region Sound Effects (Sfx)

    public void ChangeSfxVol(float sliderValue)
    {
        sfxVolume = sliderValue; 
        audioMixer.SetFloat("SfxVol", Mathf.Log10(sfxVolume) * 20);
    }
    
    public void PlaySound(Sound soundToPlay)
    {
        if (masterMute || sfxMute)
            return;
        
        if (_activeSources.Count >= 5)
            return;

        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = sfxMix;

        source.clip = soundToPlay.clip;
        source.volume = soundToPlay.volumePercent * sfxVolume;
        source.Play();

        _activeSources.Add(source);
    }
    
    private void CleanUpSoundEffects()
    {
        // Clean up finished sounds
        for (int i = _activeSources.Count - 1; i >= 0; i--)
        {
            if (!_activeSources[i].isPlaying)
            {
                Destroy(_activeSources[i]);
                _activeSources.RemoveAt(i);
            }
        }
    }

    #endregion
    
    #region Music
    
    public void PlayMusic(Sound musicToPlay)
    {
        if (musicToPlay.clip is not null)
        {
            //Check if Muted
            if (masterMute || musicMute)
            {
                return;
            }

            //Don't play an already playing track
            if (currentMusic == musicToPlay)
            {
                Debug.Log(String.Format("Attempting to play a music '%s', but it was already playing!", musicToPlay.name));
                return;
            }

            //Initialize Music Source 
            if (musicSource is null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.outputAudioMixerGroup = musicMix;
            }
            
            //Update and play Track
            currentMusic = musicToPlay;
            musicSource.clip = currentMusic.clip;
            musicSource.volume = currentMusic.volumePercent * musicVolume;
        
            musicSource.loop = true;
            musicSource.Play();
        }
    }
    
    public void ChangeMusicVol(float sliderValue)
    {
        musicVolume = sliderValue; 
        audioMixer.SetFloat("MusicVol", Mathf.Log10(musicVolume) * 20);
    }

    #endregion
    
    
    
}
