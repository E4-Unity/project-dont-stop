using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMonoSingleton<T> : MonoBehaviour where T : class
{
    /* Static */
    static T Instance;
    public static T Get() => Instance;

    /* Initialization */
    [Header("Initialization")] 
    [SerializeField] bool m_DontDestroyOnLoad = true;

    /* MonoBehaviour */
    protected void Awake()
    {
        if (Instance is null)
        {
            Instance = GetComponent<T>();
            if(m_DontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
            Awake_Implementation();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected void OnDestroy()
    {
        if (!m_DontDestroyOnLoad && Instance == GetComponent<T>())
            Instance = null;
        
        OnDestroy_Implementation();
    }

    protected virtual void Awake_Implementation()
    {
        
    }

    protected virtual void OnDestroy_Implementation()
    {
        
    }
}
