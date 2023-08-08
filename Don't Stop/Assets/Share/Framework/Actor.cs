using System;
using UnityEngine;

namespace Framework
{
    public abstract class BaseObject : MonoBehaviour
    {
        /*
         * 레퍼런스 할당 혹은 초기값 설정을 수동으로 할 필요가 있는 경우 true 체크
         * Awake와 OnDisable에서 enabled = false를 자동으로 호출한다.
         * 
         * 이를 통해 OnEnable 호출 전에 자체 Init 함수를 호출할 수 있다.
         * 단, 자체 Init 함수에서 enabled = true를 호출해주어야 한다.
         */
        bool m_IsPaused = false;

        public bool IsPaused
        {
            get => m_IsPaused;
            set => m_IsPaused = value;
        }
        
        bool m_IsStarted = false;

        #region Interface

        #region Initialization

        /*
         * 컴포넌트와 레퍼런스의 차이 (공식 문서 내용이 아니라 개인적인 분류입니다.)
         * 컴포넌트는 동일한 GameObject에 부착되어있는 컴포넌트를 말하며,
         * 레퍼런스는 다른 GameObject에 부착되어있는 컴포넌트 혹은 GameObject 그 자체를 의미합니다.
         */
        protected virtual void AssignComponents(){}
        // 외부 주입을 하지 않는 경우 직접 레퍼런스를 할당하는 메서드
        protected virtual void GetReferences(){}
        
        // 레퍼런스 변수들이 모두 초기화된 이후에 진행하는 초기화 관련 메서드 목록
        protected virtual void InitializeComponent(){}
        protected virtual void BindEventFunctions(){} 
        protected virtual void UnbindEventFunctions(){}

        #endregion

        #region Event Function

        protected virtual void OnPause_Event()
        {
            m_IsPaused = true;
            enabled = false;
        }

        protected virtual void OnResume_Event()
        {
            enabled = true;
        }

        /*
         * 위의 초기화 관련 메서드들을 반드시 호출하기 위해 Wrapper 방식을 사용하였다.
         * 이에 자손 클래스에서 MonoBehaviour 가상 함수들을 간접적으로 사용할 수 있도록 대체 함수들을 제공
         */
        protected virtual void Awake_Event(){}
        protected virtual void Start_Event(){}
        protected virtual void OnEnable_Event(){}
        protected virtual void OnDisable_Event(){}

        #endregion
        
        #endregion

        #region Monobehaviour

        /* MonoBehaviour 가상 함수 */
        protected void Awake()
        {
            // 컴포넌트 할당
            AssignComponents();
            GetReferences();
            Awake_Event();
        }

        protected void Start()
        {
            m_IsStarted = true;
            OnEnable();
            
            Start_Event();
        }
        protected void OnEnable()
        {
            // Start 호출이 되기 전에는 무시
            if (!m_IsStarted) return;
            
            // OnPause_Event가 발생한 경우에는 OnEnable 무시
            if (m_IsPaused)
            {
                m_IsPaused = false;
                return;
            }
            
            // 초기화
            InitializeComponent();
            
            // 이벤트 구독
            BindEventFunctions();
            OnEnable_Event();
        }

        protected void OnDisable()
        {
            // OnPause_Event가 발생한 경우에는 OnEnable 무시
            if (m_IsPaused) return;
            
            // 이벤트 구독 해지
            UnbindEventFunctions();
            OnDisable_Event();
        }

        #endregion
    }
    
    public abstract class Actor : BaseObject
    {
        /* Static */
        public static Actor PlayerActor { get; private set; }
        
        /* 프로퍼티 */
        public Actor Owner { get; set; }
        public Actor Instigator { get; set; }

        /* 이벤트 */
        public event Action<float, Actor, Actor> OnTakeDamage;

        /* 메서드 */
        protected virtual void TakeDamage(float _damage, Actor _damageCauser, Actor _instigator) { }
        
        /* API */
        public static void ApplyDamage(Actor _target, float _damage, Actor _damageCauser, Actor _instigator)
        {
            // 데미지가 0이면 무시한다
            if (Mathf.Approximately(_damage, 0)) return;
            
            _target.OnTakeDamage?.Invoke(_damage, _damageCauser, _instigator);
        }

        protected Actor()
        {
            OnTakeDamage += TakeDamage;
        }
        
        /* Base Object 가상 함수 */
        protected override void GetReferences()
        {
            base.GetReferences();
            if (CompareTag("Player") && PlayerActor is null)
                PlayerActor = this;
        }
    }

    [RequireComponent(typeof(Actor))]
    public abstract class ActorComponent : BaseObject
    {
        /* 레퍼런스 */
        [Header("Reference")]
        [SerializeField] Actor m_Owner;
        protected Actor Owner => m_Owner;
        
        /* Base Object 가상 함수 */
        protected override void AssignComponents()
        {
            m_Owner = GetComponent<Actor>();
        }
    }
}