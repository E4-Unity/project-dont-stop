using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    [SerializeField] GameObject[] m_Title_UIs;

    /* API */
    public void Lose()
    {
        m_Title_UIs[0].SetActive(true);
    }

    public void Win()
    {
        m_Title_UIs[1].SetActive(true);
    }
}
