using UnityEngine;

public class ChangeSceneButton : UISoundButton
{
    /* 필드 */
    [Header("Change Scene")]
    [SerializeField] EBuildScene m_TargetScene;
    [SerializeField] SceneLoadingManager.ETransitionType m_TransitionType = SceneLoadingManager.ETransitionType.Fade;
    
    /* 컴포넌트 */
    SceneLoadingManager sceneLoadingManager;
    
    /* MonoBehaviour */
    protected override void Awake()
    {
        base.Awake();
        
        sceneLoadingManager = GlobalGameManager.Instance.GetSceneLoadingManager();
    }
    
    /* UISoundButton */
    protected override void OnButtonClick()
    {
        base.OnButtonClick();
        
        ChangeScene();
    }

    /* API */
    public void ChangeScene()
    {
        sceneLoadingManager.ChangeScene(m_TargetScene, m_TransitionType);
    }
}
