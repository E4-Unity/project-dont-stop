using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D.Animation;

public class Enemy : MonoBehaviour
{
    // 상태
    [SerializeField, ReadOnly] Rigidbody2D m_Target;
    [SerializeField, ReadOnly] int m_Type = 0;
    [SerializeField, ReadOnly] bool bIsDead;
    [SerializeField, ReadOnly] int m_Health;
    
    // 에디터 할당
    [SerializeField] EnemyData[] m_EnemyData;

    // 컴포넌트
    Rigidbody2D m_Rigidbody;
    SpriteRenderer m_SpriteRenderer;
    SpriteLibrary m_SpriteLibrary;
    Animator m_Animator;
    Collider2D m_Collider;
    SortingGroup m_SortingGroup;

    // 프로퍼티
    float Speed => m_EnemyData[m_Type].Speed;

    int Health
    {
        get => m_Health;
        set
        {
            if(value > MaxHealth)
                m_Health = MaxHealth;
            else if (value <= 0)
            {
                m_Health = 0;
                bIsDead = true;
                Dead();
            }
            else
                m_Health = value;
        }
    }
    int MaxHealth => m_EnemyData[m_Type].MaxHealth;
    SpriteLibraryAsset GetSpriteLibraryAsset() => m_EnemyData[m_Type].GetSpriteLibraryAsset();
    
    /* 버퍼 시작 */
    // FixedUpdate
    Vector2 position;
    Vector2 dir;
    
    // KnockBack
    WaitForFixedUpdate waitForFixedUpdate;
    Vector3 knockBackDir;
    /* 버퍼 종료 */
    
    // MonoBehaviour
    void Awake()
    {
        // 컴포넌트 할당
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteLibrary = GetComponent<SpriteLibrary>();
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<Collider2D>();
        m_SortingGroup = GetComponent<SortingGroup>();
        
        // 변수 할당
        waitForFixedUpdate = new WaitForFixedUpdate();
    }
    
    void FixedUpdate()
    {
        // 게임 정지
        if (GameManager.Get().IsPaused) return;

        if (bIsDead || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;
        
        position = m_Rigidbody.position;
        dir = m_Target.position - position;
        Vector2 next = Speed * Time.fixedDeltaTime * dir.normalized;
        m_Rigidbody.MovePosition(position + next);
        m_Rigidbody.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        // 게임 정지
        if (GameManager.Get().IsPaused) return;

        m_SpriteRenderer.flipX = dir.x < 0;
    }

    void OnEnable()
    {
        // 부활
        Revive();

        // 목표물을 플레이어로 설정
        if(GameManager.Get())
            m_Target = GameManager.Get().GetPlayer().GetComponent<Rigidbody2D>();
    }

    public void Init(SpawnData _spawnData)
    {
        m_Type = Mathf.Min(_spawnData.Type, m_EnemyData.Length - 1);
        m_SpriteLibrary.spriteLibraryAsset = GetSpriteLibraryAsset();
    }
    
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (bIsDead) return;
        if (!_other.CompareTag("Bullet")) return;

        Health -= _other.GetComponent<Bullet>().Damage;
        m_Animator.SetTrigger("Hit");
        StartCoroutine(KnockBack());
        
        if (GameManager.Get().IsPaused)
            return;
        AudioManager.Get().PlaySfx(AudioManager.Sfx.Hit);
    }

    IEnumerator KnockBack()
    {
        yield return waitForFixedUpdate;
        knockBackDir = (transform.position - GameManager.Get().GetPlayer().transform.position).normalized;
        m_Rigidbody.AddForce(knockBackDir * 3, ForceMode2D.Impulse);
    }

    void Dead()
    {
        bIsDead = true;
        m_Animator.SetBool("Dead", bIsDead);
        m_Collider.enabled = false;
        m_Rigidbody.simulated = false;
        m_SortingGroup.sortingLayerName = "Dead";
        
        GameManager.Get().GetExp();

        // 게임 종료 후 Enemy Cleaner로 발생하는 죽음은 무시
        if (GameManager.Get().IsPaused)
            return;
        AudioManager.Get().PlaySfx(AudioManager.Sfx.Dead);
    }

    void Revive()
    {
        // Dead의 반대 동작
        bIsDead = false;
        m_Animator.SetBool("Dead", bIsDead);
        m_Collider.enabled = true;
        m_Rigidbody.simulated = true;
        m_SortingGroup.sortingLayerName = "Enemy";

        // 체력 설정
        m_Health = MaxHealth;
    }

    void Release()
    {
        GameManager.Get().GetPoolManager().GetPool(gameObject.GetComponent<PoolTracker>().PrefabID).Release(gameObject);
    }
}
