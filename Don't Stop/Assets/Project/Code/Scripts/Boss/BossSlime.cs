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

    float playerAttackDamage;
    public float BossSlimeHp;
    [SerializeField] bool isJumping = false;
    bool bossIsDead = false;
    bool newFlip;

    float BossSlimeAttackCoolTime = 1f;
    float BossSlimeAttackLastTime = 0f;

    WaitForFixedUpdate waitForFixedUpdate;
    Vector3 knockBackDir;

    void Awake()
    {
        BossSlimeAnimator = GetComponent<Animator>();
        BossSlimeRenderer = GetComponent<SpriteRenderer>();
        BossSlimeRigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        lastPlayerPosition = playerTransform.position;
        playerAttackDamage = GameObject.Find("Player").GetComponent<PlayerBoss>().attackDamage;
        BossSlimeHp = GameManagerBoss.Get().MaxBossHP;
    }

    void Update()
    {
        if (bossIsDead == true) return;

        Vector3 BossSlimeFlip = playerTransform.position - transform.position;

        if (BossSlimeFlip.x == 0) return;

        newFlip = BossSlimeFlip.x > 0;
        if (BossSlimeRenderer.flipX == newFlip) return;

        BossSlimeRenderer.flipX = !BossSlimeRenderer.flipX;
    }

    private void LateUpdate()
    {
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        if (bossIsDead == true) return;

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

    void OnTriggerEnter2D(Collider2D other)
    {
        GameManagerBoss.Get().BossHP = BossSlimeHp;

        if (other.gameObject.CompareTag("Bullet"))
        {
            BossSlimeHp -= playerAttackDamage;
            if (other.gameObject.name == "Bullet_01")
                other.gameObject.SetActive(false);
        }
        StartCoroutine(KnockBack());
        if (BossSlimeHp <= 0)
        {
            StopCoroutine(KnockBack());
            StartCoroutine(Dead());
        }
    }

    IEnumerator KnockBack()
    {
        yield return waitForFixedUpdate;
        knockBackDir = (transform.position - GameManagerBoss.Get().GetPlayer().transform.position).normalized;
        BossSlimeRigidbody.AddForce(knockBackDir * 2, ForceMode2D.Impulse);
    }

    IEnumerator Dead()
    {
        bossIsDead = true;
        BossSlimeRigidbody.velocity = Vector2.zero;
        BossSlimeAnimator.SetTrigger("BossSlimeDead");
        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);
        GameManagerBoss.Get().portalTouch = false;
        GameManagerBoss.Get().GameVictory();
    }
}