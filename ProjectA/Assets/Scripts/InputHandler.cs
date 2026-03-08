using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public enum InputMode{Keyboard, Controller, Touch}
    public InputMode currentInputMode;
    private InputMode lastFrameInputMode;
    public event Action<InputMode> OnInputModeChanged;
    public static InputHandler instance;
    void Awake()
    {
        // persistance
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        //if theres no instance of this orig, create a new one
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentInputMode = InputMode.Keyboard;
    }

    // Update is called once per frame
    void Update()
    {
        
        currentInputMode = ProcessInputMode();
        if(lastFrameInputMode != currentInputMode)
        {
            OnInputModeChanged?.Invoke(currentInputMode);
        }
        lastFrameInputMode = ProcessInputMode();
    }

    private InputMode ProcessInputMode()
    {
        if(Application.platform == RuntimePlatform.Android || 
        Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return InputMode.Touch;
        }

        if (Input.GetJoystickNames().Length == 0)
        {
            return InputMode.Keyboard;
        }
        if (Input.anyKeyDown)
        {
            if(Input.GetKeyDown(KeyCode.JoystickButton0)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton1)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton2)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton3)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton4)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton5)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton6)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton7)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton9)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton10)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton11)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton12)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton13)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton14)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton15)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton16)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton17)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton18)) return InputMode.Controller;
            else if(Input.GetKeyDown(KeyCode.JoystickButton19)) return InputMode.Controller;
            else return InputMode.Keyboard;
        }
        
        return currentInputMode;
        
    }
}
