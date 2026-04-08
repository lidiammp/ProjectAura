using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    private float timer = 0;
    private float currentTimer = 0;

    private void OnEnable()
    {
        EventDispatcher.instance.AddListener<PauseEvent>(Pause);
        EventDispatcher.instance.AddListener<ResumeEvent>(Resume);
    }

    private void OnDisable()
    {
        EventDispatcher.instance.RemoveListener<PauseEvent>(Pause);
        EventDispatcher.instance.RemoveListener<ResumeEvent>(Resume);
    }
    void Update()
    {
        //check if theres a timer active and after it finishes, reset time scale to 1
        if (currentTimer > 0)
        {
            timer += Time.unscaledDeltaTime;
            if(timer >= currentTimer) {
                Time.timeScale = 1f;
                timer = 0;  
                currentTimer = 0;
            }
        }

    }

    //sets scalee of time for a duration, after the duration is over, 
    //time scale is reset to 1
    //if duration is less than 0, time scale is set to 0 and will not reset until Resume is called
    private void Pause(PauseEvent eventData)
    {
        if (eventData.duration >= 0)
        {
            currentTimer = eventData.duration;
            Time.timeScale = eventData.timeScale;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }



    //reset timer and resume normal timescale
    private void Resume(ResumeEvent eventData)
    {
        Time.timeScale = 1f;
        timer = 0;
        currentTimer = 0;
    }

    // wat if one slowmo is called while another is still active? 
    // for now we will just override the current one with the new one, but we can implement a queue system later if we want to.
}
