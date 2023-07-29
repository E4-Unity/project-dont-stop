using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // 에디터 설정
    [SerializeField] int m_WeaponID;
    [SerializeField] int m_PrefabID;
    [SerializeField] int m_Damage;
    [SerializeField] int m_Count;
    [SerializeField] float m_Speed;
    
    // 프로퍼티
    public int WeaponID => m_WeaponID;

    public float Speed
    {
        get => m_Speed;
        set => m_Speed = value;
    }

    // 상태
    float m_Timer;
    
    // 컴포넌트
    Player m_Player;
    Scanner m_Scanner;

    void Awake()
    {
        m_Player = GameManager.Get().GetPlayer();
    }

    void Update()
    {
        // 게임 정지
        if (GameManager.Get().IsPaused) return;

        switch (m_WeaponID)
        {
            case 0:
                transform.Rotate(m_Speed * Character.WeaponSpeed * Time.deltaTime * Vector3.back);
                break;
            default:
                m_Timer += Time.deltaTime;
                if (m_Timer > m_Speed * Character.WeaponRate)
                {
                    m_Timer = 0;
                    Fire();
                }
                break;
        }
    }

    public void LevelUp(int _damage, int _count)
    {
        m_Damage = _damage * (int)Character.Damage;
        m_Count += _count;
        
        if(m_WeaponID == 0)
            Arrange();
        
        m_Player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData _data)
    {
        // Basic Set
        name = "Weapon " + _data.ItemID;
        transform.parent = m_Player.transform;
        transform.localPosition = Vector3.zero;
        m_Scanner = GetComponentInParent<Scanner>(); // TODO 리팩토링 필요
        
        // Property Set
        m_WeaponID = _data.ItemID;
        m_Damage = _data.BaseDamage * (int)Character.Damage;
        m_Count = _data.BaseCount + Character.Count;
        m_PrefabID = GameManager.Get().GetPoolManager().GetPrefabID(_data.Projectile);
        
        switch (m_WeaponID)
        {
            case 0:
                m_Speed = 150;
                Arrange();
                break;
            default:
                m_Speed = 0.3f;
                break;
        }

        // Hand Set
        // TODO OnEquip 델리게이트로 Player에서 직접 할당 예정
        var hand = m_Player.GetHands()[(int)_data.Type];
        hand.sprite = _data.WeaponSprite;
        hand.gameObject.SetActive(true);
        
        m_Player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Arrange()
    {
        for (int i = 0; i < m_Count; i++)
        {
            GameObject bullet;
            if (i < transform.childCount)
                bullet = transform.GetChild(i).gameObject;
            else
            {
                // Bullet 스폰 및 배치
                bullet = GameManager.Get().GetPoolManager().GetPool(m_PrefabID).Get();
                bullet.transform.parent = transform;
            }
            
            bullet.transform.localPosition = Vector3.zero;
            bullet.transform.localRotation = Quaternion.identity;

            Vector3 rot = 360f * i / m_Count * Vector3.forward;
            bullet.transform.Rotate(rot);
            bullet.transform.Translate(bullet.transform.up * 1.5f, Space.World);
            
            // Bullet 설정
            bullet.GetComponent<Bullet>().Init(m_Damage, -100, Vector3.zero); // -1은 무한 관통
            
            // Bullet 활성화
            bullet.SetActive(true); // 나중에 IObjectPool에 Finish 추가 예정
        }
    }

    void Fire()
    {
        // 목표물 확인
        if (!m_Scanner.MainTarget) return;
        
        // Bullet 스폰
        GameObject bullet = GameManager.Get().GetPoolManager().GetPool(m_PrefabID).Get();
        
        // Bullet 방향 및 속도 설정
        Vector3 position = transform.position;
        Vector3 dir = (m_Scanner.MainTarget.position - position).normalized;
        bullet.transform.position = position;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(m_Damage, m_Count, dir);

        // Bullet 활성화
        bullet.SetActive(true);
        
        AudioManager.Get().PlaySfx(AudioManager.Sfx.Range);
    }
}
