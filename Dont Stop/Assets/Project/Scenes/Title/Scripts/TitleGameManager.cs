using System.Collections;
using UnityEngine;

public class TitleGameManager : MonoBehaviour
{
    [Header("설정")]
    [SerializeField] float m_DelayTimeForChangingScene = 1f; // 씬 전환을 시작하기 전까지 대기 시간
    
    /* MonoBehaviour 가상 함수 */
    void Awake()
    {
        SceneLoadingManager.Instance.OnSceneShown += OnSceneShown_Event;
    }

    void OnDestroy()
    {
        SceneLoadingManager.Instance.OnSceneShown -= OnSceneShown_Event;
    }
    
    void OnSceneShown_Event()
    {
        // TODO 데이터 로딩
        StartCoroutine(GoToLobby());
    }

    IEnumerator GoToLobby()
    {
        yield return new WaitForSeconds(m_DelayTimeForChangingScene);
        SceneLoadingManager.Instance.ChangeScene(EBuildScene.Lobby);
    }
}
