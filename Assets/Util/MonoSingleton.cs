﻿using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static readonly object _lock = new object();
    private static T _instance;
    public static T Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));
                    if(_instance == null)
                    {
                        Debug.LogWarning("You're trying to access a singleton that's not currently loaded!");
                    }
                }
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
}
