using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    bool m_Clicked;
    
    [SerializeField] ESceneList m_TargetScene;

    /* 이벤트 함수 */
    void OnRefresh_Event(float _loadingRate)
    {
        if (Mathf.Approximately(_loadingRate, 1f))
        {
            LevelManager.Get().OnRefresh -= OnRefresh_Event;
            LevelManager.Get().OnFinish.Invoke(false);
        }
    }
    
    public void OnChangeScene()
    {
        if (m_Clicked) return;
        m_Clicked = true;
        LevelManager.Get().OnRefresh += OnRefresh_Event;
        LevelManager.Get().ChangeScene((int)m_TargetScene);
    }

    public void ReloadScene()
    {
        LevelManager.Get().OnRefresh += OnRefresh_Event;
        LevelManager.Get().ChangeScene(SceneManager.GetActiveScene().buildIndex);
    }
}
