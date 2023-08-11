using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerBoss : MonoBehaviour
{
    [SerializeField] float m_ScanRange;
    [SerializeField] LayerMask m_TargetLayer;

    RaycastHit2D[] m_Targets;
    [SerializeField, ReadOnly] Transform m_MainTarget;

    public Transform MainTarget => m_MainTarget;

    void FixedUpdate()
    {
        m_Targets = Physics2D.CircleCastAll(transform.position, m_ScanRange, Vector2.zero, 0, m_TargetLayer);
        m_MainTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float dist = 100;

        foreach (var target in m_Targets)
        {
            float curDist = Vector3.Distance(transform.position, target.transform.position);

            if (curDist < dist)
            {
                dist = curDist;
                result = target.transform;
            }
        }

        return result;
    }
}
