using UnityEngine;

public class GenericMonoSingleton<T> : MonoBehaviour where T : GenericMonoSingleton<T>
{
    #region Static

    static T Instance;

    public static T Get() => Instance;

    public static void FindInstance()
    {
        Instance ??= FindObjectOfType<T>();
    }

    #endregion

    #region Monobehaviour

    protected virtual void Awake()
    {
        var instance = GetComponent<T>();
        if (Instance is null)
        {
            Instance = instance;
        }
        else if(Instance != instance)
        {
            Destroy(gameObject);
            return;
        }
    }

    protected virtual void OnDestroy()
    {
        if (Instance == GetComponent<T>())
            Instance = null;
    }

    #endregion
}
