using System;
using System.Collections.Generic;
using Core.Audio;
using UnityEngine;
using UnityEngine.Video;

namespace CustomScripts.Core
{
    [Serializable]
    public struct Video
    {
    
        #region Equality Overrides

        public static bool operator ==(Video v1, Video v2) 
        {
            return v1.Equals(v2);
        }

        public static bool operator !=(Video v1, Video v2) 
        {
            return !v1.Equals(v2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return this.filename == ((Video)obj).filename;
        }
    
        public override int GetHashCode ()
        {
            return this.filename.GetHashCode ();
        }

        #endregion

        [Tooltip("The clip filename and its extension. E.g. 'YourVideoClip.mp4'. \n This file must be stored inside '/Assets/StreamingAssets/'")]
        public string filename;
        [Tooltip("Should this clip loop on completion? This will override the play Sequence.")]
        public bool isLooping;
    }

    [RequireComponent(typeof(VideoPlayer))]
    [RequireComponent(typeof(AudioSource))]
    public class VideoComponent : MonoBehaviour
    {
        public List<Video> videoList;
        public bool playInSequence;
        public bool shouldLoopOnceFinishedAllVideos;
        public float volume;
        public bool playOnStart;
    
        private int currentVideoIndex;
        private VideoManager _videoManager;
        private VideoPlayer _videoPlayer;
        private AudioSource _audioSource;

        private void Awake()
        {
            _videoManager = FindObjectOfType<VideoManager>();
        }


        void Start()
        {
            //TODO Hook up the game start @Jeremy!
            OnStart();
        }

        // Start is called before the first frame update
        void OnStart()
        {
            //Init Audio Source
            if (!_audioSource)
            {
                _audioSource = GetComponent<AudioSource>();
            }

            _audioSource.volume = volume;
            _audioSource.outputAudioMixerGroup = AudioManager.Instance.sfxMix;
        
            //Initialize Video Player
            if (!_videoPlayer)
            {
                _videoPlayer = GetComponent<VideoPlayer>();
            }
        
            _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            _videoPlayer.SetTargetAudioSource(0, _audioSource);
            _videoPlayer.playOnAwake = false;
        
            if (playOnStart)
            {
                PlayVideo(currentVideoIndex);
            }
        }

        void OnVideoFinished()
        {
            if (playInSequence)
            {
                //Next index is valid?
                if (currentVideoIndex + 1 < videoList.Count)
                {
                    PlayVideo(currentVideoIndex + 1);
                
                }
                else //Reached end of Video List
                {
                    if (shouldLoopOnceFinishedAllVideos)
                    {
                        PlayVideo(0);
                    }
                    else
                    {
                        StopVideo(false);
                    }
                }
            }
            else
            {
                StopVideo(false);
            }
        
        }
    
        public void PlayVideo(int videoIndex)
        {
            if (videoIndex < 0 || videoIndex >= videoList.Count)
            {
                Debug.LogWarning($"{gameObject.name}: Cannot play video. Invalid Video Index ({videoIndex.ToString()})");
                return;
            }
        
            PlayVideo(videoList[videoIndex]);
        }

        void PlayVideo(Video videoToPlay)
        {
            _videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoToPlay.filename);
        
            //Check if its a valid file path on non-webGL platforms.
            if (Application.platform != RuntimePlatform.WebGLPlayer)
            {
                if (!System.IO.File.Exists(_videoPlayer.url))
                {
                    Debug.LogWarning($"{gameObject.name}: Cannot play video. Cannot find file ({_videoPlayer.url})");
                    return;
                }
            }
        
            currentVideoIndex = videoList.IndexOf(videoToPlay);
            _videoPlayer.prepareCompleted += VideoPrepared;
            _videoPlayer.Prepare();
            _videoPlayer.Play();
        }

        void VideoPrepared(VideoPlayer videoPlayer)
        {
            _videoPlayer.prepareCompleted -= VideoPrepared;

            if (videoList[currentVideoIndex].isLooping)
            {
                _videoPlayer.isLooping = true;
            }
            else
            {
                _videoPlayer.isLooping = false;
                Invoke(nameof(OnVideoFinished), (float)_videoPlayer.length);
            }
        }

        public void StopVideo(bool interrupt)
        {
            if (_videoPlayer.isPlaying ) 
            {
                _videoPlayer.Stop();
                //_videoPlayer.url = string.Empty;
                CancelInvoke(nameof(OnVideoFinished));
                if (!interrupt)
                {
                    _videoManager.FadeOut();
                }
            }
        }

        //TODO Hook up to Game.cs pause. @jeremy!
        void OnGamePause(bool isPaused)
        {
            if (isPaused)
            {
                _videoPlayer.Pause();
            }
            else
            {
                _videoPlayer.Play();
            }
        }
    }
}