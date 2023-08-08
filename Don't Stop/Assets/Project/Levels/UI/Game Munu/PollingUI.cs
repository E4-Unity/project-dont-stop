using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PollingUI : MonoBehaviour
{
    [SerializeField] float m_PollingTime = 0.1f;
    [SerializeField] WaitForSeconds pollingTime;

    protected void Awake()
    {
        pollingTime = new WaitForSeconds(m_PollingTime);
        Awake_Event();
    }
    
    protected virtual void Awake_Event(){}

    protected void OnEnable()
    {
        Refresh();
        StartCoroutine(Polling());
        OnEnable_Event();
    }
    
    protected virtual void OnEnable_Event(){}

    IEnumerator Polling()
    {
        yield return null;
        Refresh();
        
        while (true)
        {
            yield return pollingTime;
            Refresh();
        }
    }

    protected abstract void Refresh();
}
