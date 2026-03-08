using UnityEngine;

public class InputTooltipManager : MonoBehaviour
{
    [SerializeField] private UI controllerUI;
    [SerializeField] private UI keyboardUI;

    void OnEnable()
    {
        if (InputHandler.instance != null)
        {
            InputHandler.instance.OnInputModeChanged += UpdateUI;
            UpdateUI(InputHandler.instance.currentInputMode);   
        }
    }
    void OnDisable()
    {
        InputHandler.instance.OnInputModeChanged -=  UpdateUI;
    }
    void UpdateUI(InputHandler.InputMode mode)
    {
        if(mode == InputHandler.InputMode.Controller)
        {
            ShowControllerUI();
        }else if( mode == InputHandler.InputMode.Keyboard)
        {
            ShowKeyboardUI();
        }
    }

    void ShowControllerUI()
    {
        keyboardUI.CloseWindow(keyboardUI.gameObject);
        controllerUI.OpenWindow(controllerUI.gameObject);
    }
    void ShowKeyboardUI()
    {
        controllerUI.CloseWindow(controllerUI.gameObject);
        keyboardUI.OpenWindow(keyboardUI.gameObject);
        
    }
}