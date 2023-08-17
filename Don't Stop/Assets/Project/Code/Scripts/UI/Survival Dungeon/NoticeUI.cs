using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeUI : MonoBehaviour
{
    [SerializeField] GameObject m_StageClearUI;

    void Start()
    {
        SurvivalGameManager.Get().OnStageClear += _i =>
        {
            m_StageClearUI.SetActive(true);
            StartCoroutine(Disappear(m_StageClearUI));
        };
    }

    IEnumerator Disappear(GameObject _noticeUI)
    {
        yield return new WaitForSeconds(2);
        _noticeUI.SetActive(false);
    }
}
