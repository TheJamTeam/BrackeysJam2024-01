using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

[Serializable]
public struct Sound
{
    public string name;
    public AudioClip clip;
    public float volumePercent;
}

public class AudioManager : Singleton<AudioManager>
{
    public bool masterMute = false;
    public float masterVolume;
    public AudioMixer audioMixer;

    [Header("Music")]
    public bool musicMute;
    public float musicVolume;
    public AudioMixerGroup musicMix;
    private AudioSource musicSource;
    public bool playOnStart;
    private Sound currentMusic;
    public string startingMusic;
    public List<Sound> music = new List<Sound>();

    [Header("Sound Effects")]
    public bool sfxMute = false;
    public float sfxVolume;
    public AudioMixerGroup sfxMix;
    public List<Sound> sounds = new List<Sound>();
    private List<AudioSource> activeSources = new List<AudioSource>();

    [Header("Dialogue")]
    public bool dialogueMute;
    public float dialogueVolume;
    public AudioMixerGroup dialogueMix;
    private AudioSource dialogueSource;
    public List<Sound> dialogue = new List<Sound>();


    private void Start()
    {
        if(playOnStart)
            PlayMusic(startingMusic);

        //Reset volumes to default.
        ChangeMasterVol(masterVolume);
        ChangeSfxVol(sfxVolume);
        ChangeMusicVol(musicVolume);
        ChangeDialogueVol(dialogueVolume);
    }
    
    public void PlaySound(string soundToPlay)
    {
        Sound sound = sounds.Find(s => s.name == soundToPlay);

        if (sound.clip == null)
        {
            Debug.Log("Sound not found: " + soundToPlay);
            return;
        }

        PlaySound(sound);
    }
    
    public void PlaySound(Sound soundToPlay)
    {
        if (masterMute || sfxMute)
            return;
        
        if (activeSources.Count >= 5)
            return;

        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = sfxMix;

        source.clip = soundToPlay.clip;
        source.volume = soundToPlay.volumePercent * sfxVolume;
        source.Play();

        activeSources.Add(source);
    }

    public void PlayMusic(string musicToPlay)
    {
        Sound sound = music.Find(s => s.name == musicToPlay);

        if (sound.clip == null)
        {
            Debug.Log("Music not found: " + musicToPlay);
            return;
        }
        
        PlayMusic(sound);
    }
    
    public void PlayMusic(Sound musicToPlay)
    {
        if (masterMute || sfxMute)
            return;
        
        if (activeSources.Count >= 5)
            return;

        if (!musicSource)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.outputAudioMixerGroup = musicMix;
        }
        
        currentMusic = musicToPlay;
        musicSource.clip = currentMusic.clip;
        musicSource.volume = currentMusic.volumePercent * musicVolume;
        
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayDialogue(string dialogueToPlay)
    {
        Sound sound = dialogue.Find(s => s.name == dialogueToPlay);

        if (sound.clip == null)
        {
            Debug.Log("Dialogue not found: " + dialogueToPlay);
            return;
        }

        PlayDialogue(sound);
    }

    public void PlayDialogue(Sound dialogueToPlay)
    {
        if (masterMute || dialogueMute)
            return;

        if (!dialogueSource)
        {
            dialogueSource = gameObject.AddComponent<AudioSource>();
            dialogueSource.outputAudioMixerGroup = dialogueMix;
        }

        dialogueSource.clip = dialogueToPlay.clip;
        dialogueSource.volume = dialogueToPlay.volumePercent * dialogueVolume;
        dialogueSource.Play();

        activeSources.Add(dialogueSource);
    }

    //Audio Clean-up
    private void Update()
    {
        // Clean up finished sounds
        for (int i = activeSources.Count - 1; i >= 0; i--)
        {
            if (!activeSources[i].isPlaying)
            {
                Destroy(activeSources[i]);
                activeSources.RemoveAt(i);
            }
        }
    }

    public void ChangeMasterVol(float sliderValue)
    {
        masterVolume = sliderValue;
        audioMixer.SetFloat("MasterVol", Mathf.Log10(masterVolume) * 20);
    }
    
    public void ChangeSfxVol(float sliderValue)
    {
        sfxVolume = sliderValue; 
        audioMixer.SetFloat("SfxVol", Mathf.Log10(sfxVolume) * 20);
    }
    
    public void ChangeMusicVol(float sliderValue)
    {
        musicVolume = sliderValue; 
        audioMixer.SetFloat("MusicVol", Mathf.Log10(musicVolume) * 20);
    }

    public void ChangeDialogueVol(float sliderValue)
    {
        dialogueVolume = sliderValue;
        audioMixer.SetFloat("DialogueVol", Mathf.Log10(dialogueVolume) * 20);
    }
}
