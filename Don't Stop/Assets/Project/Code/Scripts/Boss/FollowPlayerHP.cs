using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerHP : MonoBehaviour
{
    RectTransform m_Rect;
    // Start is called before the first frame update
    void Awake()
    {
        m_Rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_Rect.position = Camera.main.WorldToScreenPoint(GameManagerBoss.Get().GetPlayer().transform.position);
    }
}
