using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class WinScreen : MonoBehaviour
{
    public void SetUp()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartButton()
    {
        //get name of active scene
        string currentSceneName = SceneManager.GetActiveScene().name;
        //load it up
        SceneManager.LoadScene(currentSceneName);
    }
}
