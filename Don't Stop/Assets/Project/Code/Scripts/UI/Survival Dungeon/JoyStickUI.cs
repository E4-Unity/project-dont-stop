using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickUI : MonoBehaviour
{
    #region Event Functions

    void OnStart_Event()
    {
        
    }

    void OnPause_Event()
    {
        transform.localScale = Vector3.zero;
    }

    void OnResume_Event()
    {
        transform.localScale = Vector3.one;
    }

    #endregion

    #region Method

    void BindEventFunctions()
    {
        var timeManager = TimeManager.Get();
        timeManager.OnPause += OnPause_Event;
        timeManager.OnResume += OnResume_Event;
    }

    void UnbindEventFunctions()
    {
        var timeManager = TimeManager.Get();
        if (timeManager)
        {
            timeManager.OnPause -= OnPause_Event;
            timeManager.OnResume -= OnResume_Event;
        }
    }

    #endregion

    #region Monobehaviour

    void Start()
    {
        BindEventFunctions();
    }

    void OnEnable()
    {
        BindEventFunctions();
    }

    void OnDisable()
    {
        UnbindEventFunctions();
    }

    #endregion
}
