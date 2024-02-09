using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicComponent : MonoBehaviour
{
    private AudioManager _audioManager;
    public List<Sound> music;
    public bool playFirstTrackOnStart;

    void Start()
    {
        if (!_audioManager)
        {
            _audioManager = AudioManager.Instance;
        }

        if (playFirstTrackOnStart)
        {
            StartMusic(0);
        }
    }
    public void StartMusic(int musicIndex)
    {
        if (musicIndex < music.Count && musicIndex >= 0)
        {
            StartMusic(music[musicIndex]);
        }
        else
        {
            Debug.LogError(string.Format("%s: Could not find music index: %d", gameObject.name,  musicIndex));
        }
    }
    
    public void StartMusic(string musicName)
    {
        foreach(Sound musicItem in music)
        {
            if (string.Equals(musicItem.name.ToLower(), musicName.ToLower(), StringComparison.Ordinal))
            {
                StartMusic(musicItem);
                return;
            }
        }
        
        Debug.LogError(string.Format("%s: Could not find sound: %s", gameObject.name,  musicName));
    }
    
    public void StartMusic(Sound musicToPlay)
    {
        _audioManager.PlayMusic(musicToPlay);
    }
}
