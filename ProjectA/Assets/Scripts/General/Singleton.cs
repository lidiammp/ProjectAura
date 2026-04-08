using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T _instance = null;

    public static T instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = gameObject.GetComponent<T>();
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (this == _instance)
        {
            _instance = null;
        }
    }
}