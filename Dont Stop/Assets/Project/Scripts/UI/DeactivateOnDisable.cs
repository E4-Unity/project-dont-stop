using UnityEngine;

public class DeactivateOnDisable : MonoBehaviour
{
    // 메인 창이 꺼지면 자동으로 꺼지기
    void OnDisable()
    {
        if(gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}
