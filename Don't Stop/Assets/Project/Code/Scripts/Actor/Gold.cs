using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    [SerializeField] int m_Gold = 1;
    [SerializeField] int m_DestroyTime = 20;

    public void Init(int _gold)
    {
        m_Gold = _gold;
    }

    void OnDisable()
    {
        m_Gold = 1;
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Player"))
        {
            SurvivalGameState.Get().Gold += m_Gold;
            Destroy(gameObject);
        }
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(m_DestroyTime);
        Destroy(gameObject);
    }
}
