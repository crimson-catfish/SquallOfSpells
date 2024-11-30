using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T instance {
        get
        {
            if (_instance == null)
            {
                _instance = FindOrCreateInstance();
            }

            return _instance;
        }
    }

    private static T FindOrCreateInstance()
    {
        var instance = FindObjectOfType<T>();

        if (instance != null)
        {
            DontDestroyOnLoad(instance);

            return instance;
        }

        var name = typeof(T).Name + " Singleton";
        var containerGameObject = new GameObject(name);
        var singletonComponent = containerGameObject.AddComponent<T>();

        return singletonComponent;
    }
}