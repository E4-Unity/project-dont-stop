using Framework;
using UnityEngine;

public class Bullet : Actor
{
    // 컴포넌트
    Rigidbody2D m_Rigidbody;
    protected Rigidbody2D GetRigidbody() => m_Rigidbody;
    
    // 에디터 할당
    [SerializeField] int m_Damage = 10;
    [SerializeField] int m_Penetration = -1;

    // 프로퍼티
    public int Damage => m_Damage;

    // 상태
    Vector3 m_Velocity;

    #region Actor

    protected override void AssignComponents()
    {
        base.AssignComponents();
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void InitializeComponent()
    {
        base.InitializeComponent();
        if (m_Penetration < 0) return; // 근접 무기 확인
        m_Rigidbody.velocity = m_Velocity;
    }

    #endregion

    #region Monobehaviour

    void OnTriggerEnter2D(Collider2D _other)
    {
        // 한 프레임에 여러 물체와 충돌하는 경우, SetActive(false) 호출 이후의 이벤트는 무시
        if (!gameObject.activeSelf) return;
        
        // 원거리 무기
        if (!_other.CompareTag("Enemy") || m_Penetration == -100) return;
        
        // 관통 계산
        m_Penetration--;

        if (m_Penetration < 0)
        {
            if(gameObject.activeSelf)
                PoolManager.ReleaseInstance(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D _other)
    {
        // 한 프레임에 여러 물체와 충돌하는 경우, SetActive(false) 호출 이후의 이벤트는 무시
        if (!gameObject.activeSelf) return;
        
        // Dead Zone
        if (!_other.CompareTag("Area")) return;

        PoolManager.ReleaseInstance(gameObject);
    }

    #endregion

    #region API

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

    #endregion
}
