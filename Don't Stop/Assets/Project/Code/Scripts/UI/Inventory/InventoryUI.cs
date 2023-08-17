using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    #region Initialization

    [SerializeField] GameObject m_SlotListGameObject; // Inventory Slot UI의 부모
    [SerializeField] InventorySlotUI m_InventorySlotUI;
    [SerializeField] PlayerInventory m_InventoryComponent;

    #endregion

    void Awake()
    {
        m_InventoryComponent = PlayerState.Get().GetInventoryComponent();
        m_InventoryComponent.OnInventoryUpdate += Refresh;
        Refresh();
    }
    

    void Refresh()
    {
        if (m_SlotListGameObject && m_InventorySlotUI)
        {
            // 리셋
            foreach (Transform child in m_SlotListGameObject.transform)
            {
                Destroy(child.gameObject);
            }
            
            // 아이템 목록 불러오기
            foreach (var data in m_InventoryComponent.InventoryList)
            {
                var inventorySlotUi = Instantiate(m_InventorySlotUI, m_SlotListGameObject.transform);
                inventorySlotUi.Init(data);
            }
        }
    }
}
