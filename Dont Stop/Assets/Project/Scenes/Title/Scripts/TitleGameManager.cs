using System.Collections;
using UnityEngine;

public class TitleGameManager : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] float m_DelayTimeForChangingScene = 1f; // 씬 전환을 시작하기 전까지 대기 시간
    
    /* 컴포넌트 */
    SceneLoadingManager sceneLoadingManager;
    
    /* MonoBehaviour 가상 함수 */
    void Awake()
    {
        sceneLoadingManager = GlobalGameManager.Instance.GetSceneLoadingManager();
        sceneLoadingManager.OnSceneShown += OnSceneShown_Event;
    }

    void OnDestroy()
    {
        sceneLoadingManager.OnSceneShown -= OnSceneShown_Event;
    }
    
    void OnSceneShown_Event()
    {
        // TODO 데이터 로딩
        StartCoroutine(GoToLobby());
    }

    IEnumerator GoToLobby()
    {
        yield return new WaitForSeconds(m_DelayTimeForChangingScene);
        sceneLoadingManager.ChangeScene(EBuildScene.Lobby);
    }
}
