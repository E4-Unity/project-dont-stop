using System;
using UnityEngine;

namespace Framework
{
    public class AttributeComponent : ActorComponent
    {
        /* 필드 */
        [Header("Initialization")] 
        [SerializeField] int m_MaxHealth = 100;
        
        [Header("State")]
        [SerializeField] int m_Health;
        public int Health
        {
            get => m_Health;
            set
            {
                // 이미 죽은 상태면 무시
                if (m_IsDead) return;

                // 변화량 저장
                diff = value - m_Health;
                
                if (value <= 0)
                {
                    m_IsDead = true;
                    m_Health = 0;
                    
                    // OnDead 이벤트 발송
                    OnDead?.Invoke();
                }
                else if (value >= m_MaxHealth)
                {
                    m_Health = m_MaxHealth;
                }
                else
                {
                    m_Health = value;
                }
                
                // OnUpdate 이벤트 발송
                OnUpdate?.Invoke(diff);
                
            }
        }
        
        [SerializeField] bool m_IsDead;
        public bool IsDead => m_IsDead;
        
        /* 버퍼 */
        int diff; // Health에서 사용
        
        /* 이벤트 */
        public event Action<int> OnUpdate;
        public event Action OnDead;
        
        /* 이벤트 함수 */
        void OnTakeDamage_Event(float _damage, Actor _damageCauser, Actor _instigator)
        {
            // 데미지 연산
            Health -= Mathf.RoundToInt(_damage);
        }

        /* ActorComponent 가상 함수 */
        protected override void InitializeComponent()
        {
            Health = m_MaxHealth;
        }

        protected override void BindEventFunctions()
        {
            Owner.OnTakeDamage += OnTakeDamage_Event;
        }
    }
}