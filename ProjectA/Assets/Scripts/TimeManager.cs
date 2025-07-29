using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public float slowDownFactor = 0.05f;
    public float slowDownDuration = 2f;
    bool waiting;
    void Update()
    {
        // //slowly raise time
        // Time.timeScale += (1f / slowDownDuration) * Time.unscaledDeltaTime;
        // //clamp it from 0 -> 1
        // Time.timeScale = Mathf.Clamp01(Time.timeScale);

    }
    public void SlowMo()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void Stop(float duration)
    {
        if (waiting)
            return;
        Time.timeScale = 0;
        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration)
    {
        waiting = true;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
        waiting = false;
    }
}
