using UnityEngine;

public class ChangeSceneButton : MonoBehaviour
{
    bool m_Clicked;
    
    [SerializeField] EBuildScene m_TargetScene;
    [SerializeField] SceneLoadingManager.ETransitionType m_TransitionType = SceneLoadingManager.ETransitionType.Fade;

    public void ChangeScene()
    {
        SceneLoadingManager.Instance.ChangeScene(m_TargetScene, m_TransitionType);
    }
}
