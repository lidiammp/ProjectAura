using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


public class Splashscreenmanager2 : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 0f;

    // Call this when the Quit button is pressed
    public void OnButtonPressed()
    {
        EventSystem.current.SetSelectedGameObject(null);

        StartCoroutine(QuitAfterAnimation());
    }

    IEnumerator QuitAfterAnimation()
    {
        // Play your transition animation
        transition.SetTrigger("Start");

        // Wait for the animation to finish
        yield return new WaitForSeconds(transitionTime);

        // Quit the game
       
        Application.Quit();
    }
}
