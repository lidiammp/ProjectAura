using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UIManager
{
    // Stack data
    private static Stack<UIScreen> _screensStack = new();
    private static UIScreen _currentScreen;

    // ==================================================================================
    // Public Methods
    // ==================================================================================
    public delegate void OnStackEmpty();
    /// <summary>
    /// Called when UI stack becomes empty.
    /// </summary>
    public static event OnStackEmpty onStackEmpty;

    public delegate void OnStartDisplay();
    /// <summary>
    /// Called when the first UIScreen is displayed.
    /// </summary>
    public static event OnStartDisplay onStartDisplay;

    public delegate void OnCloseScreen();
    /// <summary>
    /// Called when any sceen is closed
    /// </summary>
    public static event OnCloseScreen onCloseScreen;

    // ==================================================================================
    // Public Methods
    // ==================================================================================
    /// <summary>
    /// Displays a UIScreen. NOTE: Any screen that is already open 
    /// will be disabled and pushed onto the UI stack. 
    /// </summary>
    /// <param name="newscreen"></param>
    public static void OpenScreen(UIScreen newscreen)
    {
        // Disable the current screen and pushing it onto the stack
        _currentScreen?.disable();
        if (_currentScreen != null) 
            _screensStack.Push(_currentScreen);

        _currentScreen = newscreen;
        _currentScreen.open();

        // Update system information
        EventSystem eventSystem = EventSystem.current;
        if (eventSystem != null)
        {
            eventSystem.firstSelectedGameObject = _currentScreen.firstselected;
            eventSystem.SetSelectedGameObject(_currentScreen.firstselected);
        }
        else
            Debug.LogWarning("No eventsystem was detected");

        if (_screensStack.Count == 0) 
            onStartDisplay?.Invoke();

        Debug.Log("Open screen " + newscreen.name + " | Stack size = " + _screensStack.Count);
    }
    /// <summary>
    /// Closes the current UIScreen and pops the previous UIScreen from the UI stack.
    /// NOTE: If there are no more UIScreens left on UI stack, then the onStackEmpty callback will be called.
    /// </summary>
    public static void CloseScreen()
    {
        _currentScreen?.close();

        if (_currentScreen != null) onCloseScreen?.Invoke();

        if (_screensStack.TryPop(out UIScreen screen))
        {
            _currentScreen = screen;
            _currentScreen.enable();
        }
        else
        {
            _currentScreen = null;
            onStackEmpty?.Invoke();
        }

        Debug.Log("Close screen | Stack size = " + _screensStack.Count + " and current screen is null = " + (_currentScreen == null));
    }
    /// <summary>
    /// Closes all UIScreens on UI stack.
    /// </summary>
    public static void CloseAll()
    {
        while (_screensStack.Count > 0)
            CloseScreen();
        CloseScreen();
    }
}
