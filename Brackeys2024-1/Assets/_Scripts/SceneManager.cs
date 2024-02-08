using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Managers
{
    public class SceneManager : Singleton<SceneManager>
    {
        [Serializable]
        public enum Scenes
        {
            Title,
            Game1,
            Game2,
        }
        

        /// <summary>
        /// Load given Scenes
        /// </summary>
        /// <param name="scene">Build Index of Scene</param>
        public void LoadScene(int sceneBuildIndex)
        {
            LoadScene((Scenes)sceneBuildIndex);
        }
        
        public void LoadScene(Scenes scene)
        {
            int sceneIndex = (int)scene;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
        }

        public void QuitToMenu()
        {
            LoadScene(Scenes.Title);
        }

        public void Quit()
        {
            #if UNITY_EDITOR
            // Code to run only in the Unity Editor
            EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }

    }
}

