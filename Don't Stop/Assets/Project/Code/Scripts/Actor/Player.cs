using Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;

public class Player : Actor
{
    /* 컴포넌트 */
    Rigidbody2D m_Rigidbody;
    SpriteRenderer m_Renderer;
    Animator m_Animator;
    SpriteLibrary m_SpriteLibrary;

    protected Rigidbody2D GetRigidbody() => m_Rigidbody;
    protected SpriteRenderer GetSpriteRenderer() => m_Renderer;
    protected Animator GetAnimator() => m_Animator;
    protected SpriteLibrary GetSpriteLibrary() => m_SpriteLibrary;

    /* 레퍼런스 */
    [Header("[Reference]")]
    [SerializeField] SpriteRenderer[] m_Hands;
    [SerializeField] GameObject[] m_ObjectsToDeactivate;

    public SpriteRenderer[] GetHands() => m_Hands;

    /* 필드 */
    [Header("[Initialization]")]
    [SerializeField] float m_Speed = 3f;

    [SerializeField] SpriteLibraryAsset[] m_SpriteLibraryAssets;

    // 버퍼
    Vector2 inputValue;
    bool newFlip;

    /* 프로퍼티 */
    public float Speed
    {
        get => m_Speed;
        set => m_Speed = value;
    }
    
    public Vector2 InputValue => inputValue;

    /* 메서드 */
    void FlipHand(SpriteRenderer _hand)
    {
        // null 체크
        if (_hand is null) return;

        // flip 반전
        _hand.flipX = !_hand.flipX;

        // 이동
        Transform handTransform = _hand.gameObject.transform;
        handTransform.localPosition = Vector3.Scale(handTransform.localPosition, new Vector3(-1, 1, 1));

        // 회전
        Quaternion quaternion = Quaternion.identity;
        quaternion.eulerAngles = Vector3.Scale(handTransform.localRotation.eulerAngles, new Vector3(1, 1, -1));
        handTransform.localRotation = quaternion;
    }

    // 초기화
    void Init()
    {
        m_Speed *= SurvivalGameState.Get().GetStatsComponent().TotalStats.MovementSpeed;
        m_SpriteLibrary.spriteLibraryAsset = m_SpriteLibraryAssets[PlayerState.Get().CharacterData.DefinitionID];
    }
    
    /* Input System */
    void OnMove(InputValue _inputValue)
    {
        inputValue = _inputValue.Get<Vector2>();
    }

    /* MonoBehaviour */

    #region Actor

    protected override void AssignComponents()
    {
        base.AssignComponents();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Renderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
        m_SpriteLibrary = GetComponent<SpriteLibrary>();
    }

    protected override void BindEventFunctions()
    {
        base.BindEventFunctions();
        TimeManager.Get().OnPause += OnPause_Event;
        TimeManager.Get().OnResume += OnResume_Event;
    }

    #endregion

    #region Monobehaviour

    protected override void Start_Event()
    {
        base.Start_Event();
        Init();
    }

    void FixedUpdate()
    {
        // 움직임
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Speed * Time.fixedDeltaTime * inputValue.normalized);
    }

    void LateUpdate()
    {
        // 애니메이션 변수 업데이트
        m_Animator.SetFloat("Speed", inputValue.magnitude);
        
        /* 움직임에 따라 바라보는 방향 전환 */

        // 입력 확인
        if (inputValue.x == 0) return;

        // 반전 여부 확인
        newFlip = inputValue.x < 0;
        if (m_Renderer.flipX == newFlip) return;

        // 플레이어와 양 손 반전 
        m_Renderer.flipX = !m_Renderer.flipX;
        foreach(var hand in m_Hands)
        {
            FlipHand(hand);
        }
    }

    void OnCollisionStay2D(Collision2D _other)
    {
        // 이미 죽은 상태라면 무시
        if (SurvivalGameManager.Get().Health < 0) return;
        
        // 데미지 적용
        SurvivalGameManager.Get().Health -= Time.deltaTime * 10;

        // Dead
        if (SurvivalGameManager.Get().Health < 0)
        {
            foreach (var obj in m_ObjectsToDeactivate)
            {
                obj.SetActive(false);
            }
            
            m_Animator.SetTrigger("Dead");
            SurvivalGameManager.Get().GameOver();
        }
    }
    
    #endregion
}
