using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 에디터 할당
    [SerializeField] int m_Damage = 10;
    [SerializeField] int m_Penetration = -1;

    // 프로퍼티
    public int Damage => m_Damage;

    // 컴포넌트
    Rigidbody2D m_Rigidbody;
    
    // 상태
    Vector3 m_Velocity;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        if (m_Penetration < 0) return;
        m_Rigidbody.velocity = m_Velocity;
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        // 원거리 무기
        if (!_other.CompareTag("Enemy") || m_Penetration == -100) return;
        
        // 관통 계산
        m_Penetration--;

        if (m_Penetration < 0)
        {
            if(gameObject.activeSelf)
                GameManager.Get().GetPoolManager().GetPool(gameObject.GetComponent<PoolTracker>().PrefabID).Release(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D _other)
    {
        if (!_other.CompareTag("Area")) return;
        
        if(gameObject.activeSelf)
            GameManager.Get().GetPoolManager().GetPool(gameObject.GetComponent<PoolTracker>().PrefabID).Release(gameObject);
    }

    public void Init(int _damage, int _penetration, Vector3 _velocity)
    {
        m_Damage = _damage;
        m_Penetration = _penetration;
        
        // 원거리 무기
        if (_penetration >= 0)
        {
            m_Velocity = _velocity * 15;
        }
    }
}
