using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    /* Component */
    RectTransform m_Rect;

    /* MonoBehaviour */
    void Awake()
    {
        m_Rect = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        m_Rect.position = Camera.main.WorldToScreenPoint(GameManager.Get().GetPlayer().transform.position);
    }
}
