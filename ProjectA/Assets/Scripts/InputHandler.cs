using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public enum InputMode{Keyboard, Controller, Touch}
    private InputMode currentInputMode;
    private InputMode lastFrameInputMode;
    private event Action<InputMode> OnInputModeChanged;

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
        

    }
}
