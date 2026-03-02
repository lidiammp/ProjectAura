using UnityEngine;
using UnityEngine.SceneManagement;

public static class BootStrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute() => Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Systems")));
}