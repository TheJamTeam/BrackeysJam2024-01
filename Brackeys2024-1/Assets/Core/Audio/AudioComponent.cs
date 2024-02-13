using System;
using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class AudioComponent : MonoBehaviour
{
    private AudioManager _audioManager;
    
    public List<Sound> sounds;

    private void Start()
    {
        if (_audioManager is null)
        {
            _audioManager = AudioManager.Instance;
        }
    }

    public void PlaySound(int soundIndex)
    {
        if (soundIndex < sounds.Count && soundIndex >= 0)
        {
            PlaySound(sounds[soundIndex]);
        }
        else
        {
            Debug.LogError(string.Format("%s: Could not find sound index: %d", gameObject.name,  soundIndex));
        }
    }

    //Find and match string name then play the first sound.
    public void PlaySound(string soundName)
    {
        foreach(Sound sound in sounds)
        {
            if (string.Equals(sound.name.ToLower(), soundName.ToLower(), StringComparison.Ordinal))
            {
                PlaySound(sound);
                return;
            }
        }
        
        Debug.LogError(string.Format("%s: Could not find sound: %s", gameObject.name,  soundName));
    }

    public void PlaySound(Sound soundToPlay)
    {
        
        _audioManager.PlaySound(soundToPlay);
    }

}
