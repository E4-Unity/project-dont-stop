using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMonster : MonoBehaviour
{
    Animator EliteSlimeAnimator;
    Rigidbody2D EliteSlimeRigidbody;
    Transform playerTransform;
    float EliteSlimeMoveSpeed = 1f;

    float playerAttackDamage;
    public float maxEliteSlimeHp = 200f;
    public float EliteSlimeHp;
    public float EliteSlimeAttackDamage = 10f;

    void Awake()
    {
        EliteSlimeAnimator = GetComponent<Animator>();
        EliteSlimeRigidbody = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.Find("Player").transform;
        EliteSlimeHp = maxEliteSlimeHp;
    }

    void Update()
    {
        Vector3 EliteSlimeFlip = playerTransform.position - transform.position;

        if (GameManagerBoss.Get().portalTouch == true)
            this.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

        if (distanceToPlayer > 2) return;

        //슬라임 이동
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        EliteSlimeRigidbody.velocity = direction * EliteSlimeMoveSpeed;
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        

        if (coll.gameObject.CompareTag("Player"))
        {
            EliteSlimeHp -= 30 * Time.deltaTime;
            print(EliteSlimeHp);

            if (EliteSlimeHp <= 0)
            {
                GameManagerBoss.Get().eliteDead++;
                this.gameObject.SetActive(false);
            }
        }
    }
}