using UnityEngine;

public class JoyStickUI : MonoBehaviour
{
    /* 필드 */
    bool isEventBound;
    
    /* MonoBehaviour*/
    void Start()
    {
        BindEventFunctions();
    }

    void OnDestroy()
    {
        UnbindEventFunctions();
    }
    
    /* 이벤트 함수 */
    void OnPause_Event()
    {
        transform.localScale = Vector3.zero;
    }

    void OnResume_Event()
    {
        transform.localScale = Vector3.one;
    }

    /* 메서드 */
    void BindEventFunctions()
    {
        // 중복 호출 방지
        if (isEventBound) return;
        isEventBound = true;
        
        // TimeManger 이벤트 바인딩
        var timeManager = GlobalGameManager.Instance.GetTimeManager();
        timeManager.OnPause += OnPause_Event;
        timeManager.OnResume += OnResume_Event;
    }

    void UnbindEventFunctions()
    {
        // 중복 호출 방지
        if (!isEventBound) return;
        isEventBound = false;
        
        // TimeManger 이벤트 언바인딩
        var timeManager = GlobalGameManager.Instance.GetTimeManager();
        timeManager.OnPause -= OnPause_Event;
        timeManager.OnResume -= OnResume_Event;
    }
}
