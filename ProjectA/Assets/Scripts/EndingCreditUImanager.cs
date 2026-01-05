using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class EndingCreditUImanager : MonoBehaviour
{
    VideoPlayer videoPlayer;
    public double videoLength = 0;
    public GameObject restartButton;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoLength = videoPlayer.clip.length;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (videoPlayer != null && videoPlayer.isPrepared)
        {
            // Get the current time of the video clip in seconds
            double currentTime = videoPlayer.time;
            if (currentTime > videoLength - 2)
            {
                restartButton.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            // You can then use this 'currentTime' value as needed,
            // for example, to display it in a UI Text element or trigger events.
            Debug.Log($"Current Video Time: {currentTime} seconds");
        }
    }
}
