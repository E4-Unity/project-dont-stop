using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlime : MonoBehaviour
{
    Animator BossSlimeAnimator;
    SpriteRenderer BossSlimeRenderer;
    Rigidbody2D BossSlimeRigidbody;
    Transform playerTransform;
    Vector3 lastPlayerPosition;
    float BossSlimeMoveSpeed = 2f;

    float playerHp;
    float playerAttackDamage;
    public float BossSlimeHp;
    [SerializeField] bool isJumping = false;
    bool isDead = false;
    bool newFlip;

    float BossSlimeAttackCoolTime = 1f;
    float BossSlimeAttackLastTime = 0f;

    void Awake()
    {
        BossSlimeAnimator = GetComponent<Animator>();
        BossSlimeRenderer = GetComponent<SpriteRenderer>();
        BossSlimeRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.Find("Player").transform;
        lastPlayerPosition = playerTransform.position;
        playerHp = GameObject.Find("Player").GetComponent<PlayerBoss>().health;
        playerAttackDamage = GameObject.Find("Player").GetComponent<PlayerBoss>().attackDamage;
    }

    void Start()
    {
        BossSlimeHp = GameManagerBoss.Get().MaxBossHP;
    }

    void Update()
    {
        if (isDead == true) return;

        Vector3 BossSlimeFlip = playerTransform.position - transform.position;

        //슬라임 FlipX 조정
        if (BossSlimeFlip.x == 0) return;

        newFlip = BossSlimeFlip.x > 0;
        if (BossSlimeRenderer.flipX == newFlip) return;

        BossSlimeRenderer.flipX = !BossSlimeRenderer.flipX;
    }

    private void LateUpdate()
    {
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        if (isDead == true) return;

        //슬라임 이동
        if (!isJumping)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            BossSlimeRigidbody.velocity = direction * BossSlimeMoveSpeed;
            lastPlayerPosition = playerTransform.position;
            if (distanceToPlayer > 3f)
                StartCoroutine(Jump());
        }
        else
        {
            Vector3 jumpDirection = (lastPlayerPosition - transform.position).normalized;
            BossSlimeRigidbody.velocity = jumpDirection * BossSlimeMoveSpeed;
        }

        //슬라임 공격
        if (isJumping) return;

        BossSlimeAttackLastTime += Time.deltaTime;
        if (distanceToPlayer <= 3f && BossSlimeAttackCoolTime <= BossSlimeAttackLastTime)
        {
            BossSlimeAnimator.SetTrigger("BossSlimeAttack");
            BossSlimeAttackLastTime = 0f;
        }
    }

    IEnumerator Jump()
    {
        BossSlimeAnimator.SetTrigger("BossSlimeJump");
        isJumping = true;
        yield return new WaitForSeconds(0.5f);
        BossSlimeMoveSpeed = 5f;
        yield return new WaitForSeconds(0.7f);
        BossSlimeMoveSpeed = 2f;
        isJumping = false;
        yield return new WaitForSeconds(5f);
    }

    void OnTriggerEnter(Collider other)
    {
        GameManagerBoss.Get().BossHP = BossSlimeHp;

        if (other.gameObject.CompareTag("Bullet"))
        {
            BossSlimeHp -= playerAttackDamage;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameManagerBoss.Get().BossHP = BossSlimeHp;

        if (collision.gameObject.CompareTag("Player"))
        {
            BossSlimeHp -= 40 * Time.deltaTime;
        }

        if(BossSlimeHp <= 0)
        {
            StartCoroutine(Dead());
            GameManagerBoss.Get().portalTouch = false;
        }
    }

    IEnumerator Dead()
    {
        isDead = true;
        BossSlimeAnimator.SetTrigger("BossSlimeDead");
        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);
        GameManagerBoss.Get().GameVictory();
    }
}