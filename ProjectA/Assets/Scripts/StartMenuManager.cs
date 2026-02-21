using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject playButton; // first selected for controller
    public GameObject quitButton;

    public PlayButtonController playButtonController;
    public QuitButtonController quitButtonController;
    private bool usingController = false;


    void Start()
    {
        //IF CONTTROLER ERASE CURSOR AND SELECT PLAY    
        if (IsControllerConnected() && !IsMouseInput())
        {
            usingController = true;
            StartCoroutine(SelectOnNextFrame(playButton));
            Cursor.visible = false;
        }
        // if mouse turn bool off and unhighlight the selected button
        else
        {
            usingController = false;
            Cursor.visible = true;
            EventSystem.current.SetSelectedGameObject(null); // no highlight
        }
        AudioManager.instance.PlayMusic("MainMenu");
    }

    void Update()
    {
        HandleInputSwitching();

        // Optional: add controller Cancel behavior if needed
    }

    #region Button Functions
    public void PlayGame()
    {
        playButtonController.OnPlayButtonPressed();
    }

    public void QuitGame()
    {
        quitButtonController.OnQuitButtonPressed();
    }
    #endregion

    #region Input Handling
    private void HandleInputSwitching()
    {
        // Controller input
        if (IsControllerInput())
        {
            if (!usingController)
            {
                usingController = true;
                Cursor.visible = false;
                // Select Play button by default if nothing selected
                if (EventSystem.current.currentSelectedGameObject == null)
                    EventSystem.current.SetSelectedGameObject(playButton);
            }
        }

        // Mouse input
        if (IsMouseInput())
        {
            if (usingController)
            {
                usingController = false;
                Cursor.visible = true;

                // Force-clear selection immediately on first mouse movement
                EventSystem.current.SetSelectedGameObject(null);
            }
        }

    }

    private bool IsControllerInput()
    {
        // buttons
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || // A
            Input.GetKeyDown(KeyCode.JoystickButton1) || // B
            Input.GetKeyDown(KeyCode.JoystickButton7))   // Start
            return true;

        // bticks/D-pad
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.2f ||
            Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.2f)
            return true;

        return false;
    }

    //detect the mouse inputs
    private bool IsMouseInput()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            return true;
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            return true;
        return false;
    }

    //if bool func to check if controller is connected
    private bool IsControllerConnected()
    {
        string[] joysticks = Input.GetJoystickNames();
        foreach (var j in joysticks)
        {
            if (!string.IsNullOrEmpty(j) && (j.ToLower().Contains("xbox") || j.ToLower().Contains("controller")))
                return true;
        }
        return false;
    }
    #endregion
    
    //override all the unity stuff by waitng a frame after
    private IEnumerator SelectOnNextFrame(GameObject button)
    {
        yield return null; // wait 1 frame
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }


}
