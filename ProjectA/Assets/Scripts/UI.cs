using UnityEngine;
public class UI : MonoBehaviour
{
    public void CloseWindow(GameObject window)
    {

        CanvasGroup canvasGroup = window.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void OpenWindow(GameObject window)
    {

        CanvasGroup canvasGroup = window.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}