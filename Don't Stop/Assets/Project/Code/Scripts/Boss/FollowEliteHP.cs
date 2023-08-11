using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEliteHP : MonoBehaviour
{
    RectTransform m_Rect;
    // Start is called before the first frame update
    void Awake()
    {
        m_Rect = GetComponentInParent<RectTransform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManagerBoss.Get().spawnEnd == true)
            m_Rect.position = Camera.main.WorldToScreenPoint(GameObject.FindWithTag("Enemy").transform.position);
    }
}
