using UnityEngine;

public class SurvivalGameState : GameState<SurvivalGameState>
{
    #region Components

    PlayerEquipment m_EquipmentComponent;
    PlayerStats m_StatsComponent;

    public PlayerEquipment GetEquipmentComponent() => m_EquipmentComponent;
    public PlayerStats GetStatsComponent() => m_StatsComponent;

    #endregion

    #region Properties

    [SerializeField] int m_Gold;

    public int Gold
    {
        get => m_Gold;
        set => m_Gold = value;
    }
    
    [SerializeField] int m_Exp;

    public int Exp
    {
        get => m_Exp;
        set => m_Exp = value;
    }
    
    #endregion

    #region Method

    protected override void CopyPlayerState()
    {
        var playerEquipment = PlayerState.Get().GetEquipmentComponent();
        /* Equipment Component Deep Copy */
        FSavedAttributeData saveData;
        
        // Weapon
        UWeaponData weaponCopy = new UWeaponData();
        playerEquipment.WeaponData.GetSaveData(out saveData);
        weaponCopy.Init(saveData);
        m_EquipmentComponent.WeaponData = weaponCopy;

        // Gears
        foreach (var gearSlot in playerEquipment.GearSlots)
        {
            UGearData gearCopy = new UGearData();
            gearSlot.Value.GetSaveData(out saveData);
            gearCopy.Init(saveData);
            m_EquipmentComponent.GearSlots.Add(gearSlot.Key, gearCopy);
            m_EquipmentComponent.GearDataList.Add(gearCopy);
        }
    }

    protected override void UpdatePlayerState()
    {
        PlayerState.Get().GetInventoryComponent().Gold += Gold;
        PlayerState.Get().PlayerData.Exp += Exp / 10;
        DataManager.Get().SaveJsonData();
    }

    #endregion

    #region Monobehaviour

    protected override void Awake()
    {
        base.Awake();
        
        // 컴포넌트 할당 및 초기화
        m_EquipmentComponent = GetComponent<PlayerEquipment>();
        
        m_StatsComponent = GetComponent<PlayerStats>();
        GetStatsComponent().Init(GetEquipmentComponent());
    }

    #endregion
}
