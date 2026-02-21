using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    void Start()
    {
        // Immediately load the Splash scene
        SceneManager.LoadScene("SplashScreen");
    }
}