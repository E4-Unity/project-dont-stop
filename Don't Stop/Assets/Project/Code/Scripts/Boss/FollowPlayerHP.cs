using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerHP : MonoBehaviour
{
    RectTransform m_Rect;

    void Awake()
    {
        m_Rect = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        m_Rect.position = Camera.main.WorldToScreenPoint(GameManagerBoss.Get().GetPlayer().transform.position);
    }
}
