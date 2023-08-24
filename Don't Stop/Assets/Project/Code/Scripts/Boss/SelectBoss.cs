using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBoss : MonoBehaviour
{
    ItemBoss[] m_Items;

    private void Awake()
    {
        m_Items = GetComponentsInChildren<ItemBoss>(true);
    }
    public void Select(int _index)
    {
        m_Items[_index].OnClick();
    }
}
