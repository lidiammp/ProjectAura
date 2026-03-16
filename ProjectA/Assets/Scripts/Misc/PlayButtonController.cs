using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButtonController : MonoBehaviour
{
    [Header("Animation Settings")]
    public Animator image1Animator;   // The animator on your UI Image
    public string triggerName = "Play"; // Trigger parameter in Animator
    public float transitionTime = 1f;   // Length of the animation

    [Header("Scene Settings")]
    public string nextSceneName = "PostMentorMadnessBuild"; // Replace with your scene name

    public void OnPlayButtonPressed()
    {
        StartCoroutine(PlayAnimationAndLoadSceneAsync());
    }

    IEnumerator PlayAnimationAndLoadSceneAsync()
    {
        // Trigger the image animation
        image1Animator.SetTrigger(triggerName);

        // Wait for animation to finish
        yield return new WaitForSeconds(transitionTime);

        // Load the next scene
        SceneManager.LoadSceneAsync(nextSceneName);
    }
}
