using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;

public class PlayerBoss : MonoBehaviour
{
    Rigidbody2D m_Rigidbody;
    SpriteRenderer m_Renderer;
    Animator m_Animator;
    SpriteLibrary m_SpriteLibrary;
    protected Rigidbody2D GetRigidbody() => m_Rigidbody;
    protected SpriteRenderer GetSpriteRenderer() => m_Renderer;
    protected Animator GetAnimator() => m_Animator;
    public SpriteLibrary GetSpriteLibrary() => m_SpriteLibrary;

    [Header("[Reference]")]
    [SerializeField] SpriteRenderer[] m_Hands;
    [SerializeField] GameObject[] m_ObjectsToDeactivate;

    public SpriteRenderer[] GetHands() => m_Hands;

    [Header("[Initialization]")]
    [SerializeField] public float attack;
    [SerializeField] public float defense;
    [SerializeField] public float attackSpeed;
    [SerializeField] public float health;
    [SerializeField] public float speed;

    public float attackDamage;

    [SerializeField] SpriteLibraryAsset[] m_SpriteLibraryAssets;

    Vector2 inputValue;
    bool newFlip;
    

    void FlipHand(SpriteRenderer _hand)
    {
        if (_hand is null) return;

        _hand.flipX = !_hand.flipX;

        Transform handTransform = _hand.gameObject.transform;
        handTransform.localPosition = Vector3.Scale(handTransform.localPosition, new Vector3(-1, 1, 1));

        Quaternion quaternion = Quaternion.identity;
        quaternion.eulerAngles = Vector3.Scale(handTransform.localRotation.eulerAngles, new Vector3(1, 1, -1));
        handTransform.localRotation = quaternion;
    }

    void OnMove(InputValue _inputValue)
    {
        inputValue = _inputValue.Get<Vector2>();
    }

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
        m_SpriteLibrary = GetComponent<SpriteLibrary>();
    }

    void Start()
    {
        m_SpriteLibrary.spriteLibraryAsset = m_SpriteLibraryAssets[PlayerState.Get().CharacterData.Definition.ID];

        attack = 500f;
        defense = 500f;
        attackSpeed = 1f;
        health = 1000f;
        speed = 2.5f;
    }

    void Update()
    {
        GameManagerBoss.Get().Health = health;
        attackDamage = attack / 5;
    }

    void FixedUpdate()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + speed * Time.fixedDeltaTime * inputValue.normalized);
    }

    void LateUpdate()
    {
        m_Animator.SetFloat("Speed", inputValue.magnitude);

        if (inputValue.x == 0) return;

        newFlip = inputValue.x < 0;
        if (m_Renderer.flipX == newFlip) return;

        m_Renderer.flipX = !m_Renderer.flipX;
        foreach (var hand in m_Hands)
        {
            FlipHand(hand);
        }
    }

    void OnCollisionStay2D(Collision2D _other)
    {
        if (_other.gameObject.CompareTag("Enemy"))
        {
            health -= Time.deltaTime * 200;
        }
        else if(_other.gameObject.CompareTag("Boss"))
        {
            health -= Time.deltaTime * 400;
        }

        if (health <= 0)
        {
            foreach (var obj in m_ObjectsToDeactivate)
            {
                obj.SetActive(false);
            }
            
            m_Animator.SetTrigger("Dead");
            GameManagerBoss.Get().GameOver();
        }
    }
}
