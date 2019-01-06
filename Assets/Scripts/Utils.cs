using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Timer
{
    public bool Complete { get; private set; }

    public Timer()
    {
        Complete = false;
    }

    public void StartCountDown(float seconds, System.Action on_complete)
    {
        CoroutineHandler.Start(CountDown(seconds, on_complete));
    }

    IEnumerator CountDown(float seconds, System.Action on_complete)
    {
        yield return new WaitForSeconds(seconds);

        if (on_complete != null) on_complete();

        Complete = true;
    }
}