using System.Collections;
using System;
using UnityEngine;

public class CoreTimer : Singleton<CoreTimer>
{
    [SerializeField] private float PulseTime;
    private int PulsesSinceStart;
    private float LastPulseTime;
    public float PulseFill => (Time.time - LastPulseTime) / PulseTime;

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
            PulsesSinceStart++;
            LastPulseTime = Time.time;
        }
    }
}
