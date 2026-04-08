using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseInputHandler: MonoBehaviour
{
    [Header("Panels")]
    public GameObject darkPanel;       // Main pause menu with Resume/Quit/Controls
    public GameObject controlsPanel;   // Controls subpanel only

    [Header("First Selected Buttons")]
    public GameObject resumeButton;        // Selected when darkPanel opens
    public GameObject controlsBackButton;  // Only selected button in ControlsPanel

    [Header("Player References")]
    public MouseLook mouselookReference;
    public PlayerMovement playerMoveReference;
    public Animator playerAnimator;
    private bool usingController = false;

    void Start()
    {
        Resume();
    }

    void Update()
    {
        // Toggle pause
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (GameManager.instance.CurrentState == GameState.Paused)
                GameManager.instance.SetState(GameState.Playing);
            else
                GameManager.instance.SetState(GameState.Paused);
        }

        HandleInputSwitching();

        // Controller "Back" button in ControlsPanel
        if (usingController && controlsPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton1)) // B button
            {
                CloseControls();
            }
        }

        UpdateCursorState();
    }

    #region Pause / Resume
    public void Resume()
    {
        
    }

    private void Pause()
    {
        darkPanel.SetActive(true);
        controlsPanel.SetActive(false);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  

        playerAnimator.enabled = false;
        playerMoveReference.enabled = false;
        mouselookReference.LockMouse();


        if (IsControllerConnected())
        {
            usingController = true;
            StartCoroutine(SelectOnNextFrame(resumeButton));

        }
        else
        {
            usingController = false;
        }
    }
    #endregion

    #region Controls Panel
    public void OpenControls()
    {
        controlsPanel.SetActive(true);
        darkPanel.SetActive(false);

        // Always select the Back button if controller is active
        if (IsControllerConnected())
        {
            usingController = true;
            StartCoroutine(SelectOnNextFrame(controlsBackButton));
        }
    }


    public void CloseControls()
    {
        controlsPanel.SetActive(false);
        darkPanel.SetActive(true); // go back to main dark panel

        if (usingController)
        {
            StartCoroutine(SelectOnNextFrame(resumeButton));
        }
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


                if (controlsPanel.activeSelf)
                    EventSystem.current.SetSelectedGameObject(controlsBackButton);
                else
                    EventSystem.current.SetSelectedGameObject(resumeButton);
            }
        }

        // Mouse input
        if (IsMouseInput())
        {
            if (usingController)
            {
                usingController = false;

                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    private bool IsControllerInput()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) || // A
            Input.GetKeyDown(KeyCode.JoystickButton1) || // B
            Input.GetKeyDown(KeyCode.JoystickButton7))   // Start
            return true;

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.2f ||
            Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.2f)
            return true;

        return false;
    }

    private bool IsMouseInput()
    {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            return true;
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            return true;
        return false;
    }

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

    private IEnumerator SelectOnNextFrame(GameObject button)
    {
        yield return null; // wait 1 frame
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }

    private void UpdateCursorState()
    {
        if (GameManager.instance.CurrentState == GameState.Playing) 
        {
            // Gameplay state
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if(GameManager.instance.CurrentState == GameState.Paused)
        {
            // Paused state
            if (usingController)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
    public void LoadHome()
    {
        // Make sure time scale is normal if you paused the game
        Time.timeScale = 1f;

        // Replace "HomeScene" with the name of your scene
        SceneManager.LoadSceneAsync("SplashScreen");
    }
}
