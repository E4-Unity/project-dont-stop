using UnityEngine;
using UnityEngine.UI;

public enum EUISoundType
{
    None,
    SelectButton
}

public class UISoundButton : MonoBehaviour
{
    /* 컴포넌트 */
    UISoundManager soundManager;
    Button button;
    
    /* 필드 */
    [Header("Button Click Sound")]
    [SerializeField] EUISoundType soundType;

    /* MonoBehaviour */
    protected virtual void Awake()
    {
        // 컴포넌트 할당
        soundManager = GlobalGameManager.Instance.GetUISoundManager();
        button = GetComponent<Button>();

        // 유효성 검사
        if (button is null)
        {
#if UNITY_EDITOR
            Debug.LogError("Button Sound 컴포넌트는 Button 컴포넌트와 동일한 오브젝트에 부착되어야 합니다.");
#endif
        }
        else
        {
            // 이벤트 바인딩
            button.onClick.AddListener(OnButtonClick);
        }
    }

    protected void OnDestroy()
    {
        // 유효성 검사
        if (!button) return;
        
        // 이벤트 언바인딩
        button.onClick.RemoveListener(OnButtonClick);
    }

    /* 이벤트 함수 */
    protected virtual void OnButtonClick()
    {
        soundManager.PlaySound(soundType);
    }
}
