using System.Collections;
using System;
using UnityEngine;

public class CoreTimer : Singleton<CoreTimer>
{
    [SerializeField] private float PulseTime;
    private int CurrentPulse;

    private bool Running;
    public void TurnTimerOn() => Running = true;
    public void TurnTimerOff() => Running = false;

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
