using System;
using UnityEngine;

public class ChangeSceneButton : MonoBehaviour
{
    bool m_Clicked;
    
    [SerializeField] EBuildScene m_TargetScene;
    [SerializeField] SceneLoadingManager.ETransitionType m_TransitionType = SceneLoadingManager.ETransitionType.Fade;
    
    /* 컴포넌트 */
    SceneLoadingManager sceneLoadingManager;
    
    /* MonoBehaviour */
    void Awake()
    {
        sceneLoadingManager = GlobalGameManager.Instance.GetSceneLoadingManager();
    }

    public void ChangeScene()
    {
        sceneLoadingManager.ChangeScene(m_TargetScene, m_TransitionType);
    }
}
