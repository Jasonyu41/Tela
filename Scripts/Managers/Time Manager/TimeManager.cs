using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    bool isStartTiming = false;
    float _pauseTime;
    bool _timeIsFrame;
    float timer;

    public static void PauseGame(float pauseTime, bool timeIsFrame)
    {
        Time.timeScale = 0;
        
        Instance.isStartTiming = true;
        Instance._pauseTime = pauseTime;
        Instance._timeIsFrame = timeIsFrame;
        Instance.timer = 0;
    }
    
    void Update()
    {
#if UNITY_EDITOR        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Time.timeScale = 1f;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Time.timeScale = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Time.timeScale = 0.25f;
        }
#endif        

        if (isStartTiming)
        {
            timer += _timeIsFrame ? 1 : Time.unscaledDeltaTime;

            if (timer > _pauseTime)
            {
                Time.timeScale = 1;
                isStartTiming = false;
            }
        }
    }
}
