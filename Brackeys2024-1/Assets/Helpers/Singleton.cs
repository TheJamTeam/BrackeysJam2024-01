using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public static bool Exists => instance != null;

    private static T instance;
	public static T Instance => instance;

    protected virtual void Awake()
    {
        Initialize();
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void Initialize()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this as T;
    }
}
