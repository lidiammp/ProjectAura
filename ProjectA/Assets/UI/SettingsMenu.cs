using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{

    public bool GameIsPaused = false;
    public GameObject DarkPanel;
    [SerializeField] private MouseLook mouselookrefrence;
    [SerializeField] private PlayerMovement playermoveref;
    [SerializeField] private Animator playerAnimator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Resume()
    {

        DarkPanel.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        playerAnimator.enabled = true;
        playermoveref.enabled = true;
        mouselookrefrence.UnlockMouse();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
    void Pause()
    {
        DarkPanel.SetActive(true);
        playerAnimator.enabled = false;
        Time.timeScale = 0f;
        GameIsPaused = true;
        
        playermoveref.enabled = false;
        mouselookrefrence.LockMouse();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void LoadHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SplashScreen");
    }
    
}