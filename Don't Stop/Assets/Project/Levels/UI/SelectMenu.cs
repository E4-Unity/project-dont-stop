using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMenu : MonoBehaviour
{
    [SerializeField] GameObject[] m_MenuUIs;

    public void OnSelectMenu(int _index)
    {
        if (_index > m_MenuUIs.Length) return;

        foreach (var menuUI in m_MenuUIs)
        {
            menuUI.SetActive(false);
        }
        
        m_MenuUIs[_index].SetActive(true);
    }
}
