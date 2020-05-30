using System.Collections;
using System;
using UnityEngine;

public class CoreTimer : Singleton<CoreTimer>
{
    [SerializeField] private float PulseTime;
    private int CurrentPulse;

    private bool _running;
    public bool Running
    {
        get
        {
            return _running;
        }
        set
        {
            _running = value;
        }
    }

    public event Action OnPulse;

    void Start()
    {
        Running = true;
    }

    IEnumerator Timer()
    {
        while(Running)
        {
            yield return new WaitForSeconds(PulseTime);
            OnPulse.Invoke();
            CurrentPulse++;
        }
    }
}
