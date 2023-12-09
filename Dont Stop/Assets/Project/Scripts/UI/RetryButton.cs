using UnityEngine.SceneManagement;

public class RetryButton : UISoundButton
{
    /* 컴포넌트 */
    SceneLoadingManager sceneLoadingManager;
    
    /* MonoBehaviour */
    protected override void Awake()
    {
        base.Awake();
        
        // 컴포넌트 할당
        sceneLoadingManager = GlobalGameManager.Instance.GetSceneLoadingManager();
    }
    
    /* UISoundButton */
    protected override void OnButtonClick()
    {
        base.OnButtonClick();
        
        Retry();
    }

    /* API */
    public void Retry()
    {
        sceneLoadingManager.ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }
}
