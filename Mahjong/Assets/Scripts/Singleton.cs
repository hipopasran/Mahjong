using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                var instanceInScene = GameObject.FindObjectOfType<T>();
                instance = instanceInScene;

                DontDestroyOnLoad(instance);
                return instance;
            }

            return instance;
        }
        set
        {
            if (instance == null)
            {
                instance = value;
            }
            else
            {
                Destroy(value.gameObject);
                Debug.LogWarning($"Deleting other {typeof(T).Name} singleton instance");
            }
        }
    }

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
    }
}
