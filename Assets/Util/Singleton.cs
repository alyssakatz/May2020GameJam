using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
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
                        Debug.LogWarning("You're trying to access a singleton that's not set in editor!");
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
