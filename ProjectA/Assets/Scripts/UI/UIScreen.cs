using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIScreen : MonoBehaviour
{
    [Header("Screen Settings")]
    [SerializeField] protected GameObject _firstSelected;
    [SerializeField] private bool _hideOnDisable = true;

    // References
    protected CanvasGroup _canvasGroup;

    // ==============================================================
    // Public Events
    // ==============================================================

    public delegate void OnOpen();
    public event OnOpen onOpen;
    public delegate void OnClose();
    public event OnClose onClose;

    // ==============================================================
    // Public Properties
    // ==============================================================

    public GameObject firstselected { get => _firstSelected; }
    public bool isDisplaying { get => _canvasGroup.alpha > 0f; }
    public bool isInteractable { get => _canvasGroup.interactable; }

    // ==============================================================
    // Set up
    // ==============================================================

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    // ==============================================================
    // Public Methods
    // ==============================================================

    /// <summary>
    /// Opens screen by displaying it and setting it as interactable.
    /// </summary>
    public virtual void open()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
        onOpen?.Invoke();
    }
    /// <summary>
    /// Closes screen by disabling and hiding content.
    /// </summary>
    public virtual void close()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0f;
        onClose?.Invoke();
    }
    /// <summary>
    /// Disables interactablity
    /// </summary>
    public virtual void disable(bool blocksraycasts = false)
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = blocksraycasts;

        if (_hideOnDisable)
            _canvasGroup.alpha = 0f;
    }
    /// <summary>
    /// Enables interactablity
    /// </summary>
    public virtual void enable()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
    }

    // ==================================================================================
    // Button Events
    // ==================================================================================

    public virtual void OnClick_Open()
    {
        UIManager.OpenScreen(this);
    }
    public virtual void OnClick_CloseCurrentScreen()
    {
        UIManager.CloseScreen();
    }
    public virtual void OnClick_CloseAllScreens()
    {
        UIManager.CloseAll();
    }
    public void OnClick_DebugLogMessage(string message)
    {
        Debug.Log(message);
    }
}
