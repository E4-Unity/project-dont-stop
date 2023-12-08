using Framework;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    #region Component

    Image m_DisplayIcon;

    #endregion

    #region State

    [Header("State")] 
    [SerializeReference] UAttributeDataBase m_Data;
    [SerializeField, ReadOnly] UWeaponData m_WeaponData;
    [SerializeField, ReadOnly] UGearData m_GearData;

    #endregion

    public void Init(UAttributeDataBase _data)
    {
        m_Data = _data;
        m_DisplayIcon = GetComponentsInChildren<Image>()[1];
        Refresh();
    }

    public void Equip()
    {
        GlobalGameManager.Instance.GetPlayerState().GetInventoryComponent().Equip(m_Data);
        SystemAudioManager.Get().PlaySelectButton();
    }

    void Refresh()
    {
        if (m_Data is UWeaponData weaponData)
            m_DisplayIcon.sprite = weaponData.Icon;
        else if (m_Data is UGearData gearData)
            m_DisplayIcon.sprite = gearData.Icon;
    }
}
