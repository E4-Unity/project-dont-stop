using E4.Utility;
using UnityEngine;

public class GlobalGameManager : E4.Utility.GenericMonoSingleton<GlobalGameManager>
{
    /* 정적 메서드 */
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void GenerateGlobalGameManagerPrefabInstance()
    {
        // Resources 폴더에서 Global Game Manager 프리팹 로드 후 생성
        var prefab = Resources.Load<GameObject>("Global Game Manager");
        if (prefab is null)
        {
#if UNITY_EDITOR
            Debug.LogAssertion("Resources 폴더에서 Global Game Manager 프리팹을 찾을 수 없습니다.");
#endif
            return;
        }
        
        Instantiate(prefab);
    }
    
    /* 컴포넌트 */
    SceneLoadingManager sceneLoadingManager;
    PlayerState playerState;
    TimeManager timeManager;
    UISoundManager uiSoundManager;

    public SceneLoadingManager GetSceneLoadingManager() => sceneLoadingManager;
    public PlayerState GetPlayerState() => playerState;
    public TimeManager GetTimeManager() => timeManager;
    public UISoundManager GetUISoundManager() => uiSoundManager;

    /* GenericMonoSingleton */
    protected override void InitializeComponent()
    {
        base.InitializeComponent();
        
        // 모든 씬에서 사용
        DontDestroyOnLoad(gameObject);
        
        // 컴포넌트 할당
        sceneLoadingManager = GetComponentInChildren<SceneLoadingManager>();
        playerState = GetComponentInChildren<PlayerState>();
        timeManager = GetComponentInChildren<TimeManager>();
        uiSoundManager = GetComponentInChildren<UISoundManager>();
    }
    
    /* MonoBehaviour */
    void LateUpdate()
    {
        DataManager.HandleRequests();
    }
}
