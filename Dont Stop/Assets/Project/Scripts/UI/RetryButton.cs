using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    /* 컴포넌트 */
    SceneLoadingManager sceneLoadingManager;
    
    /* MonoBehaviour */
    void Awake()
    {
        // 컴포넌트 할당
        sceneLoadingManager = GlobalGameManager.Instance.GetSceneLoadingManager();
    }
    
    public void Retry()
    {
        sceneLoadingManager.ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }
}
