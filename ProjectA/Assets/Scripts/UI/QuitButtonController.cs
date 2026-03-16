using System.Collections;
using UnityEngine;

public class QuitButtonController : MonoBehaviour
{
    [Header("Animation Settings")]
    public Animator imageAnimator;        // Animator on your UI Image
    public string triggerName = "Quit";   // Trigger parameter in Animator
    public float transitionTime = 1f;     // Length of the animation

    // Call this from your Quit button OnClick()
    public void OnQuitButtonPressed()
    {
        StartCoroutine(PlayAnimationAndQuit());
    }

    IEnumerator PlayAnimationAndQuit()
    {
        // Trigger the animation
        imageAnimator.SetTrigger("Quit");

        // Wait for the animation to finish
        yield return new WaitForSeconds(transitionTime);

#if UNITY_EDITOR
        // Stop play mode in the editor (useful while testing)
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the game in a build
        Application.Quit();
#endif
    }
}

