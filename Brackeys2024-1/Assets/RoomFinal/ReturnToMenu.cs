using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    public void EndGame()
    {
        /* Deload everything and reset to scene 0, somehow? */
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            SceneManager.UnloadSceneAsync(i);
        }
        SceneManager.LoadScene(0);
    }
}
