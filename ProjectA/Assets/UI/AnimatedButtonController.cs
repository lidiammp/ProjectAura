using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AnimatedButtonController : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    public void OnButtonPressed()
    {
        // Deselect the button so it doesn't stay highlighted
        EventSystem.current.SetSelectedGameObject(null);

        // Start loading the next scene
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("PostMentorMadnessBuild");
    }
}
