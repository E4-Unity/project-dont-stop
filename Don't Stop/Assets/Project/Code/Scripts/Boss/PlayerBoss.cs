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
    [SerializeField] float m_Speed = 1f;

    [SerializeField] SpriteLibraryAsset[] m_SpriteLibraryAssets;

    Vector2 inputValue;
    bool newFlip;

    public float health;
    public float attackDamage = 20f;

    public float Speed
    {
        get => m_Speed;
        set => m_Speed = value;
    }

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

    void OnEnable()
    {
        m_SpriteLibrary.spriteLibraryAsset = m_SpriteLibraryAssets[GameManagerBoss.Get().PlayerID];
    }

    void FixedUpdate()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Speed * Time.fixedDeltaTime * inputValue.normalized);
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
        GameManagerBoss.Get().Health = health;

        if (_other.gameObject.CompareTag("Enemy"))
        {
            //health -= Time.deltaTime * 20;
            //print(health);
        }
        else if(_other.gameObject.CompareTag("Boss"))
        {
            health -= Time.deltaTime * 40;
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
