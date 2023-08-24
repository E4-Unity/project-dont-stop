using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonster : MonoBehaviour
{
    Animator EliteSlimeAnim;
    Rigidbody2D EliteSlimeRigidbody;
    Transform playerTransform;
    float EliteSlimeMoveSpeed = 1f;

    float playerAttackDamage;
    public float EliteSlimeHp;
    bool eliteIsDead = false;

    WaitForFixedUpdate waitForFixedUpdate;
    Vector3 knockBackDir;

    void Awake()
    {
        EliteSlimeAnim = GetComponent<Animator>();
        EliteSlimeRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.Find("Player").transform;
        playerAttackDamage = GameObject.Find("Player").GetComponent<PlayerBoss>().attackDamage;
        EliteSlimeHp = GameManagerBoss.Get().GetComponent<GameManagerBoss>().MaxEliteHP;
    }

    void Update()
    {
        Vector3 EliteSlimeFlip = playerTransform.position - transform.position;

        if (GameManagerBoss.Get().portalTouch == true)
            this.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (eliteIsDead == true) return;

        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        if (distanceToPlayer > 3.5f) return;

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        EliteSlimeRigidbody.velocity = direction * EliteSlimeMoveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Bullet"))
        {
            if (EliteSlimeHp <= 0 && eliteIsDead == false)
            {
                eliteIsDead = true;
                StopCoroutine(KnockBack());
                this.gameObject.GetComponent<Collider2D>().enabled = false;
                EliteSlimeRigidbody.velocity = Vector2.zero;
                EliteSlimeAnim.SetTrigger("Dead");
                Invoke("Dead", 1f);
                GameManagerBoss.Get().eliteDead++;
            }
            else if (EliteSlimeHp > 0 && eliteIsDead == false)
            {
                EliteSlimeAnim.SetTrigger("Hit");
                EliteSlimeHp -= playerAttackDamage;
                StartCoroutine(KnockBack());
                if (coll.gameObject.name == "Bullet_01(Clone)")
                    coll.gameObject.SetActive(false);
            }
        }
    }

    IEnumerator KnockBack()
    {
        yield return waitForFixedUpdate;
        knockBackDir = (transform.position - GameManagerBoss.Get().GetPlayer().transform.position).normalized;
        EliteSlimeRigidbody.AddForce(knockBackDir * 2, ForceMode2D.Impulse);
    }

    void Dead()
    {
        this.gameObject.SetActive(false);
    }
}